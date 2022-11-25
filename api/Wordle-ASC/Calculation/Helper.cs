using Calculation.Services;
using Database;
using Database.Entities;
using Database.Enums;
using Model.Models;

namespace Calculation;

public static class Helper
{
	public static List<Pattern> GetAllPatternsForAWord(string desiredWord, List<string> words)
	{
		var patterns = Constants.ALL_PATTERNS;
		var possiblePatternsDictionary = patterns.ToDictionary(pattern => pattern.ToString(), _ => false);

		foreach (var word in words)
		{
			var producedPattern = TestWord(word, desiredWord).ToString();
			possiblePatternsDictionary[producedPattern] = true;
		}

		var result = new List<Pattern>();

		foreach (var possiblePattern in possiblePatternsDictionary)
		{
			if (possiblePattern.Value)
			{
				result.Add(new Pattern(possiblePattern.Key));
			}
		}

		return result;
	}

	public static Pattern TestWord(string answer, string word)
	{
		var ans = new Pattern();

		var copyWord = word.ToCharArray();
		var copyAns = answer.ToCharArray();

		if (answer.Length != Constants.WORD_SIZE || word.Length != Constants.WORD_SIZE)
		{
			throw new Exception("Invalid parameters");
		}

		for (var i = 0; i < word.Length; i++)
		{
			if (answer[i] != word[i])
			{
				continue;
			}

			ans.Content[i] = MatchWays.ExistAtTheSamePosition;
			copyWord[i] = Constants.CONTROL_CHAR;
		}

		for (var i = 0; i < word.Length; i++)
		{
			if (copyWord[i] == Constants.CONTROL_CHAR)
			{
				continue;
			}

			var found = copyAns.FirstOrDefault(chr => chr == copyWord[i]);

			if (char.IsControl(found))
			{
				continue;
			}

			ans.Content[i] = MatchWays.ExistInWord;
		}
		return ans;
	}

	public static List<string> PossibleWays(string word, Pattern pattern, ICollection<string> words)
	{
		var desired = words.Where(w =>
		{
			for (var i = 0; i < word.Length; i++)
			{
				if (pattern.Content[i] == MatchWays.NotExist)
				{
					if (w.Contains(word[i]))
					{
						return false;
					}
				}
				else if (pattern.Content[i] == MatchWays.ExistAtTheSamePosition)
				{
					if (word[i] != w[i])
					{
						return false;
					}
				}
				else if (pattern.Content[i] == MatchWays.ExistInWord)
				{
					if (!w.Contains(word[i]) || w[i] == word[i])
					{
						return false;
					}
				}
			}

			return true;
		}).ToList();

		return desired;
	}

	public static ICollection<string>? GetWordsFromFile(string filePath)
	{
		var lines = File.ReadAllLines(filePath);

		return lines.Where(line => line.Length == Constants.WORD_SIZE).ToList();
	}

	public static async Task AddWordsToDatabaseAsync(string filepath)
	{
		var uoW = new UoW();

		var words = GetWordsFromFile(filepath);

		foreach (var word in words!)
		{
			await WordService.AddAsync(new Word
			{
				Text = word
			}, uoW);
		}
	}
}

