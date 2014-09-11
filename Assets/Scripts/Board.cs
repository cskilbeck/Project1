using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MTW;

public class Board : MonoBehaviour {

	private GameObject tilePrefab;
	private Piece[] pieces;
	private Main main;

	[HideInInspector]
	public int width;
	[HideInInspector]
	public int height;
	[HideInInspector]
	public int score;

	private List<Word> foundWords = new List<Word>();
	private List<Word> validWords = new List<Word>();

	// Use this for initialization
	public void Setup()
	{
		main = (Main)(GameObject.Find("Main").GetComponent<Main>());
		width = main.boardWidth;
		height = main.boardHeight;
		pieces = new Piece[main.boardWidth * main.boardHeight];
		tilePrefab = (GameObject)Resources.Load("Piece");
		MTW.Letters.Seed(2);
		int i = 0;
		for(int y = 0; y < main.boardHeight; ++y) {
			for(int x = 0; x < main.boardWidth; ++x) {
				Piece p = ((GameObject)Instantiate(tilePrefab, new Vector3(x * 96, y * 96, 0), Quaternion.identity)).GetComponent<Piece>();
				p.Setup(transform);
				p.SetSprite(Main.tileFrames[(int)(Random.value * Main.tileFrames.Length)]);
				//p.SetSprite(main.tileFrames[0]);
				p.Letter = MTW.Letters.GetRandomLetter();
				pieces[i++] = p;
			}
		}
//		Debug.Log(MTW.Dictionary.GetDefinition("sleep"));
	}

	private Piece GetWordPiece(Word w, int index)
	{
		int yo = (int)w.orientation;
		int xo = 1 - yo;
		return pieces[(w.x + xo * index) + (w.y + yo * index) * width];
	}
	
	private void MarkWordPass(MTW.Word.Orientation orientation, int offsetVector, int limit, int xMul, int yMul)
	{
		int xLim = width - 2 * xMul;
		int yLim = height - 2 * yMul;
		
		for (int y = 0; y < yLim; ++y)
		{
			for (int x = 0; x < xLim; ++x)
			{
				int n = x + y * width;
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
					if(MTW.Dictionary.IsWord(checkString))
					{
						Word word = new Word(x, y, orientation, MTW.Dictionary.GetWord(checkString));
						if(word != null)
						{
							foundWords.Add(word);
						}
					}
				}
			}
		}
	}

	public void MarkAllWords()
	{
		foreach(Piece p in pieces)
		{
			p.ResetWords();
		}

		foundWords.Clear();
		validWords.Clear();

		MarkWordPass(Word.Orientation.horizontal, 1, width, 1, 0);
		MarkWordPass(Word.Orientation.vertical, width, height, 0, 1);

		foundWords.Sort();

//		foreach(MTW.Word w in foundWords)
//		{
//			Debug.Log("Found: " + w.word.text + " at " + w.x.ToString() + "," + w.y.ToString());
//		}
		
		score = 0;
		foreach(MTW.Word w in foundWords)
		{
			bool isValid;
			isValid = true;
			string text = w.word.text;
			for(int i = 0; i < text.Length; ++i)
			{
				Piece p = GetWordPiece(w, i);
				if (p.verticalWordDetails.position != Piece.WordPosition.None && w.orientation == Word.Orientation.vertical ||
				    p.horizontalWordDetails.position != Piece.WordPosition.None && w.orientation == Word.Orientation.horizontal)
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
	
	// Update is called once per frame
	void Update() {
		//this.transform.Translate(new Vector3(1, 0, 0));
		//this.transform.RotateAround(new Vector3(3, 2, 0), new Vector3(0, 0, 1), 1);
	}
}
