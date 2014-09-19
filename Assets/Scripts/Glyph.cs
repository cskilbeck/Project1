/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////

namespace Font
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class Glyph
	{
		public GameObject[] letter;
		public float advance;

		public Glyph(TypeFace font, char c)
		{
			TypeFace.GlyphDescriptor g = font.glyphs[c];

			letter = new GameObject[g.imageCount];
			advance = g.advance;

			int ic = g.imageCount;

			for (int i=0; i<ic; ++i)
			{
				letter[i] = new GameObject();
				letter[i].AddComponent<SpriteRenderer>();
				letter[i].GetComponent<SpriteRenderer>().sprite = g.images[i];
				if(i != 0)
				{
					letter[i].transform.parent = letter[0].transform;
				}
				letter[i].transform.localPosition = g.offsets[i];
				letter[i].GetComponent<SpriteRenderer>().sortingOrder = (ic - i) + 20;
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
