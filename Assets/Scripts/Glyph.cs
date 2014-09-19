/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////

namespace FontUtil
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class Glyph
	{
		public GameObject[] letter;

		public Glyph(Font font, char c)
		{
			Font.GlyphDescriptor g = font.glyphs['G'];

			letter = new GameObject[g.imageCount];

			for (int i=0; i<g.imageCount; ++i)
			{
				letter[i] = new GameObject();
				letter[i].AddComponent<SpriteRenderer>();
				letter[i].GetComponent<SpriteRenderer>().sprite = g.images[i];
				if(i != 0)
				{
					letter[i].transform.parent = letter[0].transform;
				}
				letter[i].transform.localPosition = g.offsets[i];
			}
			letter[0].transform.localScale = new Vector2 (1, -1);
		}

		public Transform transform
		{
			get
			{
				return letter[0].transform;
			}
		}
	}

}
