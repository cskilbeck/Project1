﻿//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////

public class Board : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////

    public int score;
    public Tile activeTile;

    //////////////////////////////////////////////////////////////////////

	private Tile[] pieces;
	private List<Word> foundWords = new List<Word>();
    private List<Word> validWords = new List<Word>();

    private static StringBuilder checkString = new StringBuilder(Main.boardWidth, Main.boardWidth);

    //////////////////////////////////////////////////////////////////////

    public void Start()
	{
        pieces = new Tile[Main.boardWidth * Main.boardHeight];
		Letters.Seed(3);
		int i = 0;
		for(int y = 0; y < Main.boardHeight; ++y)
        {
			for(int x = 0; x < Main.boardWidth; ++x)
            {
                Tile p = Util.Create<Tile>();
                p.Board = this;
                p.boardPosition = new Point(x, y);
                p.Sprite = Tiles.Get(0, 4);
                p.Letter = Letters.GetRandomLetter();
                p.Position = new Vector2(x * p.Width, y * p.Height);
                p.transform.parent = transform;
				pieces[i++] = p;
			}
		}
        transform.position = new Vector3(Tiles.tileWidth / 2, Tiles.tileHeight / 2, 0);
        MarkAllWords();
    }

    //////////////////////////////////////////////////////////////////////

    private Tile GetWordPiece(Word w, int index)
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
                    checkString.Length = 0;
					int m = n;
					int i = 0;
					for (; i < e; ++i)
					{
						checkString.Append(pieces[m].Letter);
						m += offsetVector;
					}
                    string checkWord = checkString.ToString();
                    if (Dictionary.IsWord(checkWord))
					{
						foundWords.Add(new Word(x, y, orientation, Dictionary.GetWord(checkWord)));
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

        // unstable sort needs an index for comparison to make it stable
        for (int i = 0, l = foundWords.Count; i < l; ++i)
        {
            foundWords[i].sortIndex = i;
        }

		foundWords.Sort();
		
		score = 0;
        for (int i = 0, l = foundWords.Count; i < l; ++i)
        {
            Word w = foundWords[i];
            bool invalid = false;
            for (int t = 0, tl = w.word.text.Length; t < tl; ++t)
            {
                if (GetWordPiece(w, t).IsPartOf(w.orientation))
                {
                    invalid = true;
                    break;
                }
            }
            if (!invalid)
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

    public void Update()
    {
		//root.transform.Translate(new Vector3(1, 0, 0));
	}
}
