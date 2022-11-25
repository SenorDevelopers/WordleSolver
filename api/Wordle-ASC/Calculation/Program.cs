using Calculation.Services;
using Database;

namespace Calculation;

public class Program
{
	public static async Task FullFlow()
	{
		//await Helper.AddWordsToDatabaseAsync("cuvinte_wordle.txt");

		var uoW = new UoW();
		var calculateOpeners = new CalculateOpeners();

		//await calculateOpeners.CalculateAsync();
		await calculateOpeners.CalculateAsync(1, true);

		var calculateGuesses = new CalculateGuesses();

		var opener = await WordService.GetWordWithMaxEntropyAsync(uoW);

		await calculateGuesses.StartCalculateGuessesAsync(opener.Text);
	}

	public static async Task Main()
	{
		Console.WriteLine("Hello World!");

		//await Task.Delay(200);
		await FullFlow();
	}
}

