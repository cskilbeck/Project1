//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using Font;

//////////////////////////////////////////////////////////////////////

class Piece
{
    //////////////////////////////////////////////////////////////////////

    private GameObject goTile;
    private Sprite tile;
    private SpriteRenderer tileRenderer;
    private Glyph glyph;
    private float angle = 0;
    private Vector2 position;
    private char letter;

    //////////////////////////////////////////////////////////////////////

    private static TypeFace typeFace;

    //////////////////////////////////////////////////////////////////////

    public static void SetTypeFace(TypeFace face)
    {
        typeFace = face;
    }

    ////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// WordPosition. Where in a word is this tile. These numbers are used to index into the atlas
    /// </summary>

    public enum WordPosition : int
    {
        None = 0,
        Beginning = 1,
        Middle = 2,
        End = 3
    }

    ////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// WordDetails. Details about a word this Piece might be a part of
    /// </summary>

    public class WordDetails
    {
        public Word word;
        public int index;
        public WordPosition position;

        public WordDetails()
        {
            word = null;
            index = 0;
            position = WordPosition.None;
        }

        public void Set(Word wrd, int idx)
        {
            word = wrd;
            index = idx;
            position = (idx == 0) ? WordPosition.Beginning : (idx == wrd.length - 1) ? WordPosition.End : WordPosition.Middle;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// WordDetails for horizontal and vertical word membership
    /// </summary>

    public WordDetails[] wordDetails = new WordDetails[2];

    //////////////////////////////////////////////////////////////////////

    public Piece()
    {
        goTile = new GameObject("Piece");
        goTile.AddComponent<SpriteRenderer>();
        tileRenderer = goTile.GetComponent<SpriteRenderer>();
        tileRenderer.transform.localScale = new Vector3(1, -1, 1);
        wordDetails[Word.horizontal] = new WordDetails();
        wordDetails[Word.vertical] = new WordDetails();
    }

    ////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ResetWords. Clear out horiz, vert word details
    /// </summary>

    public void ResetWords()
    {
        wordDetails[Word.horizontal].position = WordPosition.None;
        wordDetails[Word.vertical].position = WordPosition.None;
    }

    ////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// SetWord. Assign this tile to a word
    /// </summary>

    public void SetWord(Word w, int index)
    {
        wordDetails[w.orientation].Set(w, index);
    }

    public bool IsPartOf(int orientation)
    {
        return wordDetails[orientation].position != WordPosition.None;
    }

    ////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// SetTile. Change the tile background based on the word membership
    /// </summary>

    public void SetupTile()
    {
        int u = (int)wordDetails[Word.horizontal].position;
        int v = (int)wordDetails[Word.vertical].position;
        if (u == 0 && v == 0)
        {
            v = 4;
        }
        this.Sprite = Tiles.Get(u, v);
    }

    //////////////////////////////////////////////////////////////////////

    public Sprite Sprite
    {
        get
        {
            return tile;
        }
        set
        {
            tile = value;
            tileRenderer.sprite = tile;
        }
    }

    ///////////////////////////////////////////////////////////////////

    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            transform.position = value;
        }
    }

    ///////////////////////////////////////////////////////////////////

    public char Letter
    {
        get
        {
            return letter;
        }
        set
        {
            letter = value;
            glyph = new Glyph(typeFace, letter);
            Vector3 d = glyph.letter[0].renderer.bounds.center;
            glyph.transform.position = new Vector3(-d.x, -d.y) + goTile.transform.position;
            glyph.transform.parent = goTile.transform;
            goTile.name = "Piece(" + Char.ToUpper(value).ToString() + ")";
        }
    }

    ///////////////////////////////////////////////////////////////////

    public float Rotation
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    ///////////////////////////////////////////////////////////////////

    public float Width
    {
        get
        {
            return tile.textureRect.width;
        }
    }

    ///////////////////////////////////////////////////////////////////

    public float Height
    {
        get
        {
            return tile.textureRect.height;
        }
    }

    ///////////////////////////////////////////////////////////////////

    public Transform transform
    {
        get
        {
            return goTile.transform;
        }
    }
}
