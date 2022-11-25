using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class asda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Words",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Guesses",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Words",
                newName: "Id2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Guesses",
                newName: "Id2");
        }
    }
}
