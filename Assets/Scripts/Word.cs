//////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////

public class Word : IComparable
{
    //////////////////////////////////////////////////////////////////////

    public static int horizontal = 0;
	public static int vertical = 1;

    //////////////////////////////////////////////////////////////////////

    public int x;
	public int y;
	public int orientation;
	public Dictionary.Word word;
    public int sortIndex;

    //////////////////////////////////////////////////////////////////////

    public int CompareTo(Object o)
	{
		Word b = o as Word;
        return (score < b.score) ? 1 :
                (score > b.score) ? -1 :
                (length < b.length) ? 1 :
                (length > b.length) ? -1 :
                (word.text.Length < b.word.text.Length) ? 1 :
                (sortIndex < b.sortIndex) ? -1 : 1;
	}

    //////////////////////////////////////////////////////////////////////

    private void Init(int x, int y, int orientation, Dictionary.Word word)
    {
        this.x = x;
        this.y = y;
        this.orientation = orientation;
        this.word = word;
    }

    //////////////////////////////////////////////////////////////////////

    public Word(int x, int y, int orientation, Dictionary.Word word)
	{
        Init(x, y, orientation, word);
	}

    //////////////////////////////////////////////////////////////////////

    public int length
	{
		get
		{
			return word.text.Length;
		}
	}

    //////////////////////////////////////////////////////////////////////

    public int score
	{
		get
		{
			return word.score;
		}
	}
}

