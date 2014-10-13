//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UI;
using System.Xml;
using System.Xml.Serialization;

//////////////////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
    public static int boardWidth = 7;
	public static int boardHeight = 5;
    
	public static Board board;

    Tile piece2;
    TypeFace arialFont;
    TypeFace debugFont;
    TypeFace calibriFont;
    TypeFace digitsFont;
	Label helloText;
    Label debugMessage;
    Label banner;

    void Start()
	{
        Screen.SetResolution(1280, 720, false);
        Camera.main.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, Screen.height, 0, 0, 100);
        //Tiles.Create();
        //Letters.Seed(56);
        //Dictionary.Init();
        //board = Util.Create<Board>();
        //debugFont = TypeFace.Load("Fixedsys");
        //arialFont = TypeFace.Load("Arial");
        //calibriFont = TypeFace.Load("Calibri");
        //digitsFont = TypeFace.Load("digits");
        //Tile.SetTypeFace(arialFont, digitsFont);
        //piece2 = Util.Create<Tile>();
        //piece2.Sprite = Tiles.Get(4, 2);
        //piece2.Letter = 'Z';
        //piece2.Position = new Vector2(500, 540);
        //helloText = Label.Create(arialFont, "HELLOWORLD");
        //helloText.transform.position = new Vector3(800, 650);
        //banner = Label.Create(calibriFont, "This is a piece of text which should, ultimately, be correctly rendered.... BUT IS IT? That's the questioning bit...");
        //banner.transform.position = new Vector3(100, 600, 0);
        //debugMessage = Label.Create(debugFont, "Hello, World!");
        //debugMessage.transform.position = new Vector3(100, 700);
        Vector3 v = Vector3.zero;
        v = Camera.main.ScreenToWorldPoint(v);
        Debug.Log("2: " + v.ToString());
    }

    //////////////////////////////////////////////////////////////////////

    void Update()
	{
        //piece2.Rotation = Time.realtimeSinceStartup * 180 + Mathf.Sin(Time.realtimeSinceStartup * 8) * 18 + 14.5f;
	}
}
