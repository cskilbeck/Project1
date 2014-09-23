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
                TypeFace.GlyphDescriptor d = font.glyphs[c];
                letters[i] = new Glyph(font, c);
                if (letters[i].HasImage)
                {
                    letters[i].transform.localPosition = new Vector2(x, 0);
                    letters[i].transform.parent = root.transform;
                }
				x += letters[i].advance;
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
