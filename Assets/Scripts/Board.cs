//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////

public class Board
{
    //////////////////////////////////////////////////////////////////////

    public int score;

    //////////////////////////////////////////////////////////////////////

    private GameObject root;
	private Piece[] pieces;
	private List<Word> foundWords = new List<Word>();
	private List<Word> validWords = new List<Word>();

    //////////////////////////////////////////////////////////////////////

    public Board()
	{
        root = new GameObject("Board");
		pieces = new Piece[Main.boardWidth * Main.boardHeight];
		Letters.Seed(2);
		int i = 0;
		for(int y = 0; y < Main.boardHeight; ++y)
        {
			for(int x = 0; x < Main.boardWidth; ++x)
            {
                Piece p = new Piece();
                p.Sprite = Tiles.Get(0, 4);
                p.Letter = Letters.GetRandomLetter();
                p.Position = new Vector2(x * p.Width, y * p.Height);
                p.transform.parent = root.transform;
				pieces[i++] = p;
			}
		}
        root.transform.position = new Vector3(48, 48, 0);
        MarkAllWords();
	}

    //////////////////////////////////////////////////////////////////////

    private Piece GetWordPiece(Word w, int index)
	{
		int yo = (int)w.orientation;
		int xo = 1 - yo;
		return pieces[(w.x + xo * index) + (w.y + yo * index) * Main.boardWidth];
	}

    //////////////////////////////////////////////////////////////////////

    private void MarkWordPass(int orientation, int offsetVector, int limit, int xMul, int yMul)
	{
        int xLim = Main.boardWidth - 2 * xMul;
        int yLim = Main.boardHeight - 2 * yMul;
		
		for (int y = 0; y < yLim; ++y)
		{
			for (int x = 0; x < xLim; ++x)
			{
                int n = x + y * Main.boardWidth;
				int t = x * xMul + y * yMul;

				for (int e = 3; e + t <= limit; ++e)
				{
					string checkString = "";
					int m = n;
					int i = 0;
					for (; i < e; ++i)
					{
						checkString += pieces[m].Letter;
						m += offsetVector;
					}
					if(Dictionary.IsWord(checkString))
					{
						Word word = new Word(x, y, orientation, Dictionary.GetWord(checkString));
						if(word != null)
						{
							foundWords.Add(word);
						}
					}
				}
			}
		}
	}

    //////////////////////////////////////////////////////////////////////

    public void MarkAllWords()
	{
        for (int i = 0, l = pieces.Length; i < l; ++i)
        {
            pieces[i].ResetWords();
		}

		foundWords.Clear();
		validWords.Clear();

        MarkWordPass(Word.horizontal, 1, Main.boardWidth, 1, 0);
        MarkWordPass(Word.vertical, Main.boardWidth, Main.boardHeight, 0, 1);

		foundWords.Sort();
		
		score = 0;
        for (int i = 0, l = foundWords.Count; i < l; ++i)
        {
            Word w = foundWords[i];
            bool isValid;
            isValid = true;
            for (int t = 0, tl = w.word.text.Length; t < tl; ++t)
            {
                Piece p = GetWordPiece(w, t);
                if (p.IsPartOf(Word.vertical) && w.orientation == Word.vertical ||
                    p.IsPartOf(Word.horizontal) && w.orientation == Word.horizontal)
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                validWords.Add(w);
                score += w.score;
                for (int t = 0, tl = w.word.text.Length; t < tl; ++t)
                {
                    GetWordPiece(w, t).SetWord(w, t);
                }
            }
        }
        for (int i = 0, l = pieces.Length; i < l; ++i)
		{
			pieces[i].SetupTile();
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

    //////////////////////////////////////////////////////////////////////

    public void Update()
    {
		root.transform.Translate(new Vector3(1, 0, 0));
	}
}
