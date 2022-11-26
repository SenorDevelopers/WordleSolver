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

	public async Task<KeyValuePair<string, double>> GetMaximumEntropyWordInListAsync(List<string> words, bool isDepthCalculationEnabled, UoW dbContext)
	{
		var entropies = GetEntropyForWords(words);

		if (isDepthCalculationEnabled)
		{
			entropies = await DepthCalculation.DepthCalculationForWordsAsync(entropies, words, dbContext);
		}

		var bestWord = GetMaxEntropyWordInDictionary(entropies, words);

		return bestWord;
	}

	public static KeyValuePair<string, double> GetMaxEntropyWordInDictionary(Dictionary<string, double> entropies, List<string> words)
	{
		var maxEntropyWord = entropies.First();

		foreach (var possibleGuess in entropies)
		{
			if (possibleGuess.Value > maxEntropyWord.Value)
			{
				maxEntropyWord = possibleGuess;
			}
			else if (Math.Abs(possibleGuess.Value - maxEntropyWord.Value) < Constants.MAX_PRECISION_ERROR)
			{
				if (words.Contains(possibleGuess.Key) && !words.Contains(maxEntropyWord.Key))
				{
					maxEntropyWord = possibleGuess;
				}
			}
		}

		return maxEntropyWord;
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

		var entropy = CalculateEntropyForWord(word, words!);

		return entropy;
	}

	private static double CalculateEntropyForWord(string word, List<string> wordsString)
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

	private Dictionary<string, double> GetEntropyForWords(List<string> words)
	{
		var possibleGuesses = new Dictionary<string, double>();

		foreach (var word in _wordsStrings)
		{
			var entropy = EntropyCalculatorForWord(word, true, words);

			possibleGuesses[word] = entropy;
		}

		return possibleGuesses;
	}
}

