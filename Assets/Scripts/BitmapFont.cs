using UnityEditor;
using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor(typeof(BitmapFont))]
public class BitmapFontEditor : Editor
{
    [MenuItem("Assets/Create/BitmapFont")]
    static public void CreateBitmapFont()
    {
        BitmapFont f = ScriptableObject.CreateInstance<BitmapFont>();
        var path = EditorUtility.OpenFilePanel("Select .bitmapfont file", "", "bitmapfont");
        if (path.Length != 0)
        {
            string s = File.ReadAllText(path);
            if (s != null && s.Length != 0)
            {
                using (StringReader reader = new StringReader(s))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(BitmapFont.Data));
                    f.sourceFilename = path;
                    try
                    {
                        f.data = serializer.Deserialize(reader) as BitmapFont.Data;
                        f.name = Path.GetFileNameWithoutExtension(path);
                        f.Setup();
                    }
                    catch (InvalidOperationException)
                    {
                        f.data = null;
                        f.name = "Error";
                    }
                }
            }
            AssetDatabase.CreateAsset(f, "Assets/" + f.name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

    Vector2 scrollPosition = Vector2.zero;
    int currentPage = 0;

    public override void OnInspectorGUI()
    {
        BitmapFont font = target as BitmapFont;
        int pageCount = font.pages.Length;
        string[] pageNames = new string[pageCount];
        int[] pageValues = new int[pageCount];

        for (int i = 0; i < pageCount; ++i)
        {
            pageNames[i] = i.ToString();
            pageValues[i] = i;
        }

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(font.name + ", Pages: " + pageCount.ToString());
        EditorGUILayout.IntField("Height:", font.data.Height);
        EditorGUILayout.IntField("Glyphs:", font.data.Glyphs.Count);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Page: ");
        currentPage = EditorGUILayout.IntPopup(currentPage, pageNames, pageValues);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        Texture2D page = font.pages[currentPage];
        if (page != null)
        {
            float scale = Mathf.Min(1.0f, 256.0f / Mathf.Min(page.width, page.height));
            Rect topLeft = EditorGUILayout.BeginVertical();
            Rect inner = new Rect(0, 0, page.width * scale, page.height * scale);
            topLeft.size = new Vector2(256, 256);
            scrollPosition = GUI.BeginScrollView(topLeft, scrollPosition, inner);
            EditorGUI.DrawTextureTransparent(inner, page);
            GUI.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}

[Serializable]
public class BitmapFont : ScriptableObject
{
    [Serializable]
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

    [Serializable]
    public class GlyphList
    {
        [XmlAttribute]
        public int Count;

        [XmlElement]
        public Glyph[] Glyph;
    }

    [Serializable]
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

    [Serializable]
    public class LayerList
    {
        [XmlAttribute]
        public int Count;

        [XmlElement]
        public Layer[] Layer;
    }

    [Serializable]
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

    [Serializable]
    [XmlRoot("BitmapFont")]
    public class Data
    {
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

    [SerializeField]
    public string sourceFilename;

    [SerializeField]
    public Data data;

    [SerializeField]
    public Texture2D[] pages;

    public class GlyphRenderer
    {
		public char c;
		public int imageCount;
		public float advance;
		public Sprite[] images;
		public Vector2[] offsets;

        public GlyphRenderer(int imgCount, float advnce, Sprite[] imgs, Vector2[] offs)
		{
			imageCount = imgCount;
			advance = advnce;
			images = imgs;
			offsets = offs;
		}
    }

    public GlyphRenderer GetGlyphDetails(char c)
    {
        return glyph.ContainsKey(c) ? glyph[c] : null;
    }

    // rebuild these in OnEnable
    private Dictionary<char, GlyphRenderer> glyph;

    void OnEnable()
    {
        CreateGlyphSprites();
    }

    private void LoadTextures()
    {
        pages = new Texture2D[data.PageCount];
        for (int i = 0; i < data.PageCount; ++i)
        {
            string dirName = Path.GetDirectoryName(sourceFilename);
            string fileName = Path.GetFileNameWithoutExtension(sourceFilename);
            string texturePageName = Path.Combine(dirName, fileName) + i.ToString() + ".png";
            Texture2D page = new Texture2D(4, 4, TextureFormat.ARGB32, false);
            byte[] file = File.ReadAllBytes(texturePageName);
            if (file != null && file.Length != 0 && page.LoadImage(file))
            {
                pages[i] = page;
                page.filterMode = FilterMode.Point;
                string path = Util.CreateAssetFolder("Assets", "FontPages");
                if (path.Length > 0)
                {
                    path = Util.CreateAssetFolder(path, fileName);
                    if (path.Length > 0)
                    {
                        string assetPath = path + "/" + i.ToString();
                        AssetDatabase.CreateAsset(page, assetPath);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }
    }

    private void CreateGlyphSprites()
    {
        if (data != null)
        {
            Vector2 pivot = new Vector2(0, 1);
            glyph = new Dictionary<char, GlyphRenderer>();
            foreach (Glyph g in data.Glyphs.Glyph)
            {
                Sprite[] sprites = new Sprite[g.images];
                Vector2[] offs = new Vector2[g.images];
                bool valid = true;
                for (int i = 0; i < g.images; ++i)
                {
                    Graphic gr = g.Graphic[i];
                    Texture2D page = pages[gr.page];
                    if (page != null)
                    {
                        float th = (float)page.height;
                        Rect r = new Rect(gr.x, th - gr.y - gr.h, gr.w, gr.h);
                        offs[i] = new Vector2(gr.offsetX, gr.offsetY);
                        sprites[i] = Sprite.Create(pages[gr.page], r, pivot);
                    }
                    else
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    GlyphRenderer glr = new GlyphRenderer(g.images, g.advance, sprites, offs);
                    glyph.Add((char)g.charcode, glr);
                }
            }
        }
    }

    public void Setup()
    {
        LoadTextures();
        CreateGlyphSprites();
    }

    public BitmapFont()
    {
    }
}
