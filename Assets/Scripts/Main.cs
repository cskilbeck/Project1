//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using Font;
using TypeFace = Font.TypeFace;

//////////////////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
    public static int boardWidth = 7;
	public static int boardHeight = 5;

    //////////////////////////////////////////////////////////////////////

	[HideInInspector]
	public Board board;

	[HideInInspector]
	public TypeFace arialFont;

    //////////////////////////////////////////////////////////////////////

    Piece piece2;
	Font.Text t;

    //////////////////////////////////////////////////////////////////////

    void Start()
	{
		Screen.SetResolution(1280, 720, false);
        Camera.main.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, Screen.height, 0, 0, 100);

        Tiles.Create();
		Dictionary.Init();

        arialFont = new TypeFace("Arial");
        
        Piece.SetTypeFace(arialFont);
        piece2 = Piece.Create();
        piece2.Sprite = Tiles.Get(0, 0);
        piece2.Letter = 'Z';
        piece2.Position = new Vector2(600, 600);

        GameObject boardPrefab = (GameObject)Resources.Load("Board");
		board = ((GameObject)Instantiate(boardPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Board>();
		board.Setup();
		board.MarkAllWords();
		
		t = new Font.Text (arialFont, "HELLO");
	}

    //////////////////////////////////////////////////////////////////////

    void Update()
	{
		t.root.transform.rotation = Quaternion.identity;
		t.root.transform.position = new Vector3 (100, 100);
		t.root.transform.RotateAround (new Vector3 (200, 120, 0), Vector3.forward, Mathf.Sin(Time.realtimeSinceStartup * 2) * 90);

        piece2.Rotation = Time.realtimeSinceStartup * 25 ;
	}
}
