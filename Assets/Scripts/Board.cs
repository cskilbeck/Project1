//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////

public class Board : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////

    [HideInInspector]
    public int score;

    //////////////////////////////////////////////////////////////////////

	private Piece[] pieces;
	private List<Word> foundWords = new List<Word>();
	private List<Word> validWords = new List<Word>();

    //////////////////////////////////////////////////////////////////////

    public void Setup()
	{
		pieces = new Piece[Main.boardWidth * Main.boardHeight];
		Letters.Seed(2);
		int i = 0;
		for(int y = 0; y < Main.boardHeight; ++y)
        {
			for(int x = 0; x < Main.boardWidth; ++x)
            {
                Piece p = Piece.Create();
                p.Position = new Vector2(x * 96, y * 96);
                p.Sprite = Main.GetTileFrame(0, 4);
                p.Letter = Letters.GetRandomLetter();
                p.transform.parent = this.transform;
				pieces[i++] = p;
			}
		}
        transform.position = new Vector3(48, 48, 0);
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
		foreach(Piece p in pieces)
		{
			p.ResetWords();
		}

		foundWords.Clear();
		validWords.Clear();

        MarkWordPass(Word.horizontal, 1, Main.boardWidth, 1, 0);
        MarkWordPass(Word.vertical, Main.boardWidth, Main.boardHeight, 0, 1);

		foundWords.Sort();
		
		score = 0;
		foreach(Word w in foundWords)
		{
			bool isValid;
			isValid = true;
			string text = w.word.text;
			for(int i = 0; i < text.Length; ++i)
			{
				Piece p = GetWordPiece(w, i);
				if (p.IsPartOf(Word.vertical) && w.orientation == Word.vertical ||
				    p.IsPartOf(Word.horizontal) && w.orientation == Word.horizontal)
				{
					isValid = false;
					break;
				}
			}
			if(isValid)
			{
				validWords.Add(w);
				score += w.score;
				for(int i=0; i<text.Length; ++i)
				{
					GetWordPiece(w, i).SetWord(w, i);
				}
			}
		}
		foreach(Piece p in pieces)
		{
			p.SetupTile();
		}
	}

    //////////////////////////////////////////////////////////////////////

    void Update()
    {
		this.transform.Translate(new Vector3(1, 0, 0));
	}
}
