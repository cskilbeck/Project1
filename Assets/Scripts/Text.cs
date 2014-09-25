/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/////////////////////////////////////////////////////////////////////////////

namespace Font
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class Text
	{
        //////////////////////////////////////////////////////////////////////

        public GameObject root;
        private TypeFace typeface;
		public Glyph[] letters;
        private string text;

        //////////////////////////////////////////////////////////////////////

        public Text(TypeFace font, string s)
		{
            text = s;
            typeface = font;
            Init();
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
                text = value;
                Init();
            }
        }

        //////////////////////////////////////////////////////////////////////

        private void Init()
        {
            string name = text;
            int sl = text.Length;
            if (sl > 5)
            {
                name = name.Substring(0, 5);
            }
            root = new GameObject("Text(" + name + ")");
            letters = new Glyph[text.Length];
            float x = 0;
            for (int i = 0, l = text.Length; i < l; ++i)
            {
                Glyph g = new Glyph(typeface, text[i]);
                g.transform.localPosition = new Vector2(x, 0);
                g.transform.parent = root.transform;
                letters[i] = g;
                x += g.advance;
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
                Init();
            }
        }

        //////////////////////////////////////////////////////////////////////

        public GameObject GameObject
        {
            get
            {
                return root;
            }
        }

        //////////////////////////////////////////////////////////////////////

        public Transform transform
        {
            get
            {
                return root.transform;
            }
        }
	}
}
