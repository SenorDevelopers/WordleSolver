using Calculation.Services;
using Database;
using Model.Models;

namespace Calculation;

public class CalculateOpeners
{
	private readonly List<string> _wordsStrings;

	public CalculateOpeners()
	{
		var uoW = new UoW();
		_wordsStrings = WordService.GetAllStringWords(uoW);
	}

	public async Task CalculateAsync(int numberOfWords = Constants.TOTAL_WORDS_COUNT, bool inDepthCalculation = false)
	{
		var uoW = new UoW();
		var words = await WordService.GetAllWordsAsync(uoW);
		words = words.OrderByDescending(w => w.Entropy).Take(numberOfWords).ToList();

		var entropyCalculator = new EntropyCalculation();

		var tasksList = new List<Task>();

		await Task.Run(async () =>
		{
			var currentNumberOfThreads = 0;

			foreach (var word in words)
			{
				tasksList.Add(Task.Run(async () =>
				{
					var threadUoW = new UoW();
					var entropy = entropyCalculator.EntropyCalculatorForWord(word.Text);

					double average = 0;
					if (inDepthCalculation)
					{
						average = await DepthCalculation.DepthCalculationForAWordAsync(word.Text, _wordsStrings, threadUoW);
					}

					entropy += average;

					var wordEntity = await WordService.GetWordByTextAsync(word.Text, threadUoW);

					wordEntity!.Entropy = (decimal)entropy;

					await threadUoW.SaveChangesAsync();
				}));

				currentNumberOfThreads++;

				if (currentNumberOfThreads < Constants.MAX_THREADS)
				{
					continue;
				}

				await Task.WhenAll(tasksList);
				tasksList.Clear();
				currentNumberOfThreads = 0;
			}
		});

		await Task.WhenAll(tasksList);
	}
}
