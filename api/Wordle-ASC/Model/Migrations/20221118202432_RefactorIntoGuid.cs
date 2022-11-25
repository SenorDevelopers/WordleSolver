using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class RefactorIntoGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecondGuesses");

            migrationBuilder.AddColumn<Guid>(
                name: "Id2",
                table: "Words",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id2",
                table: "Guesses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Guesses");

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
    }
}
