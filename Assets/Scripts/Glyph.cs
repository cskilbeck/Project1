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
            TypeFace.GlyphDescriptor g;
            try
            {
                 g = font.glyphs[c];
            }
            catch (System.Exception)
            {
                Debug.Log("Can't find " + c.ToString());
                g = font.glyphs['Z'];
            }

            advance = g.advance;
            int ic = g.imageCount;
            if (ic > 0)
            {
                root = new GameObject("Glyph");
                letter = new GameObject[Math.Max(1, g.imageCount)];
                for (int i = 0; i < ic; ++i)
                {
                    GameObject l = new GameObject("GlyphLayer" + i.ToString() + "(" + c.ToString() + ")");
                    letter[i] = l;
                    SpriteRenderer sr = letter[i].AddComponent<SpriteRenderer>();
                    sr.sprite = g.images[i];
                    l.transform.localPosition = g.offsets[i];
                    l.transform.parent = root.transform;
                    sr.sortingOrder = (ic - i) + 20;
                }
                root.transform.localScale = new Vector2(1, -1);
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
