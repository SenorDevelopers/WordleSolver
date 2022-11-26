using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class OpenerService : IOpenerService
{
	private readonly UoW _uoW;
	private readonly ILogger<OpenerService> _logger;

	public OpenerService(UoW uoW, ILogger<OpenerService> logger)
	{
		_uoW = uoW;
		_logger = logger;
	}

	public async Task<string?> GetMaxEntropyWordAsync()
	{
		var word = await _uoW.Words.OrderByDescending(word => word.Entropy).FirstOrDefaultAsync();

		_logger.LogInformation($"The maximum entropy word read from the database is: {word?.Text}");

		return word?.Text;
	}

	public async Task AddOpener()
	{
		_uoW.Words.Add(new Word()
		{
			Text = "HEY",
			Entropy = 30
		});

		await _uoW.SaveChangesAsync();
	}
}

