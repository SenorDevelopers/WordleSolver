using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
	public partial class finalll : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<Guid>(
				name: "PreviousGuessId",
				table: "Guesses",
				type: "uniqueidentifier",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PreviousGuessId",
				table: "Guesses");
		}
	}
}
