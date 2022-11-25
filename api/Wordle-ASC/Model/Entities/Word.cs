using Database.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class Word : BaseEntity
{
	[Column(TypeName = "NVARCHAR(7)")]
	[Required]
	public string Text { get; set; } = string.Empty;

	[Column(TypeName = "decimal(14,8)")]
	public decimal Entropy { get; set; }
}
