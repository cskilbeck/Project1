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
            Selection.activeGameObject = t.gameObject;
        }

        public override void OnInspectorGUI()
        {
            Label l = target as Label;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            EditorGUILayout.LabelField("Font:");
            l.TypeFace = EditorGUILayout.ObjectField(l.typeface, typeof(BitmapFont), false) as BitmapFont;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            EditorGUILayout.LabelField("Text:");
            l.Text = EditorGUILayout.TextField(l.Text);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            EditorGUILayout.LabelField("Alpha:");
            l.alpha = EditorGUILayout.Slider(l.alpha, 0, 1);
            EditorGUILayout.EndHorizontal();
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
        public string text = "";

        private Glyph[] letters;
        private string oldText = "";
        private BitmapFont oldTypeface;

        //////////////////////////////////////////////////////////////////////

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                if (text != oldText)
                {
                    oldText = text;
                    Setup();
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
                if (typeface != oldTypeface)
                {
                    oldTypeface = typeface;
                    Setup();
                }
            }
        }

        //////////////////////////////////////////////////////////////////////

        void OnEnable()
        {
            if (text != oldText || typeface != oldTypeface)
            {
                oldTypeface = typeface;
                oldText = text;
                Setup();
            }
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
            // check if anything changed
            if (text != null && typeface != null)
            {
                // delete all the existing letters, if the user hasn't already
                if (letters != null)
                {
                    for (int i = 0; i < letters.Length; ++i)
                    {
                        if (letters[i].gameObject != null)
                        {
                            DestroyImmediate(letters[i].gameObject);
                        }
                    }
                }

                gameObject.layer = 31;

                letters = new Glyph[text.Length];
                float x = 0;
                for (int i = 0, l = text.Length; i < l; ++i)
                {
                    Glyph g = UI.Glyph.Create(typeface, text[i]);
                    g.transform.localPosition = new Vector3(x, 0, 0);
                    g.transform.SetParent(transform);
                    letters[i] = g;
                    x += g.advance;
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
	}
}
