using Database;
using Model.Models;

namespace Calculation;

public class DepthCalculation
{
	public static async Task<double> DepthCalculationForAWordAsync(string word, List<string> words, UoW uoW)
	{
		double average = 0;
		var totalWords = words.Count;
		var possiblePatterns = Helper.GetAllPatternsForAWord(word, words);
		var entropyCalculator = new EntropyCalculation();

		foreach (var pattern in possiblePatterns)
		{
			var wordsNow = Helper.PossibleWays(word, pattern, words);
			var numberOfPossibleWays = wordsNow.Count;

			if (numberOfPossibleWays == 0)
			{
				continue;
			}

			var maxEntropyWord = await entropyCalculator.GetMaximumEntropyWordInListAsync(wordsNow, false, uoW);

			var probability = (double)numberOfPossibleWays / totalWords;

			average += maxEntropyWord.Value * probability;
		}

		return average;
	}

	public static async Task<Dictionary<string, double>> DepthCalculationForWordsAsync(Dictionary<string, double> entropies, List<string> words, UoW uoW)
	{
		var entropiesList = entropies.ToList();
		entropiesList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

		var numberOfElementsInDepth = Math.Min(Constants.DEPTH_SEARCH, entropiesList.Count);

		entropiesList = entropiesList.Take(numberOfElementsInDepth).ToList();

		foreach (var result in entropiesList)
		{
			var average = await DepthCalculationForAWordAsync(result.Key, words, uoW);

			entropies[result.Key] += average;
		}

		return entropies;
	}
}

