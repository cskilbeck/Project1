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

        public Camera Camera2D
        {
            get
            {
                return glyphCamera;
            }
            set
            {
                glyphCamera = value;
            }
        }

        //////////////////////////////////////////////////////////////////////

        private static Camera glyphCamera = null;

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
                        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(g.offsets[i].x, -g.offsets[i].y, 0));
                        l.transform.localPosition = v;
                        l.transform.SetParent(transform);
                        sr.sortingOrder = i + 20;
                        letter[i] = l;
                    }
                }
            }
        }

        private void Setup()
        {

        }

        public char Character
        {
            get
            {
                return character;
            }
            set
            {
                if (character != value)
                {
                    character = value;
                    Setup();
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
