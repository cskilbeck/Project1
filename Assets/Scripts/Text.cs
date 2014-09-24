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
		public Glyph[] letters;

        //////////////////////////////////////////////////////////////////////

        public Text(TypeFace font, string s)
		{
            string name = s;
            int sl = s.Length;
            if(sl > 5)
            {
                name = name.Substring(0, 5);
            }
			root = new GameObject("Text(" + name + ")");
			letters = new Glyph[s.Length];
			float x = 0;
            for(int i = 0, l = s.Length; i < l; ++i)
			{
                Glyph g = new Glyph(font, s[i]);
                g.transform.localPosition = new Vector2(x, 0);
                g.transform.parent = root.transform;
                letters[i] = g;
                x += g.advance;
			}
		}

        public GameObject GameObject
        {
            get
            {
                return root;
            }
        }

        public Transform transform
        {
            get
            {
                return root.transform;
            }
        }
	}
}
