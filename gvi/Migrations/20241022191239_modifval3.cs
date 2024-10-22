using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gvi.Migrations
{
    public partial class modifval3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stockage_Communes_CommuneId",
                table: "Stockage");

            migrationBuilder.DropForeignKey(
                name: "FK_Stockage_Valeurs_ValeurId",
                table: "Stockage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stockage",
                table: "Stockage");

            migrationBuilder.RenameTable(
                name: "Stockage",
                newName: "Stockages");

            migrationBuilder.RenameIndex(
                name: "IX_Stockage_ValeurId",
                table: "Stockages",
                newName: "IX_Stockages_ValeurId");

            migrationBuilder.RenameIndex(
                name: "IX_Stockage_CommuneId_ValeurId",
                table: "Stockages",
                newName: "IX_Stockages_CommuneId_ValeurId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stockages",
                table: "Stockages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stockages_Communes_CommuneId",
                table: "Stockages",
                column: "CommuneId",
                principalTable: "Communes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stockages_Valeurs_ValeurId",
                table: "Stockages",
                column: "ValeurId",
                principalTable: "Valeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stockages_Communes_CommuneId",
                table: "Stockages");

            migrationBuilder.DropForeignKey(
                name: "FK_Stockages_Valeurs_ValeurId",
                table: "Stockages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stockages",
                table: "Stockages");

            migrationBuilder.RenameTable(
                name: "Stockages",
                newName: "Stockage");

            migrationBuilder.RenameIndex(
                name: "IX_Stockages_ValeurId",
                table: "Stockage",
                newName: "IX_Stockage_ValeurId");

            migrationBuilder.RenameIndex(
                name: "IX_Stockages_CommuneId_ValeurId",
                table: "Stockage",
                newName: "IX_Stockage_CommuneId_ValeurId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stockage",
                table: "Stockage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stockage_Communes_CommuneId",
                table: "Stockage",
                column: "CommuneId",
                principalTable: "Communes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stockage_Valeurs_ValeurId",
                table: "Stockage",
                column: "ValeurId",
                principalTable: "Valeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
