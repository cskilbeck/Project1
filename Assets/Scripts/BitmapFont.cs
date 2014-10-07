using UnityEditor;
using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[CustomEditor(typeof(BitmapFont))]
[Serializable]
public class BitmapFontEditor : Editor
{
    public UnityEngine.Object source;

    [SerializeField]
    public string assetPath;

    public override void OnInspectorGUI()
    {
        BitmapFont f = target as BitmapFont;
        EditorGUILayout.BeginVertical();
        source = EditorGUILayout.ObjectField("XML:", source, typeof(TextAsset), false);
        EditorGUILayout.IntField("Height:", f.Height);
        EditorGUILayout.EndVertical();
        if (GUI.changed)
        {
            assetPath = AssetDatabase.GetAssetPath(source);
            Debug.Log("Source:" + source.name + " at " + assetPath);
            f.ImportXML(source as TextAsset);
        }
    }

    public void OnEnable()
    {
        Debug.Log("OnEnable(Editor): " + assetPath);
        if (assetPath != null)
        {
            BitmapFont f = target as BitmapFont;
            f.xmlFile = assetPath;
            source = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset));
            f.OnEnable();
        }
    }
}

[Serializable]
public class BitmapFont : ScriptableObject
{
    [SerializeField]
    public string xmlFile = "None";

    private Data data = new Data();

    public int Height
    {
        get 
        {
            if (data != null)
            {
                return data.Height;
            }
            else
            {
                return 0;
            }
        }
    }

    public void OnEnable()
    {
        Debug.Log("OnEnable: " + xmlFile);
        TextAsset t = AssetDatabase.LoadAssetAtPath(xmlFile, typeof(TextAsset)) as TextAsset;
        if (t != null)
        {
            ImportXML(t);
        }
    }

    public BitmapFont()
    {
    }

    public void ImportXML(TextAsset xml)
    {
        xmlFile = AssetDatabase.GetAssetPath(xml);
        Debug.Log("Import:" + xmlFile);
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        using (System.IO.StringReader reader = new System.IO.StringReader(xml.text))
        {
            data = serializer.Deserialize(reader) as Data;
            Debug.Log("Imported, height = " + data.Height.ToString());
        }
    }

    [MenuItem("Assets/Create/BitmapFont")]
    static public void CreateBitmapFont()
    {
        BitmapFont f = ScriptableObject.CreateInstance<BitmapFont>();
        AssetDatabase.CreateAsset(f, "Assets/BitmapFont.asset");
        AssetDatabase.SaveAssets();
    }

    [XmlRoot("BitmapFont")]
    public class Data
    {
        public class Glyph
        {
            [XmlAttribute("char")]
            public int charcode;

            [XmlAttribute]
            public int images;

            [XmlAttribute]
            public float advance;

            [XmlElement]
            public Graphic[] Graphic;
        }

        public class GlyphList
        {
            [XmlAttribute]
            public int Count;

            [XmlElement]
            public Glyph[] Glyph;
        }

        public class Layer
        {
            [XmlAttribute]
            public int offsetX;

            [XmlAttribute]
            public int offsetY;

            [XmlAttribute]
            public string color;

            public UInt32 Color
            {
                get
                {
                    UInt32 val;
                    UInt32.TryParse(color, NumberStyles.AllowHexSpecifier, null, out val);
                    return val;
                }
            }
        }

        public class LayerList
        {
            [XmlAttribute]
            public int Count;

            [XmlElement]
            public Layer[] Layer;
        }

        public class Graphic
        {
            [XmlAttribute]
            public float offsetX;

            [XmlAttribute]
            public float offsetY;

            [XmlAttribute]
            public int x;

            [XmlAttribute]
            public int y;

            [XmlAttribute]
            public int w;

            [XmlAttribute]
            public int h;

            [XmlAttribute]
            public int page;
        }

        [XmlAttribute]
        public int PageCount;

        [XmlAttribute]
        public int Height;

        [XmlAttribute]
        public int InternalLeading;

        [XmlAttribute]
        public double Baseline;

        [XmlElement]
        public LayerList Layers;

        [XmlElement]
        public GlyphList Glyphs;
    }
}
