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
		var dbContext = new UoW();
		var words = await WordService.GetAllWordsAsync(dbContext);
		words = words.OrderByDescending(w => w.Entropy).Take(numberOfWords).ToList();

		var entropyCalculator = new EntropyCalculation();

		var tasks = new List<Task>();

		await Task.Run(async () =>
		{
			var currentNumberOfThreads = 0;

			foreach (var word in words)
			{
				tasks.Add(Task.Run(async () =>
				{
					var uoW = new UoW();
					var entropy = entropyCalculator.EntropyCalculatorForWord(word.Text);

					double average = 0;
					if (inDepthCalculation)
					{
						average = await entropyCalculator.DepthCalculationForAWordAsync(word.Text, _wordsStrings, uoW);
					}

					entropy += average;

					var wordDb = await WordService.GetWordByTextAsync(word.Text, uoW);

					wordDb!.Entropy = (decimal)entropy;

					await uoW.SaveChangesAsync();
				}));

				currentNumberOfThreads++;

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
}
