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

        private static Material glyphMaterial;

        //////////////////////////////////////////////////////////////////////

        private static Material Material
        {
            get
            {
                return glyphMaterial != null ? glyphMaterial : CreateGlyphMaterial();
            }
        }

        private static Material CreateGlyphMaterial()
        {
            glyphMaterial = new Material(Shader.Find("Sprites/Default"));
            glyphMaterial.name = "GlyphMaterial";
            return glyphMaterial;
        }

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
                float nearZ = Camera.main.nearClipPlane;
                var zero = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, nearZ));
                var one = Camera.main.ScreenToWorldPoint(new Vector3(1, 1, nearZ));  // work out how big a pixel is...
                var pixel = one - zero;
                name = c.ToString();
                advance = g.advance;
                int imageCount = g.imageCount;
                if (imageCount > 0)
                {
                    letter = new GameObject[imageCount];
                    for (int i = 0; i < imageCount; ++i)
                    {
                        GameObject go = new GameObject("Layer:" + i.ToString());
                        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                        Sprite sprite = g.images[i];
                        if (sprite) //huh?
                        {
                            sr.sprite = sprite;
                            sr.material = Glyph.Material;
                            float pixelsPerUnit = sprite.rect.width / sprite.bounds.size.x;
                            go.transform.localPosition = new Vector3((-0.25f + g.offsets[i].x) * pixel.x, (0.25f + font.data.Height - g.offsets[i].y) * pixel.y, nearZ);
                            sr.sortingOrder = i;
                            go.transform.SetParent(transform);
                            letter[i] = go;
                        }
                    }
                }
            }
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
                    Init(font, value);
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
