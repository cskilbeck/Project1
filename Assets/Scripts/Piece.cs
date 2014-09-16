﻿////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Piece. A board piece (Tile with letter on top)
/// </summary>

using UnityEngine;
using System.Collections;
using Word = MTW.Word;

////////////////////////////////////////////////////////////////////////////////////

public class Piece : MonoBehaviour
{
	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// admin bits
	/// </summary>
	
	private Transform tileTransform;
	private Transform letterTransform;
	private Sprite tileSprite;
	[HideInInspector]
	public SpriteRenderer tileSpriteRenderer;
	private TextMesh letterTextMesh;
	private char letter;
	private float x;
	private float y;
	private float xoffset;
	private float yoffset;
	
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
			position = (idx == 0) ?	WordPosition.Beginning : (idx == wrd.length - 1) ?	WordPosition.End : WordPosition.Middle;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// WordDetails for horizontal and vertical word membership
	/// </summary>
	
	public WordDetails[] wordDetails = new WordDetails[2];

	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Setup. Init the Piece
	/// </summary>
	
	public void Setup(Transform parent)
	{
		wordDetails[Word.horizontal] = new WordDetails();
		wordDetails[Word.vertical] = new WordDetails();

		tileTransform = transform.Find ("Tile");
		tileTransform.localPosition = new Vector3(0, -96, 0);
		tileTransform.localScale = new Vector3(1, 1, 1);
		
		letterTransform = transform.Find ("Letter");
		tileSpriteRenderer = tileTransform.GetComponent<SpriteRenderer> ();
		letterTextMesh = letterTransform.GetComponent<TextMesh> ();
		letterTextMesh.transform.Translate(new Vector3(48, 48, 0));
		tileSpriteRenderer.sortingOrder = 1;
		letterTextMesh.renderer.sortingOrder = 2;
		
		transform.parent = parent;
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
		if(u == 0 && v == 0)
		{
			v = 4;
		}
		SetSprite(Main.tileFrames[u + v * 5]);
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// SetSprite. Change the tile background
	/// </summary>
	
	public void SetSprite(Sprite frame)
	{
		tileSpriteRenderer.sprite = frame;
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Letter. get/set the Letter on the Piece
	/// </summary>
	
	public char Letter
	{
		get
		{
			return letter;
		}
		set
		{
			letter = value;
			letterTextMesh.text = new string(char.ToUpper(letter), 1);
		}
		
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	
	void OnMouseDown()
	{
		Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
		xoffset = transform.position.x - v.x;
		yoffset = transform.position.y - v.y;
		tileSpriteRenderer.sortingOrder = 3;
		letterTextMesh.renderer.sortingOrder = 4;
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	
	void OnMouseUp()
	{
		tileSpriteRenderer.sortingOrder = 1;
		letterTextMesh.renderer.sortingOrder = 2;
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	
	void OnMouseDrag()
	{
		Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
		transform.position = new Vector3(v.x + xoffset, v.y + yoffset, 0);
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	
	void Start ()
	{
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	
	void Update ()
	{
		x = Input.mousePosition.x;
		y = Input.mousePosition.y;
	}
}

