/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;

/////////////////////////////////////////////////////////////////////////////

namespace UI
{
    /////////////////////////////////////////////////////////////////////////////

    [CustomEditor(typeof(Label))]
    public class LabelEditor : Editor
    {
        [MenuItem("GameObject/Create Other/Label")]
        static public void CreateLabel()
        {
            Label t = Util.Create<Label>();
            t.Text = "";
        }

        public override void OnInspectorGUI()
        {
            Label l = target as Label;
            EditorGUILayout.BeginVertical();
            l.TypeFace = EditorGUILayout.ObjectField(l.typeface, typeof(BitmapFont)) as BitmapFont;
            l.Text = EditorGUILayout.TextField(l.Text);
            l.alpha = EditorGUILayout.Slider(l.alpha, 0, 1);
            EditorGUILayout.EndVertical();
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    [Serializable]
	public class Label : MonoBehaviour
	{
        //////////////////////////////////////////////////////////////////////

        public BitmapFont typeface;
        public float alpha = 1.0f;
        public string text;

        private Glyph[] letters;

        //////////////////////////////////////////////////////////////////////

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                name = "Text " + value.Substring(0, Math.Min(value.Length, 5));
                text = value;
                Setup();
            }
        }

        //////////////////////////////////////////////////////////////////////

        void OnEnable()
        {
            Setup();
        }

        //////////////////////////////////////////////////////////////////////

        public static Label Create(BitmapFont face, string txt)
        {
            Label t = Util.Create<Label>();
            t.TypeFace = face;
            t.Text = txt;
            return t;
        }

        //////////////////////////////////////////////////////////////////////

        private void Setup()
        {
            if (text != null && typeface != null)
            {
                string name = text;
                int sl = text.Length;
                if (sl > 5)
                {
                    name = name.Substring(0, 5);
                }
                // need to delete all the existing letters
                if (letters != null)
                {
                    for (int i = 0; i < letters.Length; ++i)
                    {
                        DestroyImmediate(letters[i].gameObject);
                    }
                }
                letters = new Glyph[text.Length];
                float x = 0;
                for (int i = 0, l = text.Length; i < l; ++i)
                {
                    Glyph g = UI.Glyph.Create(typeface, text[i]);
                    g.transform.localPosition = new Vector2(x, 0);
                    g.transform.SetParent(transform);
                    letters[i] = g;
                    x += g.advance * 0.0075f;
                }
                Alpha = alpha;  // hmph
            }
        }

        //////////////////////////////////////////////////////////////////////

        public float Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = value;
                for (int i = 0; i < letters.Length; ++i)
                {
                    Glyph g = letters[i];
                    if (g.letter != null)
                    {
                        for (int j = 0; j < g.letter.Length; ++j)
                        {
                            SpriteRenderer r = g.letter[j].GetComponent<SpriteRenderer>();
                            if (r != null) // this should be redundant
                            {
                                Color c = r.sharedMaterial.color;
                                c.a = value;
                                r.sharedMaterial.color = c;
                            }
                        }
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////

        public BitmapFont TypeFace
        {
            get
            {
                return typeface;
            }
            set
            {
                typeface = value;
                Setup();
            }
        }
	}
}
