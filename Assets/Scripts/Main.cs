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

    [HideInInspector]
    public TypeFace debugFont;

    [HideInInspector]
    public TypeFace calibriFont;

    //////////////////////////////////////////////////////////////////////

    Piece piece2;
	Font.Text t;
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
        piece2.Sprite = Tiles.Get(0, 0);
        piece2.Letter = 'Z';
        piece2.Position = new Vector2(600, 600);

        board = new Board();
		
		t = new Font.Text(arialFont, "HELLO");
        t.transform.position = new Vector3(100, 100);

        banner = new Font.Text(calibriFont, "ga");
        banner.transform.position = new Vector3(100, 600, 0);

        debugMessage = new Font.Text(debugFont, "Hello, World!");
        debugMessage.transform.position = new UnityEngine.Vector3(100, 700);
	}

    //////////////////////////////////////////////////////////////////////

    void Update()
	{
        board.Update();
        piece2.Rotation = Time.realtimeSinceStartup * 25;
	}
}
