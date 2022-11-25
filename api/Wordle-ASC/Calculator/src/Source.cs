using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calculator;

public class Source
{
	public static void Shuffle<T>(ref IList<T> list)
	{
		var rng = new Random();

		var n = list.Count;
		while (n > 1)
		{
			n--;
			var k = rng.Next(n + 1);
			(list[k], list[n]) = (list[n], list[k]);
		}
	}

	public static async Task<Word> GetMaxEntropy()
	{
		var dbContext = new UoW();
		var words = await dbContext.Words!.OrderByDescending(w => w.Entropy).ToListAsync();
		var maxW = words[0];

		foreach (var word in words)
		{
			if (word.Entropy > maxW.Entropy)
			{
				maxW = word;
			}
		}

		return maxW;
	}

	public static async Task<List<Word>> GetWords()
	{
		var dbContext = new UoW();

		return await dbContext.Words!.ToListAsync();
	}


	private static async Task Run2Words()
	{
		var words = new List<string>()
		{
			"SURIA",
			"CURAI",
			"TAREI",
			"SERAI",
			"CAREI",
			"SERIA"
		};

		var dbContex = new UoW();

		foreach (var word in words)
		{
			await dbContex.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Guesses]");
			var buggyGussesInDepth = new BuggyGussesInDepth();
			await buggyGussesInDepth.StartCalculateGuesses(word);

			var average = await BuggyTest.TestAllWords(word);
			Console.WriteLine(word + "  " + average);

			string backupname = word + "-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".bak";
			const string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'MajesticDb-Ali-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
			await dbContex.Database.ExecuteSqlRawAsync(string.Format(sqlCommand, "WordleASC_Buggy", backupname));
		}
	}

	private static async Task Main()
	{
		var buggy = new Buggy();
		await buggy.StartCalculateAsync();

		//var buggyGusses = new BuggyGusses();
		//await buggyGusses.StartCalculateGuesses("TUREI");

		//var average = await BuggyTest.TestAllWords("TUREI");
		//Console.WriteLine(average);
	}


}
