using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class GuessesService : IGuessesService
{
	private readonly UoW _uoW;
	private readonly ILogger<GuessesService> _logger;

	public GuessesService(UoW uoW, ILogger<GuessesService> logger)
	{
		_uoW = uoW;
		_logger = logger;
	}

	public async Task<GuessModel> GetNextGuessByPatternAndPreviousGuessIdAsync(string pattern, Guid previousGuessId)
	{
		var guess = await _uoW.Guesses.FirstOrDefaultAsync(g => g.Pattern == pattern &&
																	 g.PreviousGuessId == previousGuessId);

		_logger.LogInformation($"Got the {guess?.GuessString} with id: {guess?.Id} as the next guess for pattern: {pattern} and previous guess id: {previousGuessId}");

		if (guess == null)
		{
			return new GuessModel();
		}

		return GuessModel.FromEntity(guess);
	}

	public async Task<GuessModel> GetNextGuessByPattern(string pattern)
	{
		var guess = await _uoW.Guesses.FirstOrDefaultAsync(g => g.Pattern == pattern &&
																g.PreviousGuessId == null);

		_logger.LogInformation($"Got the {guess?.GuessString} with id: {guess?.Id} as the next guess for pattern: {pattern} and null previous guess id");

		if (guess == null)
		{
			return new GuessModel();
		}

		return GuessModel.FromEntity(guess);
	}
}
