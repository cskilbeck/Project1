//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////

public static class Dictionary
{
    //////////////////////////////////////////////////////////////////////

    public class Word
	{
		public int index;
		public string text;
		public string definition;
		public int score;

		public Word(int i, string t, string d, int s)
		{
			index = i;
			text = t;
			definition = d;
			score = s;
		}
	}

    //////////////////////////////////////////////////////////////////////

    private static Dictionary<string, Word> words = new Dictionary<string, Word>();

    //////////////////////////////////////////////////////////////////////

    public static void Init()
	{
        TextAsset dictionary = (TextAsset)Resources.Load("Dictionary");
        StringReader reader = new StringReader(dictionary.text);
		string line;
		int i = 0;
		while((line = reader.ReadLine()) != null) {
			int space = line.IndexOf(" ");
			string word = line.Substring(0, space);
			string definition = line.Substring(space + 1);
			words.Add(word, new Dictionary.Word (i, word, definition, CalculateWordScore(word)));
			++i;
		}
	}

    //////////////////////////////////////////////////////////////////////

    private static int CalculateWordScore(string word)
	{
		int score = 0;
        for (int i = 0, l = word.Length; i < l; ++i)
        {
			score += Letters.GetScore(word[i]);
		}
		return score * word.Length;
	}

    //////////////////////////////////////////////////////////////////////

    public static Word GetWord(string s)
	{
		return words[s];
	}

    //////////////////////////////////////////////////////////////////////

    public static bool IsWord(string word)
	{
		return words.ContainsKey(word);
	}

    //////////////////////////////////////////////////////////////////////

    public static int GetScore(string word)
	{
		return IsWord(word) ? GetWord(word).score : -1;
	}

    //////////////////////////////////////////////////////////////////////

    public static string GetDefinition(string word)
	{
		return IsWord(word) ? GetWord(word).definition : "?";
	}
}

