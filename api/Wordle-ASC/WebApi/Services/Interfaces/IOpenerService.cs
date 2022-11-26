namespace WebApi.Services.Interfaces;

public interface IOpenerService
{
	Task<string?> GetMaxEntropyWordAsync();

	Task AddOpener();
}

