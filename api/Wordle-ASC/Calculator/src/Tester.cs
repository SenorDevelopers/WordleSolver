using Database;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Calculator;

public class Tester
{
	public static async Task Test()
	{

	}

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

		var resultedPattern = TestWord(answer, opener);
		words = Helper.PossibleWays(opener, resultedPattern, words);

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

			resultedPattern = TestWord(answer, nexGuess!.GuessString!);

			words = Helper.PossibleWays(nexGuess!.GuessString!, resultedPattern, words);

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

	public static List<MatchWays> TestWord(string answer, string word)
	{

		const char OK = '0';

		var ans = new List<MatchWays>()
		{
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist
		};

		var copyWord = word.ToCharArray();
		var copyAns = answer.ToCharArray();

		if (answer.Length < 5 || word.Length < 5)
		{
			throw new Exception("Invalid parameters");
		}

		for (var i = 0; i < word.Length; i++)
		{
			if (answer[i] != word[i])
			{
				continue;
			}

			ans[i] = MatchWays.ExistAtTheSamePosition;
			copyWord[i] = OK;
			copyAns[i] = OK;
		}

		for (var i = 0; i < word.Length; i++)
		{
			if (copyAns[i] == OK)
			{
				continue;
			}

			var found = copyWord.FirstOrDefault(chr => chr == copyAns[i]);

			if (char.IsControl(found))
			{
				continue;
			}

			var index = Array.IndexOf(copyWord, answer[i]);
			copyWord[index] = OK;
			ans[index] = MatchWays.ExistInWord;

		}

		return ans;
	}
}