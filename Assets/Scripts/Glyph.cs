/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////

namespace Font
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class Glyph : ScriptableObject
	{
		public GameObject[] letter;
		public float advance;

        //////////////////////////////////////////////////////////////////////

        public Glyph()
        {
        }

        //////////////////////////////////////////////////////////////////////

        private void Init(TypeFace font, char c)
        {
            TypeFace.GlyphDescriptor g;
            c = Char.ToUpper(c);
            try
            {
                 g = font.glyphs[c];
            }
            catch (System.Exception)
            {
                Debug.Log("Can't find " + c.ToString());
                g = font.glyphs['Z'];
            }

            letter = new GameObject[g.imageCount];
            advance = g.advance;

            int ic = g.imageCount;

            for (int i = 0; i < ic; ++i)
            {
                letter[i] = new GameObject("GlyphLayer" + i.ToString() + "(" + c.ToString() + ")");
                letter[i].AddComponent<SpriteRenderer>();
                letter[i].GetComponent<SpriteRenderer>().sprite = g.images[i];
                if (i != 0)
                {
                    letter[i].transform.parent = letter[0].transform;
                }
                letter[i].transform.localPosition = g.offsets[i];
                letter[i].GetComponent<SpriteRenderer>().sortingOrder = (ic - i) + 20;
            }
            letter[0].transform.localScale = new Vector2(1, -1);
        }

        //////////////////////////////////////////////////////////////////////

        public static Glyph Create(TypeFace font, char c)
        {
            Glyph glyph = ScriptableObject.CreateInstance<Glyph>();
            glyph.Init(font, c);
            return glyph;
		}

        //////////////////////////////////////////////////////////////////////

        public Transform transform
		{
			get
			{
				return letter[0].transform;
			}
		}
	}

}
