//////////////////////////////////////////////////////////////////////

using System;
using Util;

//////////////////////////////////////////////////////////////////////

public static class Letters
{
    //////////////////////////////////////////////////////////////////////

    private static int[] letters = {
		1,9,	//A
		3,2,	//B
		3,2,	//C
		2,4,	//D
		1,12,	//E
		4,2,	//F
		2,3,	//G
		4,2,	//H
		1,9,	//I
		8,1,	//J
		5,1,	//K
		1,4,	//L
		3,2,	//M
		1,6,	//N
		1,8,	//O
		3,2,	//P
		10,1,	//Q
		1,6,	//R
		1,4,	//S
		1,6,	//T
		1,4,	//U
		4,2,	//V
		4,2,	//W
		8,1,	//X
		4,2,	//Y
		10,1	//Z
	};

    //////////////////////////////////////////////////////////////////////

    private static char[] GetDistributionTable()
	{
		int size = 0;
		for(int i=0; i < letters.Length; i += 2)
		{
			size += letters[i + 1];
		}
		char[] dist = new char[size];
		int k = 0;
		for(int i=0; i < letters.Length; i += 2)
		{
			for(int j=0; j < letters[i + 1]; ++j)
			{
				dist[k++] = (char)('a' + (i / 2));
			}
		}
		return dist;
	}

    //////////////////////////////////////////////////////////////////////

    private static char[] distribution = GetDistributionTable();
	private static Util.Random random = new Util.Random();

    //////////////////////////////////////////////////////////////////////

    public static void Seed(UInt32 seed)
	{
		random.Seed(seed);
	}

    //////////////////////////////////////////////////////////////////////

    public static int GetScore(char letter)
	{
		return letters[((int)char.ToLower(letter) - 'a') * 2];
	}

    //////////////////////////////////////////////////////////////////////

    public static char GetRandomLetter()
	{
		return distribution[random.Next() % distribution.Length];
	}
}

