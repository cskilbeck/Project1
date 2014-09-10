////////////////////////////////////////////////////////////////////////////////////
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

	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// WordPosition. Where in a word is this tile
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
	}

	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// WordDetails for horizontal and vertical word membership
	/// </summary>

	public WordDetails horizontalWordDetails = new WordDetails();
	public WordDetails verticalWordDetails = new WordDetails();

	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Setup. Init the Piece
	/// </summary>

	public void Setup(Transform parent)
	{
		tileTransform = transform.Find ("Tile");
		letterTransform = transform.Find ("Letter");
		tileSpriteRenderer = tileTransform.GetComponent<SpriteRenderer> ();
		letterTextMesh = letterTransform.GetComponent<TextMesh> ();
		letterTextMesh.transform.Translate(new Vector3(48, 48, 0));
		tileSpriteRenderer.sortingOrder = 1;
		tileTransform.localPosition = new Vector3(0, -96, 0);
		tileTransform.localScale = new Vector3(1, 1, 1);
		letterTextMesh.renderer.sortingOrder = 2;
		transform.parent = parent;
	}

	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// ResetWords. Clear out horiz, vert word details
	/// </summary>

	public void ResetWords()
	{
		horizontalWordDetails.position = WordPosition.None;
		verticalWordDetails.position = WordPosition.None;
	}

	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// SetWord. Assign this tile to a word
	/// </summary>

	public void SetWord(Word w, int index)
	{
		WordPosition p =	(index == 0)			?	WordPosition.Beginning :
							(index == w.length - 1) ?	WordPosition.End :
														WordPosition.Middle;
		WordDetails d = (w.orientation == Word.Orientation.horizontal) ? horizontalWordDetails : verticalWordDetails;
		d.word = w;
		d.index = index;
		d.position = p;
	}

	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// SetTile. Change the tile background based on the word membership
	/// </summary>

	public void SetupTile()
	{
		int u = (int)horizontalWordDetails.position;
		int v = (int)verticalWordDetails.position;
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
	
	void Start ()
	{
	}
	
	////////////////////////////////////////////////////////////////////////////////////
	
	void Update ()
	{	
	}
}
