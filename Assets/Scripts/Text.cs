/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/////////////////////////////////////////////////////////////////////////////

namespace Font
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class Text : MonoBehaviour
	{
        //////////////////////////////////////////////////////////////////////

        private TypeFace typeface;
		public Glyph[] letters;
        private string text;

        //////////////////////////////////////////////////////////////////////

        public static Text Create(TypeFace face, string txt)
        {
            Text t = Util.Create<Text>();
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
                letters = new Glyph[text.Length];
                float x = 0;
                for (int i = 0, l = text.Length; i < l; ++i)
                {
                    Glyph g = Font.Glyph.Create(typeface, text[i]);
                    g.transform.localPosition = new Vector2(x, 0);
                    g.transform.parent = transform;
                    letters[i] = g;
                    x += g.advance;
                }
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
