using Database.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class Guess : BaseEntity
{
	[Column(TypeName = "NVARCHAR(7)")]
	public string? GuessString { get; set; }
	public int? GuessNumber { get; set; }

	[Column(TypeName = "NVARCHAR(7)")]
	public string? Pattern { get; set; }

	public Guid? PreviousGuessId { get; set; }
}

