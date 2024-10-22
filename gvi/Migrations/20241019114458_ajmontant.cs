using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gvi.Migrations
{
    public partial class ajmontant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "montant",
                table: "Valeurs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "montant",
                table: "Valeurs");
        }
    }
}
