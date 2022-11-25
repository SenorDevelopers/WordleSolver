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
			var uoW = new UoW();

			foreach (var pattern in possiblePatterns)
			{
				var previousGuess = await GuessesService.AddGuessAsync(new Guess
				{
					GuessNumber = CURRENT_GUESS_NUMBER,
					GuessString = opener,
					Pattern = pattern.ToString()
				}, uoW);

				var wordsNow = Helper.PossibleWays(opener, pattern, _wordsStrings);

				if (wordsNow.Count == 0 || pattern.IsGuessed())
				{
					continue;
				}

				currentNumberOfThreads++;

				var threadUoW = new UoW();
				tasks.Add(Guesses(CURRENT_GUESS_NUMBER + 1, pattern, wordsNow, previousGuess.Id, threadUoW));

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
		List<string> currentWords, Guid previousGuessId, UoW uoW)
	{
		await Task.Run(async () =>
		{
			var maxEntropyAnswer = await _entropyCalculation.GetMaximumEntropyWordInListAsync(currentWords, currentGuessNumber == Constants.MAX_IN_DEPTH_GUESSES, uoW);

			var previousGuess = await GuessesService.AddGuessAsync(new Guess
			{
				GuessNumber = currentGuessNumber,
				GuessString = maxEntropyAnswer.Key,
				Pattern = currentGuessPattern.ToString(),
				PreviousGuessId = previousGuessId
			}, uoW);

			if (currentGuessPattern.IsGuessed())
			{
				return;
			}

			var possiblePatterns = Helper.GetAllPatternsForAWord(maxEntropyAnswer.Key, currentWords);
			foreach (var pattern in possiblePatterns)
			{
				var wordsNow = Helper.PossibleWays(maxEntropyAnswer.Key, pattern, currentWords);

				if (wordsNow.Count == 0 || pattern.IsGuessed())
				{
					continue;
				}

				await Guesses(currentGuessNumber + 1, pattern, wordsNow,
					previousGuess.Id, uoW);
			}
		});
	}
}

