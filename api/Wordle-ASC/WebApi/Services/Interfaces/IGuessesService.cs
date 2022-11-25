using Database.Models;

namespace WebApi.Services.Interfaces;

public interface IGuessesService
{
	Task<GuessModel> GetNextGuessByPatternAndPreviousGuessIdAsync(string pattern, Guid previousGuessId);
	Task<GuessModel> GetNextGuessByPattern(string pattern);
}

