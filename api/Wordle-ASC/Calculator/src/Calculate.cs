using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calculator;

public class Calculate
{
	public async Task StartCalculateAsync()
	{
		var dbContext = new UoW();

		var words = await dbContext.Words!.ToListAsync();

		await CalculateEntropy(words, dbContext);
	}

	private static async Task CalculateEntropy(List<Word> words, UoW dbContext)
	{
		var wordsText = await dbContext.Words!.Select(w => w.Text).ToListAsync();
		var totalWords = words.Count;
		var helper = new Helper();

		foreach (var word in words)
		{
			double entropy = 0;
			var possiblePatterns = helper.GetAllPatternsForAWord(word.Text, wordsText);

			foreach (var pattern in possiblePatterns)
			{
				var wordsNow = Helper.PossibleWays(word.Text, pattern, wordsText);
				var numberOfPossibleWays = wordsNow.Count;

				if (numberOfPossibleWays == 0)
				{
					continue;
				}

				var probability = (double)numberOfPossibleWays / totalWords;

				var addValue = probability * Math.Log2(probability);

				entropy -= addValue;
			}
			word.Entropy = (decimal)entropy;
		}
	}


	public static async Task CalculateEntropyForAWord(string word)
	{
		var dbContext = new UoW();
		double totalWords = dbContext.Words!.Count();

		var patterns = Helper.GetAllPatterns();
		var wordsText = await dbContext.Words!.Select(w => w.Text).ToListAsync();

		double entropy = 0;
		foreach (var pattern in patterns)
		{
			double numberOfPossibleWays = Helper.NumberOfPossibleWays(word, pattern, wordsText);

			var probability = numberOfPossibleWays / totalWords;

			if (probability == 0)
			{
				continue;
			}

			var addValue = probability * Math.Log2(probability);

			entropy -= addValue;
		}

		Console.WriteLine(entropy);
	}
}

