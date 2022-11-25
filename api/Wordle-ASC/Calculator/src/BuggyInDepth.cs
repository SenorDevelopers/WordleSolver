using Database;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Calculator;

public class BuggyInDepth
{
	private const int MAX_THREADS = 20;
	public readonly List<List<MatchWays>> Patterns;

	public BuggyInDepth()
	{
		Patterns = GetAllPatterns();
	}

	public async Task StartCalculateAsync()
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.ToListAsync();

		var tasks = new List<Task>();

		await Task.Run(async () =>
		{
			var i = 0;
			foreach (var word in words)
			{
				tasks.Add(CalculateForOneWordAsync(word.Text));
				i++;
				if (i < MAX_THREADS)
				{
					continue;
				}

				await Task.WhenAll(tasks);
				tasks.Clear();
				i = 0;
			}
		});

		await Task.WhenAll(tasks);
	}

	public async Task CalculateForXWords(int numberOfWords)
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.OrderByDescending(w => w.Entropy)
			.Take(numberOfWords)
			.ToListAsync();

		var tasks = new List<Task>();

		await Task.Run(async () =>
		{
			var i = 0;
			foreach (var word in words)
			{
				tasks.Add(CalculateForOneWordAsync(word.Text));
				i++;
				if (i < MAX_THREADS)
				{
					continue;
				}

				await Task.WhenAll(tasks);
				tasks.Clear();
				i = 0;
			}
		});

		await Task.WhenAll(tasks);
	}

	private static async Task CalculateForOneWordAsync(string word)
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.ToListAsync();
		var wordsText = await dbContext.Words!.Select(w => w.Text).ToListAsync();
		var totalWords = words.Count;
		var helper = new Helper();
		double average = 0;

		double entropy = 0;
		var possiblePatterns = helper.GetAllPatternsForAWord(word, wordsText);

		foreach (var pattern in possiblePatterns)
		{
			var wordsNow = Helper.PossibleWays(word, pattern, wordsText);
			var numberOfPossibleWays = wordsNow.Count;

			if (numberOfPossibleWays == 0)
			{
				continue;
			}

			var ans = await CalculatedInList(wordsNow, dbContext);

			var probability = (double)numberOfPossibleWays / totalWords;

			var addValue = probability * Math.Log2(probability);

			entropy -= addValue;

			average += ans * probability;
		}

		var wordDb = await dbContext.Words.FirstOrDefaultAsync(w => w.Text == word);

		wordDb!.Entropy = (decimal)entropy + (decimal)average;

		await dbContext.SaveChangesAsync();
	}


	public static List<string> PossibleWays(string word, List<MatchWays> pattern, ICollection<string> words)
	{
		var desired = words.Where(w =>
		{
			var ok = true;
			for (var i = 0; i < word.Length && ok; i++)
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
		}).ToList();

		return desired;
	}

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
		}

		for (var i = 0; i < word.Length; i++)
		{
			if (copyWord[i] == OK)
			{
				continue;
			}

			var found = copyAns.FirstOrDefault(chr => chr == copyWord[i]);

			if (char.IsControl(found))
			{
				continue;
			}

			ans[i] = MatchWays.ExistInWord;
		}

		return ans;
	}

	public static async Task<double> CalculatedInList(List<string> currentWords, UoW dbContext)
	{
		var words = await dbContext.Words!.Select(w => w.Text).ToListAsync();
		var totalWords = currentWords.Count;
		var buggy = new Buggy();
		double ans = 0;

		foreach (var word in words)
		{
			double entropy = 0;
			var possiblePatterns = buggy.GetAllPatternsForAWord(word, currentWords);
			foreach (var pattern in possiblePatterns)
			{
				var wordsNow = Buggy.PossibleWays(word, pattern, currentWords);
				var numberOfPossibleWays = wordsNow.Count;

				if (numberOfPossibleWays == 0)
				{
					continue;
				}

				var probability = (double)numberOfPossibleWays / totalWords;

				var addValue = probability * Math.Log2(probability);

				entropy -= addValue;
			}
			ans = Math.Max(entropy, ans);
		}

		return ans;
	}
}

