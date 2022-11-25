using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;


[ApiController]
[Route("[controller]")]
public class GuessesController : ControllerBase
{
	private readonly ILogger<GuessesController> _logger;
	private readonly IGuessesService _guessesService;

	public GuessesController(ILogger<GuessesController> logger, IGuessesService guessesService)
	{
		_logger = logger;
		_guessesService = guessesService;
	}

	/// <summary>
	/// Get the next guess based on the produced pattern of the previous guess and the previous guess Id
	/// </summary>
	/// <param name="pattern"> The resulted pattern of the previous guess</param>
	/// <param name="previousId"> The previous guess Id</param>
	/// <returns> A 5 letter string containing the next guess </returns>
	[HttpGet("next-guess/{pattern}/{previousId:guid}")]
	public async Task<ActionResult<GuessModel>> NextGuess(string pattern, Guid previousId)
	{
		_logger.LogInformation(nameof(NextGuess) + " was called");

		if (!Pattern.IsValidPattern(pattern))
		{
			return BadRequest("Invalid pattern");
		}

		var nextGuess = await _guessesService.GetNextGuessByPatternAndPreviousGuessIdAsync(pattern, previousId);

		if (nextGuess.Id == Guid.Empty)
		{
			return BadRequest("Some/Both of the parameters aren't valid");
		}

		return Ok(nextGuess);
	}

	/// <summary>
	/// Get the second guess based on the produced pattern of the opener
	/// </summary>
	/// <param name="pattern"> The resulted pattern of the opener</param>
	/// <returns> A 5 letter string containing the second guess </returns>
	[HttpGet("next-guess/{pattern}")]
	public async Task<ActionResult<GuessModel>> SecondGuess(string pattern)
	{
		_logger.LogInformation(nameof(NextGuess) + " was called");

		if (!Pattern.IsValidPattern(pattern))
		{
			return BadRequest("Invalid pattern");
		}

		var firstGuess = await _guessesService.GetNextGuessByPattern(pattern);

		if (firstGuess.Id == Guid.Empty)
		{
			return BadRequest("The resulted pattern for the opener isn't valid");
		}

		var secondGuess = await _guessesService.GetNextGuessByPatternAndPreviousGuessIdAsync(pattern, firstGuess.Id);

		if (secondGuess.Id == Guid.Empty)
		{
			return NotFound();
		}

		return Ok(secondGuess);
	}
}
