//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

//////////////////////////////////////////////////////////////////////

class Tiles
{
    //////////////////////////////////////////////////////////////////////

    public const int tileWidth = 96;
    public const int tileHeight = 96;

    //////////////////////////////////////////////////////////////////////

    private static Sprite[] tileFrames = new Sprite[25];

    private const int tilesWide = 5;
    private const int tilesHigh = 5;

    //////////////////////////////////////////////////////////////////////

    public static void Create()
    {
        Texture2D t = (Texture2D)Resources.Load("allColour");
        t.filterMode = FilterMode.Trilinear;
        int i = 0;
        for (int y = 0; y < tilesHigh; ++y)
        {
            for (int x = 0; x < tilesWide; ++x)
            {
                Sprite s = Sprite.Create(t, new Rect(x * tileWidth, t.height - (y * tileHeight) - tileHeight, tileWidth, tileHeight), new Vector2(0.5f, 0.5f), 1);
                tileFrames[i++] = s;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////

    public static Sprite Get(int u, int v)
    {
        return tileFrames[u + v * tilesWide];
    }
}
