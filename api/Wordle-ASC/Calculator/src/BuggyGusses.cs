using Database;
using Database.Entities;
using Database.Enums;

namespace Calculator;

public class BuggyGusses
{
	private readonly List<string> _wordsStrings;
	private readonly Buggy _helper;

	private const string GUESSED_PATTERN = "22222";
	private const int MAX_THREADS = 20;

	public BuggyGusses()
	{
		var dbContext = new UoW();
		_wordsStrings = dbContext.Words!.Select(w => w.Text).ToList();
		_helper = new Buggy();
	}

	public async Task StartCalculateGuesses(string opener)
	{
		const int CURRENT_GUESS_NUMBER = 1;

		var possiblePatterns = _helper.GetAllPatternsForAWord(opener, _wordsStrings);

		var tasks = new List<Task>();

		var i = 0;

		await Task.Run(async () =>
		{
			var dbContext = new UoW();
			foreach (var pattern in possiblePatterns)
			{
				var previousGuess = await dbContext.Guesses.AddAsync(new Guess
				{
					GuessNumber = CURRENT_GUESS_NUMBER,
					GuessString = opener,
					Pattern = SecondGuess.ToPatternString(pattern)
				});

				await dbContext.SaveChangesAsync();

				var wordsNow = Buggy.PossibleWays(opener, pattern, _wordsStrings);
				if (wordsNow.Count == 0 ||
					SecondGuess.ToPatternString(pattern) == Helper.GUESSED_STRING)
				{
					continue;
				}

				i++;

				var threadContext = new UoW();
				tasks.Add(CalculateGuesses(CURRENT_GUESS_NUMBER + 1, pattern, wordsNow, previousGuess.Entity.Id, threadContext));

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

	public async Task CalculateGuesses(int currentGuessNumber, List<MatchWays> currentGuessPattern,
		List<string> currentWords, Guid previousGuessId, UoW dbContext)
	{
		await Task.Run(async () =>
		{
			var ans = CalculateEntropy(currentWords);

			var previousGuess = await dbContext.Guesses.AddAsync(new Guess
			{
				GuessNumber = currentGuessNumber,
				GuessString = ans,
				Pattern = SecondGuess.ToPatternString(currentGuessPattern),
				PreviousGuessId = previousGuessId
			});

			await dbContext.SaveChangesAsync();


			if (SecondGuess.ToPatternString(currentGuessPattern) != GUESSED_PATTERN)
			{
				var possiblePatterns = _helper.GetAllPatternsForAWord(ans, currentWords);
				foreach (var pattern in possiblePatterns)
				{
					var wordsNow = Buggy.PossibleWays(ans, pattern, currentWords);

					if (wordsNow.Count == 0 ||
						SecondGuess.ToPatternString(pattern) == Helper.GUESSED_STRING)
					{
						continue;
					}

					await CalculateGuesses(currentGuessNumber + 1, pattern, wordsNow,
						previousGuess.Entity.Id, dbContext);
				}
			}
		});
	}

	private string CalculateEntropy(List<string> words)
	{
		var totalWords = words.Count;
		var possibleGuesses = new Dictionary<string, double>();

		foreach (var word in _wordsStrings)
		{
			double entropy = 0;
			var possiblePatterns = _helper.GetAllPatternsForAWord(word, words);
			foreach (var pattern in possiblePatterns)
			{
				var wordsNow = Buggy.PossibleWays(word, pattern, words);
				var numberOfPossibleWays = wordsNow.Count;

				if (numberOfPossibleWays == 0)
				{
					continue;
				}

				var probability = (double)numberOfPossibleWays / totalWords;

				var addValue = probability * Math.Log2(probability);

				entropy -= addValue;
			}

			possibleGuesses[word] = entropy;
		}

		var answer = possibleGuesses.First();

		foreach (var possibleGuess in possibleGuesses)
		{
			if (possibleGuess.Value > answer.Value)
			{
				answer = possibleGuess;
			}
			else if (Math.Abs(possibleGuess.Value - answer.Value) < 0.001)
			{
				if (words.Contains(possibleGuess.Key) && !words.Contains(answer.Key))
				{
					answer = possibleGuess;
				}
			}
		}

		return answer.Key;
	}
}

