//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UI;

//////////////////////////////////////////////////////////////////////

public class Tile : MonoBehaviour
{
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

    public enum State : int
    {
        Idle = 0,
        Moving = 1
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

    //////////////////////////////////////////////////////////////////////

    public WordDetails[] wordDetails = new WordDetails[2];
    public bool swapped;
    public Vector2 org;
    public Vector2 target;
    public Vector2 source;
    public float moveStartTime; // when the lerp started
    public float moveEndTime;      // how long the lerp should take
    public State state;
    public Point boardPosition;

    public delegate void OnMoveCompleteDelegate(Tile t);
    public event OnMoveCompleteDelegate OnMoveComplete;

    //////////////////////////////////////////////////////////////////////

    Board board;
    bool selected;
    Sprite tile;                // when it's not selected
    Sprite currentTile;         // whatever is currently being shown
    SpriteRenderer tileRenderer;
    BoxCollider2D boxCollider;
    Glyph glyph;
    float angle;
    Vector2 position;
    char letter;

    //////////////////////////////////////////////////////////////////////

    static TypeFace typeFace;

    //////////////////////////////////////////////////////////////////////

    public static void SetTypeFace(TypeFace face)
    {
        typeFace = face;
    }

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

    public Board Board
    {
        get
        {
            return board;
        }
        set
        {
            board = value;
        }
    }

    //////////////////////////////////////////////////////////////////////

    void Update()
    {
        switch(state)
        {
            case State.Idle:
                break;
            case State.Moving:
                if (moveEndTime < Time.realtimeSinceStartup)
                {
                    float delta = Util.Ease((Time.realtimeSinceStartup - moveStartTime) / (moveEndTime - moveStartTime));
                    position = Util.Lerp(source, target, delta);
                }
                else
                {
                    position = target;
                    state = State.Idle;
                    if (OnMoveComplete != null)
                    {
                        OnMoveComplete(this);   // tell whoever cares that we got there...
                    }
                }
                break;
        }
        if (selected)
        {
            transform.localRotation = Quaternion.AngleAxis(Mathf.Sin(Time.realtimeSinceStartup * 32) * 8, Vector3.forward);
        }
    }

    //////////////////////////////////////////////////////////////////////

    public void SetTarget(Vector2 target, float time)
    {
        state = State.Moving;
        moveStartTime = Time.realtimeSinceStartup;
        moveEndTime = moveStartTime + time;
        source = org;
        this.target = target;
   }

    //////////////////////////////////////////////////////////////////////

    private struct Layer
    {
        public string sortingLayerName;
        public float zValue;
    }

    private static Layer[] layers = new Layer[]
    {
        new Layer() { sortingLayerName = "Board", zValue = 0.0f },
        new Layer() { sortingLayerName = "DragPiece", zValue = -0.1f }
    };

    public enum SortingLayer : int
    {
        Board = 0,
        Dragging = 1
    }

    public void SetSortingLayer(SortingLayer layer)
    {
        SetSortingLayerName(transform, layers[(int)layer].sortingLayerName);
        Vector3 pos = transform.localPosition;
        pos.z = layers[(int)layer].zValue;
        transform.localPosition = pos;
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

    public bool Selected
    {

        get 
        {
            return selected;
        }
        set
        {
            selected = value;
            float scale = selected ? 1.25f : 1.0f;
            transform.localScale = new Vector2(scale, -scale);
            SetSortingLayer(selected ? SortingLayer.Dragging : SortingLayer.Board);
            Sprite = selected ? Tiles.Get(4, 4) : tile;
            if (!selected)
            {
                transform.localRotation = Quaternion.identity;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////

    public void OnMouseDown()
    {
        if (board != null)
        {
            //Vector2 mousePos = Input.mousePosition;
            //mousePos.y = Screen.height - mousePos.y;
            //dragOffset = mousePos - Position;
            if (Selected)
            {
                Main.board.activeTile = null;
                Selected = false;
            }
            else
            {
                if (Main.board.activeTile != null)
                {
                    Main.board.activeTile.Selected = false;
                    SwapLetters(Main.board.activeTile);
                    SetSortingLayer(SortingLayer.Board);
                    Main.board.activeTile.SetSortingLayer(SortingLayer.Board);
                    Main.board.MarkAllWords();
                    Main.board.activeTile = null;
                }
                else
                {
                    Selected = true;
                    Main.board.activeTile = this;
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////

    public void OnMouseUp()
    {
    }

    //////////////////////////////////////////////////////////////////////

    public void OnMouseDrag()
    {
        //Vector2 mousePos = Input.mousePosition;
        //mousePos.y = Screen.height - mousePos.y;
        //Position = mousePos - dragOffset;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    public void ResetWords()
    {
        wordDetails[Word.horizontal].position = WordPosition.None;
        wordDetails[Word.vertical].position = WordPosition.None;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    public void SetWord(Word w, int index)
    {
        wordDetails[w.orientation].Set(w, index);
    }

    ////////////////////////////////////////////////////////////////////////////////////

    public bool IsPartOf(int orientation)
    {
        return wordDetails[orientation].position != WordPosition.None;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    public void SetupTile()
    {
        int u = (int)wordDetails[Word.horizontal].position;
        int v = (int)wordDetails[Word.vertical].position;
        if (u == 0 && v == 0)
        {
            v = 4;
        }
        tile = Tiles.Get(u, v);
        Sprite = tile;
    }

    //////////////////////////////////////////////////////////////////////

    public Sprite Sprite
    {
        get
        {
            return currentTile;
        }
        set
        {
            currentTile = value;
            tileRenderer.sprite = currentTile;
            boxCollider.center = currentTile.bounds.center;
            boxCollider.size = currentTile.bounds.size;
        }
    }

    ///////////////////////////////////////////////////////////////////

    private void SetGlyph(Glyph g)
    {
        g.transform.SetParent(transform);
        glyph = g;
    }

    private void SwapLetters(Tile other)
    {
        char otherLetter = other.letter;
        Glyph otherGlyph = other.glyph;
        other.SetGlyph(glyph);
        SetGlyph(otherGlyph);
        other.letter = letter;
        letter = otherLetter;
        other.name = "Piece! >>" + Char.ToUpper(other.letter);
        name = "Piece: >>" + Char.ToUpper(letter);
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
            glyph = UI.Glyph.Create(typeFace, u);
            glyph.transform.localPosition = -glyph.bounds.center;
            glyph.transform.parent = transform;
            name = "Piece:" + u;
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
            return currentTile.textureRect.width;
        }
    }

    ///////////////////////////////////////////////////////////////////

    public float Height
    {
        get
        {
            return currentTile.textureRect.height;
        }
    }
}
