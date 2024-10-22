using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gvi.Migrations
{
    public partial class modifval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntreeValeurs_Valeurs_ValeurId",
                table: "EntreeValeurs");

            migrationBuilder.DropIndex(
                name: "IX_EntreeValeurs_ValeurId",
                table: "EntreeValeurs");

            migrationBuilder.DropColumn(
                name: "ValeurId",
                table: "EntreeValeurs");

            migrationBuilder.CreateIndex(
                name: "IX_EntreeValeurs_ValeurInactiveId",
                table: "EntreeValeurs",
                column: "ValeurInactiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntreeValeurs_Valeurs_ValeurInactiveId",
                table: "EntreeValeurs",
                column: "ValeurInactiveId",
                principalTable: "Valeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntreeValeurs_Valeurs_ValeurInactiveId",
                table: "EntreeValeurs");

            migrationBuilder.DropIndex(
                name: "IX_EntreeValeurs_ValeurInactiveId",
                table: "EntreeValeurs");

            migrationBuilder.AddColumn<int>(
                name: "ValeurId",
                table: "EntreeValeurs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EntreeValeurs_ValeurId",
                table: "EntreeValeurs",
                column: "ValeurId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntreeValeurs_Valeurs_ValeurId",
                table: "EntreeValeurs",
                column: "ValeurId",
                principalTable: "Valeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
