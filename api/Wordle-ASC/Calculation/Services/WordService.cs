using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calculation.Services;

public class WordService
{
	public static async Task<Word> GetWordWithMaxEntropyAsync(UoW uoW)
	{
		var words = await uoW.Words!.ToListAsync();

		if (words.Count == 0)
		{
			throw new Exception("Error while getting the words from the database");
		}

		var maxWord = words.First();

		foreach (var word in words)
		{
			if (word.Entropy > maxWord.Entropy)
			{
				maxWord = word;
			}
		}

		return maxWord;
	}

	public static async Task<List<Word>> GetAllWordsAsync(UoW uoW)
	{
		return await uoW.Words!.ToListAsync();
	}

	public static async Task<List<string>> GetAllStringWordsAsync(UoW uoW)
	{
		return await uoW.Words!.Select(word => word.Text).ToListAsync();
	}

	public static List<Word> GetAllWords(UoW uoW)
	{
		return uoW.Words!.ToList();
	}

	public static List<string> GetAllStringWords(UoW uoW)
	{
		return uoW.Words!.Select(word => word.Text).ToList();
	}

	public static async Task<Word?> GetWordByTextAsync(string text, UoW uoW)
	{
		return await uoW.Words.FirstOrDefaultAsync(w => w.Text == text);
	}
}

