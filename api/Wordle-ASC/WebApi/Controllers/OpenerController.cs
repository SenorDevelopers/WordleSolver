using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OpenerController : ControllerBase
{
	private readonly ILogger<OpenerController> _logger;
	private readonly IOpenerService _openerService;

	public OpenerController(ILogger<OpenerController> logger, IOpenerService openerService)
	{
		_openerService = openerService;
		_logger = logger;
	}

	/// <summary>
	/// Get the opener with the maximum entropy
	/// </summary>
	/// <returns> A 5 letter string containing the opener </returns>
	[HttpGet]
	public async Task<ActionResult<string>> Opener()
	{
		_logger.LogInformation(nameof(Opener) + " was called");

		var word = await _openerService.GetMaxEntropyWordAsync();

		if (word == null)
		{
			_logger.LogError("Error while reading from the database, got a null opener");
			return NotFound();
		}

		_logger.LogInformation($"Successfully returned the opener {word}");
		return Ok(word);
	}
}
