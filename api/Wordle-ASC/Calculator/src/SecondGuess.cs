using Database.Enums;

namespace Calculator;

public class SecondGuess

{
	public static async Task Start(string firstGuess)
	{
		//var dbContext = new UoW();
		//var allWords = await dbContext.Words!.Select(w => w.Text).ToListAsync();
		//var patterns = Helper.GetAllPatterns();

		//foreach (var pattern in patterns)
		//{
		//	var words = Helper.PossibleWays(firstGuess, pattern, allWords);
		//	if (words.Count == 0)
		//	{
		//		continue;
		//	}

		//	var ans = CalculateEntropy(words, patterns);

		//	dbContext.SecondGuesses!.Add(new Database.Entities.SecondGuess
		//	{
		//		Word = ans,
		//		Pattern = Helper.IsGuessed(pattern) ? Helper.GUESSED_STRING : ToPatternString(pattern),
		//	});
		//}

		//await dbContext.SaveChangesAsync();
	}

	public static string ToPatternString(List<MatchWays> pattern)
	{
		string ans = "";
		foreach (var way in pattern)
		{
			var ok = (int)way;
			var okk = ok.ToString();
			ans += okk;
		}

		return ans;
	}

	private static string CalculateEntropy(List<string> words, in List<List<MatchWays>> patterns)
	{
		var totalWords = words.Count;

		var possibleGuesses = new Dictionary<string, double>();

		foreach (var word in words)
		{
			double entropy = 0;
			foreach (var pattern in patterns)
			{
				double numberOfPossibleWays = Helper.NumberOfPossibleWays(word, pattern, words);

				var probability = numberOfPossibleWays / totalWords;

				if (probability == 0)
				{
					continue;
				}

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
		}

		return answer.Key;
	}
}

