using Database.Enums;

namespace Calculator;

public class Helper
{
	public const string GUESSED_STRING = "22222";
	public readonly List<List<MatchWays>> Patterns;

	public Helper()
	{
		Patterns = GetAllPatterns();
	}

	public static ICollection<string>? GetWords(string filePath, int wordSize)
	{
		var lines = File.ReadAllLines(filePath);

		return lines.Where(line => line.Length == wordSize).ToList();
	}

	//public static async Task AddWordsToDatabaseAsync(string filepath)
	//{
	//	//var dbContext = new UoW();
	//	//var wordRepository = new WordService(dbContext);  // TODO change these, inject UoW

	//	//var words = GetWords(filepath, 5);

	//	//foreach (var word in words!)
	//	//{
	//	//	await dbContext.Words.AddAsync(new Word
	//	//	{
	//	//		Text = word
	//	//	});
	//	//}

	//	//await dbContext.SaveChangesAsync();
	//}

	public List<List<MatchWays>> GetAllPatternsForAWord(string desiredWord, List<string> words)
	{
		var possiblePatternsDictionary = Patterns.ToDictionary(SecondGuess.ToPatternString, _ => false);
		foreach (var word in words)
		{
			var producedPattern = SecondGuess.ToPatternString(TestWord(word, desiredWord));
			possiblePatternsDictionary[producedPattern] = true;
		}

		var result = new List<List<MatchWays>>();

		foreach (var possiblePattern in possiblePatternsDictionary)
		{
			if (possiblePattern.Value)
			{
				result.Add(Helper.FromStringToPattern(possiblePattern.Key));
			}
		}

		return result;
	}

	public static List<List<MatchWays>> GetAllPatterns()
	{
		const int LENGTH = 5;

		var patterns = new List<List<MatchWays>>();
		var currentState = Enumerable.Repeat(MatchWays.NotExist, LENGTH).ToList();

		GenerateAllPatterns(ref patterns, LENGTH, 0, ref currentState);

		return patterns;
	}

	private static void GenerateAllPatterns(ref List<List<MatchWays>> patterns, int length, int currentLength,
		 ref List<MatchWays> currentPattern)
	{
		if (currentLength == length)
		{
			var aux = currentPattern.ToList();
			patterns.Add(aux);
		}
		else
		{
			currentPattern[currentLength] = MatchWays.NotExist;

			while (currentPattern[currentLength] <= MatchWays.ExistAtTheSamePosition)
			{
				GenerateAllPatterns(ref patterns, length, currentLength + 1, ref currentPattern);
				currentPattern[currentLength]++;
			}
		}
	}

	public static int NumberOfPossibleWays(string word, List<MatchWays> pattern, in ICollection<string> words)
	{
		return PossibleWays(word, pattern, words).Count;
	}

	public static List<string> PossibleWays(string word, List<MatchWays> pattern, in ICollection<string> words)
	{
		var desiredWords = words.Where(w =>
		{
			if (w == word)
			{
				return false;
			}

			var copyWord = w.ToCharArray();

			for (var i = 0; i < pattern.Count; i++)
			{
				if (pattern[i] != MatchWays.ExistAtTheSamePosition)
				{
					continue;
				}

				if (w[i] != word[i])
				{
					return false;
				}

				copyWord[i] = ' ';
			}

			for (var i = 0; i < pattern.Count; i++)
			{
				if (pattern[i] != MatchWays.ExistInWord)
				{
					continue;
				}

				if (copyWord[i] == word[i])
				{
					return false;
				}

				var found = copyWord.FirstOrDefault(chr => chr == word[i]);
				if (char.IsControl(found))
				{
					return false;
				}

				copyWord[Array.IndexOf(copyWord, word[i])] = ' ';
			}

			for (var i = 0; i < pattern.Count; i++)
			{
				if (pattern[i] != MatchWays.NotExist)
				{
					continue;
				}

				var found = copyWord.FirstOrDefault(chr => chr == word[i]);
				if (!char.IsControl(found))
				{
					return false;
				}
			}

			return true;
		}).ToList();

		return desiredWords;
	}

	public static int Test(string word, List<MatchWays> pattern, in ICollection<string> words)
	{
		var desired = words.Count(w =>
		{
			var ok = true;
			for (int i = 0; i < word.Length && ok; i++)
			{
				if (pattern[i] == MatchWays.NotExist)
				{
					if (w.Contains(word[i]))
					{
						ok = false;
					}
				}
				else if (pattern[i] == MatchWays.ExistAtTheSamePosition)
				{
					if (word[i] != w[i])
					{
						ok = false;
					}
				}
				else
				{
					if (!w.Contains(word[i]) || w[i] == word[i])
					{
						ok = false;
					}
				}
			}

			return ok;
		});

		return desired;
	}

	public static List<MatchWays> FromStringToPattern(string pattern)
	{
		var result = new List<MatchWays>();

		foreach (var c in pattern)
		{
			switch (c)
			{
				case ('0'):
					result.Add(MatchWays.NotExist);
					break;

				case ('1'):
					result.Add(MatchWays.ExistInWord);
					break
						;
				case ('2'):
					result.Add(MatchWays.ExistAtTheSamePosition);
					break;
			}
		}

		return result;
	}

	public static bool IsGuessed(List<MatchWays> pattern)
	{
		return pattern.Count(mw => mw == MatchWays.ExistAtTheSamePosition) == 4 &&
			   pattern.Count(mw => mw == MatchWays.ExistInWord) == 1;
	}

	public static List<MatchWays> TestWord(string answer, string word)
	{
		const char OK = '0';

		var ans = new List<MatchWays>()
		{
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist
		};

		var copyWord = word.ToCharArray();
		var copyAns = answer.ToCharArray();

		if (answer.Length < 5 || word.Length < 5)
		{
			throw new Exception("Invalid parameters");
		}

		for (var i = 0; i < word.Length; i++)
		{
			if (answer[i] != word[i])
			{
				continue;
			}

			ans[i] = MatchWays.ExistAtTheSamePosition;
			copyWord[i] = OK;
			copyAns[i] = OK;
		}

		for (var i = 0; i < word.Length; i++)
		{
			if (copyAns[i] == OK)
			{
				continue;
			}

			var found = copyWord.FirstOrDefault(chr => chr == copyAns[i]);

			if (char.IsControl(found))
			{
				continue;
			}

			var index = Array.IndexOf(copyWord, answer[i]);
			copyWord[index] = OK;
			ans[index] = MatchWays.ExistInWord;

		}

		return ans;
	}
}
