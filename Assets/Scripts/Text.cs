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
            Setup();
		}

        //////////////////////////////////////////////////////////////////////

        private void Setup()
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

        public string String
        {
            get
            {
                return text;
            }
            set
            {
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
