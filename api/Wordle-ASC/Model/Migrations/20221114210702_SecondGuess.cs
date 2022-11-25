using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class SecondGuess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecondGuesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pattern = table.Column<string>(type: "NVARCHAR(7)", nullable: false),
                    Word = table.Column<string>(type: "NVARCHAR(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondGuesses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecondGuesses");
        }
    }
}
