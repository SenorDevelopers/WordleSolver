using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class _232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Words",
                table: "Words");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guesses",
                table: "Guesses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Guesses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Words",
                table: "Words",
                column: "Id2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guesses",
                table: "Guesses",
                column: "Id2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Words",
                table: "Words");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guesses",
                table: "Guesses");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Guesses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Words",
                table: "Words",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guesses",
                table: "Guesses",
                column: "Id");
        }
    }
}
