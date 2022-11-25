using Database;
using Database.Entities;
using Database.Models;

namespace Calculation.Services;

public class GuessesService
{
	public static async Task<GuessModel> AddGuessAsync(Guess guess, UoW uoW)
	{
		var addedGuess = await uoW.AddAsync(guess);

		await uoW.SaveChangesAsync();

		return GuessModel.FromEntity(addedGuess.Entity);
	}
}

