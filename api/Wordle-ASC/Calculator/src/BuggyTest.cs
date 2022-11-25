using Database;
using Microsoft.EntityFrameworkCore;

namespace Calculator;

public class BuggyTest
{
	public static async Task<double> TestAllWords(string opener)
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.Select(w => w.Text).ToListAsync();

		int total = 0;
		int maxx = 0;

		foreach (var word in words)
		{
			var ans = await TestAgainstAnAnswer(word, opener);
			total += ans;

			maxx = Math.Max(maxx, ans);
		}

		var nrWords = words.Count;
		var medie = (double)total / nrWords;

		return medie;
	}

	public static async Task<int> TestAgainstAnAnswer(string answer, string opener)
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.Select(w => w.Text).ToListAsync();

		var resultedPattern = Buggy.TestWord(answer, opener);
		words = Buggy.PossibleWays(opener, resultedPattern, words);

		var guessed = false;
		var numberOfTried = 1;

		if (SecondGuess.ToPatternString(resultedPattern) == Helper.GUESSED_STRING)
		{
			return numberOfTried;
		}

		var guess = await dbContext.Guesses!.FirstOrDefaultAsync(g =>
			g.GuessNumber == numberOfTried && g.Pattern == SecondGuess.ToPatternString(resultedPattern));

		//Console.WriteLine(guess!.GuessString);

		numberOfTried++;

		while (!guessed)
		{
			var nexGuess = await dbContext.Guesses!
					.FirstOrDefaultAsync(g => g.Pattern == SecondGuess.ToPatternString(resultedPattern)
											  && g.GuessNumber == numberOfTried &&
											  g.PreviousGuessId == guess!.Id);

			//Console.WriteLine(nexGuess!.GuessString);

			resultedPattern = Buggy.TestWord(answer, nexGuess!.GuessString!);

			words = Buggy.PossibleWays(nexGuess!.GuessString!, resultedPattern, words);

			if (SecondGuess.ToPatternString(resultedPattern) == Helper.GUESSED_STRING)
			{
				break;
			}

			numberOfTried++;
			guess = nexGuess;
		}

		//Console.WriteLine(numberOfTried);
		return numberOfTried;
	}

}

