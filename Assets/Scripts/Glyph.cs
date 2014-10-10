/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////

namespace UI
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class Glyph : MonoBehaviour
	{
        public char character;
        public BitmapFont font;
		public GameObject[] letter;
		public float advance;

        //////////////////////////////////////////////////////////////////////

        public static Glyph Create(BitmapFont font, char c)
        {
            Glyph g = Util.Create<Glyph>();
            g.Init(font, c);
            return g;
        }

        //////////////////////////////////////////////////////////////////////

        private void Init(BitmapFont font, char c)
        {
            character = c;
            this.font = font;
            BitmapFont.GlyphRenderer g = font.GetGlyphDetails(c);
            if(g != null)
            {
                name = "Glyph " + c;
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
                        Vector2 off = new Vector2(g.offsets[i].x, g.offsets[i].y);
                        l.transform.localPosition = off;
                        l.transform.SetParent(transform);
                        sr.sortingOrder = i + 20;
                        letter[i] = l;
                    }
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
	}
}
