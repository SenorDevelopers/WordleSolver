using Database;
using Microsoft.EntityFrameworkCore;

namespace Calculator;

public class CalculateOpeners
{
	private const int MAX_THREADS = 10;

	public static async Task StartCalculateAsync()
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.ToListAsync();

		var tasks = new List<Task>();

		await Task.Run(async () =>
		{
			var i = 0;
			foreach (var word in words)
			{
				tasks.Add(CalculateForOneWordAsync(word.Text));
				i++;
				if (i < MAX_THREADS)
				{
					continue;
				}

				await Task.WhenAll(tasks);
				tasks.Clear();
				i = 0;
			}
		});

		await Task.WhenAll(tasks);
	}

	public static async Task CalculateForXWords(int numberOfWords)
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.OrderByDescending(w => w.Entropy)
													.Take(numberOfWords)
													.ToListAsync();

		var tasks = new List<Task>();

		await Task.Run(async () =>
		{
			var i = 0;
			foreach (var word in words)
			{
				tasks.Add(CalculateForOneWordAsync(word.Text));
				i++;
				if (i < MAX_THREADS)
				{
					continue;
				}

				await Task.WhenAll(tasks);
				tasks.Clear();
				i = 0;
			}
		});

		await Task.WhenAll(tasks);
	}

	public static async Task CalculateForOneWordAsync(string word)
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.ToListAsync();
		var wordsText = await dbContext.Words!.Select(w => w.Text).ToListAsync();
		var totalWords = words.Count;
		var helper = new Helper();
		double average = 0;

		double entropy = 0;
		var possiblePatterns = helper.GetAllPatternsForAWord(word, wordsText);

		foreach (var pattern in possiblePatterns)
		{
			var wordsNow = Helper.PossibleWays(word, pattern, wordsText);
			var numberOfPossibleWays = wordsNow.Count;

			if (numberOfPossibleWays == 0)
			{
				continue;
			}

			var ans = await CalculatedInList(wordsNow, dbContext);

			var probability = (double)numberOfPossibleWays / totalWords;

			var addValue = probability * Math.Log2(probability);

			entropy -= addValue;

			average += ans * probability;
		}

		var wordDb = await dbContext.Words.FirstOrDefaultAsync(w => w.Text == word);

		wordDb!.Entropy = (decimal)entropy + (decimal)average;

		await dbContext.SaveChangesAsync();
	}


	public static async Task<double> CalculateForOneWord(string word)
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.ToListAsync();
		var wordsText = await dbContext.Words!.Select(w => w.Text).ToListAsync();
		var totalWords = words.Count;
		var helper = new Helper();


		double entropy = 0;
		var possiblePatterns = helper.GetAllPatternsForAWord(word, wordsText);

		foreach (var pattern in possiblePatterns)
		{
			var wordsNow = Helper.PossibleWays(word, pattern, wordsText);
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


	public static async Task<double> CalculatedInList(List<string> currentWords, UoW dbContext)
	{
		var words = await dbContext.Words!.Select(w => w.Text).ToListAsync();
		var totalWords = currentWords.Count;
		var helper = new Helper();

		double ans = 0;

		foreach (var word in words)
		{
			double entropy = 0;
			var possiblePatterns = helper.GetAllPatternsForAWord(word, currentWords);
			foreach (var pattern in possiblePatterns)
			{
				var wordsNow = Helper.PossibleWays(word, pattern, currentWords);
				var numberOfPossibleWays = wordsNow.Count;

				if (numberOfPossibleWays == 0)
				{
					continue;
				}

				var probability = (double)numberOfPossibleWays / totalWords;

				var addValue = probability * Math.Log2(probability);

				entropy -= addValue;
			}
			ans = Math.Max(entropy, ans);
		}


		return ans;
	}
}

