//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using Font;

//////////////////////////////////////////////////////////////////////

class Piece : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////

    private Sprite tile;
    private SpriteRenderer tileRenderer;
    private BoxCollider2D boxCollider;
    private Glyph glyph;
    private float angle;
    private Vector2 position;
    private char letter;
    private Vector2 dragOffset;

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

    public void Awake()
    {
        tileRenderer = gameObject.AddComponent<SpriteRenderer>();
        tileRenderer.transform.localScale = new Vector3(1, -1, 1);
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        wordDetails[Word.horizontal] = new WordDetails();
        wordDetails[Word.vertical] = new WordDetails();
    }

    //////////////////////////////////////////////////////////////////////

    public void SetSortingLayer(string name)
    {
        SetSortingLayerName(transform, name);
    }

    //////////////////////////////////////////////////////////////////////

    private static void SetSortingLayerName(Transform transform, string name)
    {
        SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = name;
        }
        foreach (Transform t in transform)
        {
            SetSortingLayerName(t, name);
        }
    }

    //////////////////////////////////////////////////////////////////////

    public void OnMouseDown()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        dragOffset = mousePos - Position;
        transform.localScale = new Vector2(1.2f, -1.2f);
        SetSortingLayer("DragPiece");
    }

    //////////////////////////////////////////////////////////////////////

    public void OnMouseUp()
    {
        transform.localScale = new Vector2(1, -1);
        SetSortingLayer("Board");
    }

    //////////////////////////////////////////////////////////////////////

    public void OnMouseDrag()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        Position = mousePos - dragOffset;
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

    ////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// IsPartOf. is this piece a part of any word of a certain orientation?
    /// </summary>

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
            boxCollider.center = tile.bounds.center;
            boxCollider.size = tile.bounds.size;
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
            transform.localPosition = value;
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
            letter = Char.ToLower(value);
            char u = Char.ToUpper(value);
            glyph = Font.Glyph.Create(typeFace, u);
            glyph.transform.localPosition = -glyph.bounds.center;
            glyph.transform.parent = transform;
            name = "Piece:" + u;
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
}
