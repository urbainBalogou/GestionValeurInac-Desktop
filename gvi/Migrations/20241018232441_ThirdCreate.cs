using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gvi.Migrations
{
    public partial class ThirdCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeValeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nature = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ValeurFaciale = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeValeurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Valeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeValeurId = table.Column<int>(type: "int", nullable: false),
                    nombre_de_valeur_par_feuillet_ou_carnet = table.Column<int>(type: "int", nullable: false),
                    nombre_de_feuillets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valeurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Valeurs_TypeValeurs_typeValeurId",
                        column: x => x.typeValeurId,
                        principalTable: "TypeValeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stockage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommuneId = table.Column<int>(type: "int", nullable: false),
                    ValeurId = table.Column<int>(type: "int", nullable: false),
                    QuantiteDisponible = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stockage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stockage_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stockage_Valeurs_ValeurId",
                        column: x => x.ValeurId,
                        principalTable: "Valeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stockage_CommuneId_ValeurId",
                table: "Stockage",
                columns: new[] { "CommuneId", "ValeurId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stockage_ValeurId",
                table: "Stockage",
                column: "ValeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Valeurs_typeValeurId",
                table: "Valeurs",
                column: "typeValeurId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stockage");

            migrationBuilder.DropTable(
                name: "Valeurs");

            migrationBuilder.DropTable(
                name: "TypeValeurs");
        }
    }
}
