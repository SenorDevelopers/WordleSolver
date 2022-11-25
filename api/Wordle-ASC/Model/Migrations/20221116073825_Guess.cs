using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class Guess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuessString = table.Column<string>(type: "NVARCHAR(7)", nullable: true),
                    GuessNumber = table.Column<int>(type: "int", nullable: true),
                    PreviousPattern = table.Column<string>(type: "NVARCHAR(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guesses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guesses");
        }
    }
}
