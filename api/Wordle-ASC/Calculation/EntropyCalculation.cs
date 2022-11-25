using Calculation.Services;
using Database;
using Model.Models;

namespace Calculation;

public class EntropyCalculation
{
	private readonly List<string> _wordsStrings;

	public EntropyCalculation()
	{
		var uoW = new UoW();
		_wordsStrings = WordService.GetAllStringWords(uoW);
	}

	public Dictionary<string, double> GetEntropyForWords(List<string> words)
	{
		var possibleGuesses = new Dictionary<string, double>();

		foreach (var word in _wordsStrings)
		{
			var entropy = EntropyCalculatorForWord(word, true, words);

			possibleGuesses[word] = entropy;
		}

		return possibleGuesses;
	}

	public double EntropyCalculatorForWord(string word, bool calculateInList = false, List<string>? words = null)
	{
		if (calculateInList && words?.Count == 0)
		{
			throw new Exception("Invalid parameters");
		}

		if (!calculateInList)
		{
			words = _wordsStrings;
		}

		var entropy = CalculateForOneWord(word, words);

		return entropy;
	}

	private static double CalculateForOneWord(string word, List<string> wordsString)
	{
		var totalWords = wordsString.Count;

		double entropy = 0;
		var possiblePatterns = Helper.GetAllPatternsForAWord(word, wordsString);

		foreach (var pattern in possiblePatterns)
		{
			var wordsNow = Helper.PossibleWays(word, pattern, wordsString);
			var numberOfPossibleWays = wordsNow.Count;

			if (numberOfPossibleWays == 0)
			{
				continue;
			}

			var probability = (double)numberOfPossibleWays / totalWords;

			var addValue = probability * Math.Log2(probability);

			entropy -= addValue;
		}

		return entropy;
	}


	public async Task<Dictionary<string, double>> DepthCalculationAsync(Dictionary<string, double> entropies, List<string> words, UoW uoW)
	{
		var resultList = entropies.ToList();
		resultList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

		var elements = Math.Min(Constants.DEPTH_SEARCH, resultList.Count);

		resultList = resultList.Take(elements).ToList();

		foreach (var result in resultList)
		{
			var average = await DepthCalculationForAWordAsync(result.Key, words, uoW);

			entropies[result.Key] += average;
		}

		return entropies;
	}

	public async Task<double> DepthCalculationForAWordAsync(string word, List<string> words, UoW uoW)
	{
		double average = 0;
		var totalWords = words.Count;

		var possiblePatterns = Helper.GetAllPatternsForAWord(word, words);

		foreach (var pattern in possiblePatterns)
		{
			var wordsNow = Helper.PossibleWays(word, pattern, words);
			var numberOfPossibleWays = wordsNow.Count;

			if (numberOfPossibleWays == 0)
			{
				continue;
			}

			var ans = await GetMaximumEntropyWord(wordsNow, false, uoW);

			var probability = (double)numberOfPossibleWays / totalWords;

			average += ans.Value * probability;
		}

		return average;
	}

	public async Task<KeyValuePair<string, double>> GetMaximumEntropyWord(List<string> words, bool depthCalculation, UoW dbContext)
	{
		var entropies = GetEntropyForWords(words);

		if (depthCalculation)
		{
			entropies = await DepthCalculationAsync(entropies, words, dbContext);
		}

		var bestWord = GetMaxEntropyWordInDictionary(entropies, words);

		return bestWord;
	}

	public static KeyValuePair<string, double> GetMaxEntropyWordInDictionary(Dictionary<string, double> entropies, List<string> words)
	{
		var bestWord = entropies.First();

		foreach (var possibleGuess in entropies)
		{
			if (possibleGuess.Value > bestWord.Value)
			{
				bestWord = possibleGuess;
			}
			else if (Math.Abs(possibleGuess.Value - bestWord.Value) < Constants.MAX_PRECISION_ERROR)
			{
				if (words.Contains(possibleGuess.Key) && !words.Contains(bestWord.Key))
				{
					bestWord = possibleGuess;
				}
			}
		}

		return bestWord;
	}
}

