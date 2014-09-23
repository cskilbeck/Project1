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
			root = new GameObject("Text(" + s + ")");
			letters = new Glyph[s.Length];
			float x = 0;
            for(int i = 0, l = s.Length; i < l; ++i)
			{
                char c = s[i];
                letters[i] = new Glyph(font, c);
				letters[i].transform.position = new Vector2(x, 0);
				x += letters[i].advance;
				letters[i].transform.parent = root.transform;
			}
		}
	}
}
