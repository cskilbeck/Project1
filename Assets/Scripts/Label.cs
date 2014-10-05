/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/////////////////////////////////////////////////////////////////////////////

namespace UI
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class Label : MonoBehaviour
	{
        //////////////////////////////////////////////////////////////////////

        private TypeFace typeface;
		public Glyph[] letters;
        string text;
        float alpha = 1.0f;

        //////////////////////////////////////////////////////////////////////

        public static Label Create(TypeFace face, string txt)
        {
            Label t = Util.Create<Label>();
            t.TypeFace = face;
            t.String = txt;
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
                        Destroy(letters[i].gameObject);
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
                    x += g.advance;
                }
                Alpha = alpha;  // hmph
            }
        }

        //////////////////////////////////////////////////////////////////////

        public string String
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
                                Color c = r.material.color;
                                c.a = value;
                                r.material.color = c;
                            }
                        }
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////

        public TypeFace TypeFace
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
