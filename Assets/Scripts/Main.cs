using UnityEngine;
using System.Collections;
using MTW;

public class Main : MonoBehaviour {

	public int boardWidth;
	public int boardHeight;

	[HideInInspector]
	public static Sprite[] tileFrames = new Sprite[25];

	[HideInInspector]
	public Board board;

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
		Debug.Log (Screen.width.ToString() + "," + Screen.height.ToString());
		CreateTiles();
		Camera.main.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, Screen.height, 0, 0, 100);
		TextAsset dictionary = (TextAsset)Resources.Load ("Dictionary");
		MTW.Dictionary.Init(dictionary);

		GameObject boardPrefab = (GameObject)Resources.Load("Board");
		board = ((GameObject)Instantiate(boardPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Board>();
		board.Setup();

		board.MarkAllWords();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
