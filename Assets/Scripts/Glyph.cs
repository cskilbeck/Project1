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
        public GameObject root;
		public GameObject[] letter;
		public float advance;

        //////////////////////////////////////////////////////////////////////

        public Glyph(TypeFace font, char c)
        {
            root = new GameObject("Glyph:" + c.ToString());
            TypeFace.GlyphDescriptor g;
            if (!font.glyphs.ContainsKey(c))
            {
                c = font.defaultChar;
            }
            g = font.glyphs[c];
            advance = g.advance;
            int ic = g.imageCount;
            if (ic > 0)
            {
                letter = new GameObject[g.imageCount];
                for (int i = 0; i < ic; ++i)
                {
                    GameObject l = new GameObject("Layer:" + i.ToString());
                    SpriteRenderer sr = l.AddComponent<SpriteRenderer>();
                    sr.sprite = g.images[i];
                    Vector2 off = g.offsets[i];
                    l.transform.localPosition = off;
                    l.transform.localScale = new Vector2(1, -1);
                    l.transform.parent = root.transform;
                    sr.sortingOrder = (ic - i) + 20;
                    letter[i] = l;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Only measure the 1st layer

        public Bounds bounds
        {
            get
            {
                return letter[0].renderer.bounds;
            }
        }

        //////////////////////////////////////////////////////////////////////

        public bool HasImage
        {
            get
            {
                return letter != null;
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
