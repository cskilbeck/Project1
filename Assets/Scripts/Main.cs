using UnityEngine;
using System.Collections;
using MTW;
using Font = FontUtil.Font;

public class Main : MonoBehaviour {

	public int boardWidth;
	public int boardHeight;

	[HideInInspector]
	public static Sprite[] tileFrames = new Sprite[25];

	[HideInInspector]
	public Board board;

	[HideInInspector]
	public Font arialFont;

	GameObject[] letter = new GameObject[2];

	private void CreateTiles()
	{
		Texture2D t = (Texture2D)Resources.Load("allColour");
		t.filterMode = FilterMode.Point;
		int i = 0;
		for(int y=0; y<5; ++y)
		{
			for(int x=0; x<5; ++x)
			{
				Sprite s = Sprite.Create(t, new Rect(x * 96, t.height - (y * 96) - 96 , 96, 96), Vector2.zero, 1);
				tileFrames[i++] = s;
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		Screen.SetResolution(1280, 720, false);
		CreateTiles();
		Camera.main.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, Screen.height, 0, 0, 100);
//		TextAsset dictionary = (TextAsset)Resources.Load ("Dictionary");
//		MTW.Dictionary.Init(dictionary);

		// Load the texture
		// create a gameobject
		// add a spriterenderer to the gameobject
		// create a sprite from the texture
		// set the spriterenderer.sprite to the sprite

		//fontPage = (GameObject)Instantiate<FontPage>();
		//fontPage.transform.parent = main.transform;

		arialFont = new Font("Arial");

		letter[0] = new GameObject();
		letter[1] = new GameObject();

		letter[0].AddComponent<SpriteRenderer>();
		letter[1].AddComponent<SpriteRenderer>();

		letter[0].GetComponent<SpriteRenderer>().sprite = arialFont.glyphs['A'].images[0];
		letter[1].GetComponent<SpriteRenderer>().sprite = arialFont.glyphs['A'].images[1];

		letter[0].transform.localPosition = arialFont.glyphs['A'].offsets[0];
		letter[1].transform.localPosition = arialFont.glyphs['A'].offsets[1];

		//letter[0].transform.localRotation = Quaternion.AngleAxis(180, Vector3.forward);
		//letter[1].transform.localRotation = Quaternion.AngleAxis(180, Vector3.forward);
		letter[1].transform.parent = letter[0].transform;

		letter[0].transform.Translate(new Vector2(100, 100));


//		GameObject boardPrefab = (GameObject)Resources.Load("Board");
//		board = ((GameObject)Instantiate(boardPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Board>();
//		board.Setup();
//		board.MarkAllWords();

		//arialFont.CreateLetter('A', 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//letter[0].transform.Translate(new Vector3(0.1f,0.1f,0));
//		newPage.transform.RotateAround(new Vector3(256, 256, 0), Vector3.forward, 1);
//		fontPage.transform.Translate(new Vector3(1.5f, 2.5f, 0));
//		newPage.GetComponent<SpriteRenderer>().sprite = tileFrames[0];
	}
}
