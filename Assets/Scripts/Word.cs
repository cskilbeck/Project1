using System;
using DictionaryWord = MTW.Dictionary.Word;

namespace MTW
{
	public class Word : IComparable
	{
		public enum Orientation
		{
			horizontal = 0,
			vertical = 1
		}

		public int x;
		public int y;
		public Orientation orientation;
		public DictionaryWord word;

		public int CompareTo(Object o)
		{
			Word b = o as Word;
			return (score < b.score) ? 1 :
					(score > b.score) ? -1 : 
					(length < b.length) ? 1 :
					(length > b.length) ? -1 :
					(word.text.Length - b.word.text.Length);
		}

		public Word (int x, int y, Orientation orientation, MTW.Dictionary.Word word)
		{
			this.x = x;
			this.y = y;
			this.orientation = orientation;
			this.word = word;
		}

		public int length
		{
			get
			{
				return word.text.Length;
			}
		}

		public int score
		{
			get
			{
				return word.score;
			}
		}
	}
}

