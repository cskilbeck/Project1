//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

//////////////////////////////////////////////////////////////////////

class Tiles
{
    //////////////////////////////////////////////////////////////////////

    private static Sprite[] tileFrames = new Sprite[25];

    //////////////////////////////////////////////////////////////////////

    public static void Create()
    {
        Texture2D t = (Texture2D)Resources.Load("allColour");
        t.filterMode = FilterMode.Trilinear;
        int i = 0;
        for (int y = 0; y < 5; ++y)
        {
            for (int x = 0; x < 5; ++x)
            {
                Sprite s = Sprite.Create(t, new Rect(x * 96, t.height - (y * 96) - 96, 96, 96), new Vector2(0.5f, 0.5f), 1);
                tileFrames[i++] = s;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////

    public static Sprite Get(int u, int v)
    {
        return tileFrames[u + v * 5];
    }

}
