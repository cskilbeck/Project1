//////////////////////////////////////////////////////////////////////

using UnityEngine;
using Font;

//////////////////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
    public static int boardWidth = 7;
	public static int boardHeight = 5;
    
	Board board;
    Piece piece2;
    TypeFace arialFont;
    TypeFace debugFont;
    TypeFace calibriFont;
	Text helloText;
    Text debugMessage;
    Text banner;

    //////////////////////////////////////////////////////////////////////

    void Start()
	{
        Debug.Log("Main: Start");
        Screen.SetResolution(1280, 720, false);
        Camera.main.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, Screen.height, 0, 0, 100);
        Tiles.Create();
        Letters.Seed(56);
		Dictionary.Init();
        board = Util.Create<Board>();
        debugFont = TypeFace.Load("Fixedsys");
        arialFont = TypeFace.Load("Arial");
        calibriFont = TypeFace.Load("Calibri");
        Piece.SetTypeFace(arialFont);
        piece2 = Util.Create<Piece>();
        piece2.Sprite = Tiles.Get(4, 2);
        piece2.Letter = 'Z';
        piece2.Position = new Vector2(500, 540);
		helloText = Text.Create(arialFont, "HELLOWORLD");
        helloText.transform.position = new Vector3(800, 650);
        banner = Text.Create(calibriFont, "This is a piece of text which should, ultimately, be correctly rendered.... BUT IS IT? That's the questioning bit...");
        banner.transform.position = new Vector3(100, 600, 0);
        debugMessage = Text.Create(debugFont, "Hello, World!");
        debugMessage.transform.position = new Vector3(100, 700);
	}

    //////////////////////////////////////////////////////////////////////

    void Update()
	{
        piece2.Rotation = Time.realtimeSinceStartup * 180 + Mathf.Sin(Time.realtimeSinceStartup * 8) * 18 + 14.5f;
	}
}
