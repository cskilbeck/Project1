using UnityEngine;
using System.Collections;
using MTW;
using Font;
using TypeFace = Font.TypeFace;

public class Main : MonoBehaviour {

	public int boardWidth;
	public int boardHeight;

	[HideInInspector]
	public static Sprite[] tileFrames = new Sprite[25];

	[HideInInspector]
	public Board board;

	[HideInInspector]
	public TypeFace arialFont;

	Font.Text t;

	private void CreateTiles()
	{
		Texture2D t = (Texture2D)Resources.Load("allColour");
		t.filterMode = FilterMode.Trilinear;
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

		TextAsset dictionary = (TextAsset)Resources.Load ("Dictionary");
		MTW.Dictionary.Init(dictionary);

		GameObject boardPrefab = (GameObject)Resources.Load("Board");
		board = ((GameObject)Instantiate(boardPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Board>();
		board.Setup();
		board.MarkAllWords();
		
		arialFont = new TypeFace("Arial");
		t = new Font.Text (arialFont, "HELLO");
	}
	
	// Update is called once per frame
	void Update ()
	{
		t.root.transform.rotation = Quaternion.identity;
		t.root.transform.position = new Vector3 (100, 100);
		t.root.transform.RotateAround (new Vector3 (200, 120, 0), Vector3.forward, Mathf.Sin(Time.realtimeSinceStartup * 4) * 90);
	}
}
