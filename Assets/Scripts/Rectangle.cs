﻿//////////////////////////////////////////////////////////////////////

public class Rectangle
{
    //////////////////////////////////////////////////////////////////////

    public Point origin;
    public Point size;

    //////////////////////////////////////////////////////////////////////

    public Rectangle()
    {
    }

    //////////////////////////////////////////////////////////////////////

    public Rectangle(int x, int y, int w, int h)
    {
        origin = new Point(x, y);
        size = new Point(w, h);
    }

    //////////////////////////////////////////////////////////////////////

    public int Left
    {
        get { return origin.x; }
        set { origin.x = value; }
    }

    //////////////////////////////////////////////////////////////////////

    public int Right
    {
        get { return origin.x + size.x; }
        set { size.x = value - origin.x; }
    }

    //////////////////////////////////////////////////////////////////////

    public int Top
    {
        get { return origin.y; }
        set { origin.y = value; }
    }

    //////////////////////////////////////////////////////////////////////

    public int Bottom
    {
        get { return origin.y + size.y; }
        set { size.y = value - origin.y; }
    }

    //////////////////////////////////////////////////////////////////////

    public int Width
    {
        get { return size.x; }
        set { size.x = value; }
    }

    //////////////////////////////////////////////////////////////////////

    public int Height
    {
        get { return size.y; }
        set { size.y = value; }
    }

    //////////////////////////////////////////////////////////////////////

    public bool Contains(Point p)
    {
        return p.x >= Left && p.y >= Top && p.x < Right && p.y < Bottom;
    }
}