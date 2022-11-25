using Calculation.Services;
using Database;
using Database.Entities;
using Model.Models;

namespace Calculation;

public class CalculateGuesses
{
	private readonly List<string> _wordsStrings;
	private readonly EntropyCalculation _entropyCalculation;

	public CalculateGuesses()
	{
		var uoW = new UoW();
		_wordsStrings = WordService.GetAllStringWords(uoW);
		_entropyCalculation = new EntropyCalculation();
	}

	public async Task StartCalculateGuessesAsync(string opener)
	{
		const int CURRENT_GUESS_NUMBER = 1;

		var possiblePatterns = Helper.GetAllPatternsForAWord(opener, _wordsStrings);

		var tasks = new List<Task>();

		var currentNumberOfThreads = 0;

		await Task.Run(async () =>
		{
			var dbContext = new UoW();
			foreach (var pattern in possiblePatterns)
			{
				var previousGuess = await dbContext.Guesses.AddAsync(new Guess
				{
					GuessNumber = CURRENT_GUESS_NUMBER,
					GuessString = opener,
					Pattern = pattern.ToString()
				});

				await dbContext.SaveChangesAsync();

				var wordsNow = Helper.PossibleWays(opener, pattern, _wordsStrings);

				if (wordsNow.Count == 0 || pattern.IsGuessed())
				{
					continue;
				}

				currentNumberOfThreads++;

				var threadContext = new UoW();
				tasks.Add(Guesses(CURRENT_GUESS_NUMBER + 1, pattern, wordsNow, previousGuess.Entity.Id, threadContext));

				if (currentNumberOfThreads < Constants.MAX_THREADS)
				{
					continue;
				}

				await Task.WhenAll(tasks);
				tasks.Clear();
				currentNumberOfThreads = 0;
			}
		});
		await Task.WhenAll(tasks);
	}

	private async Task Guesses(int currentGuessNumber, Pattern currentGuessPattern,
		List<string> currentWords, Guid previousGuessId, UoW dbContext)
	{
		await Task.Run(async () =>
		{
			var ans = await _entropyCalculation.GetMaximumEntropyWord(currentWords, currentGuessNumber == 2, dbContext);

			var previousGuess = await dbContext.Guesses.AddAsync(new Guess
			{
				GuessNumber = currentGuessNumber,
				GuessString = ans.Key,
				Pattern = currentGuessPattern.ToString(),
				PreviousGuessId = previousGuessId
			});

			await dbContext.SaveChangesAsync();

			if (currentGuessPattern.IsGuessed())
			{
				return;
			}

			var possiblePatterns = Helper.GetAllPatternsForAWord(ans.Key, currentWords);
			foreach (var pattern in possiblePatterns)
			{
				var wordsNow = Helper.PossibleWays(ans.Key, pattern, currentWords);

				if (wordsNow.Count == 0 || pattern.IsGuessed())
				{
					continue;
				}

				await Guesses(currentGuessNumber + 1, pattern, wordsNow,
					previousGuess.Entity.Id, dbContext);
			}
		});
	}
}

