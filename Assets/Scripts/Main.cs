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
    Font.TypeFace arialFont;
    Font.TypeFace debugFont;
    Font.TypeFace calibriFont;
	Font.Text helloText;
    Font.Text debugMessage;
    Font.Text banner;

    //////////////////////////////////////////////////////////////////////

    void Start()
	{
		Screen.SetResolution(1280, 720, false);
        Camera.main.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, Screen.height, 0, 0, 100);
        Tiles.Create();
		Dictionary.Init();
        debugFont = new TypeFace("Fixedsys");
        arialFont = new TypeFace("Arial");
        calibriFont = new TypeFace("Calibri");
        Piece.SetTypeFace(arialFont);
        piece2 = new Piece();
        piece2.Sprite = Tiles.Get(4, 2);
        piece2.Letter = 'Z';
        piece2.Position = new Vector2(500, 540);
        board = new Board();
		helloText = new Font.Text(arialFont, "HELLO");
        helloText.transform.position = new Vector3(800, 650);
        banner = new Font.Text(calibriFont, "This is a piece of text which should, ultimately, be correctly rendered.... BUT IS IT? That's the questioning bit...");
        banner.transform.position = new Vector3(100, 600, 0);
        debugMessage = new Font.Text(debugFont, "Hello, World!");
        debugMessage.transform.position = new UnityEngine.Vector3(100, 700);
	}

    //////////////////////////////////////////////////////////////////////

    void Update()
	{
        board.Update();
        piece2.Rotation = Time.realtimeSinceStartup * 180 + Mathf.Sin(Time.realtimeSinceStartup * 8) * 18 + 14.5f;
	}
}
