using Database.Entities;

namespace Database.Models;

public class GuessModel
{
	public Guid Id { get; set; }
	public string GuessString { get; set; } = null!;

	public static GuessModel FromEntity(Guess guess)
	{
		return new GuessModel()
		{
			Id = guess.Id,
			GuessString = guess.GuessString!
		};
	}
}
