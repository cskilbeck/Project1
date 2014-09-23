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
			int i = 0;
			float x = 0;
			foreach(char c in s)
			{
                letters[i] = Glyph.Create(font, c);
				letters[i].transform.Translate(new Vector2(x, 0));
				x += letters[i].advance;
				letters[i].transform.parent = root.transform;
			}
		}
	}
}
