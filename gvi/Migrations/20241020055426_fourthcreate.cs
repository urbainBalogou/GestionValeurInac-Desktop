using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gvi.Migrations
{
    public partial class fourthcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Demandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommuneId = table.Column<int>(type: "int", nullable: false),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstTraitee = table.Column<bool>(type: "bit", nullable: false),
                    EnCours = table.Column<bool>(type: "bit", nullable: false),
                    EstSortie = table.Column<bool>(type: "bit", nullable: false),
                    DateRetrait = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demandes_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommuneId = table.Column<int>(type: "int", nullable: false),
                    DateEntree = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrees_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemandeValeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    ValeurId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    MontantTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandeValeurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandeValeurs_Demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "Demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemandeValeurs_Valeurs_ValeurId",
                        column: x => x.ValeurId,
                        principalTable: "Valeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sorties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommuneId = table.Column<int>(type: "int", nullable: false),
                    EmployeId = table.Column<int>(type: "int", nullable: false),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    DateSortie = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sorties_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sorties_Demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "Demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sorties_Employes_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntreeValeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntreeId = table.Column<int>(type: "int", nullable: false),
                    ValeurInactiveId = table.Column<int>(type: "int", nullable: false),
                    ValeurId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    MontantTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntreeValeurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntreeValeurs_Entrees_EntreeId",
                        column: x => x.EntreeId,
                        principalTable: "Entrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntreeValeurs_Valeurs_ValeurId",
                        column: x => x.ValeurId,
                        principalTable: "Valeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SortieValeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SortieId = table.Column<int>(type: "int", nullable: false),
                    ValeurId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    MontantTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortieValeurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SortieValeurs_Sorties_SortieId",
                        column: x => x.SortieId,
                        principalTable: "Sorties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SortieValeurs_Valeurs_ValeurId",
                        column: x => x.ValeurId,
                        principalTable: "Valeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demandes_CommuneId",
                table: "Demandes",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandeValeurs_DemandeId",
                table: "DemandeValeurs",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandeValeurs_ValeurId",
                table: "DemandeValeurs",
                column: "ValeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrees_CommuneId",
                table: "Entrees",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_EntreeValeurs_EntreeId",
                table: "EntreeValeurs",
                column: "EntreeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntreeValeurs_ValeurId",
                table: "EntreeValeurs",
                column: "ValeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorties_CommuneId",
                table: "Sorties",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorties_DemandeId",
                table: "Sorties",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorties_EmployeId",
                table: "Sorties",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_SortieValeurs_SortieId",
                table: "SortieValeurs",
                column: "SortieId");

            migrationBuilder.CreateIndex(
                name: "IX_SortieValeurs_ValeurId",
                table: "SortieValeurs",
                column: "ValeurId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandeValeurs");

            migrationBuilder.DropTable(
                name: "EntreeValeurs");

            migrationBuilder.DropTable(
                name: "SortieValeurs");

            migrationBuilder.DropTable(
                name: "Entrees");

            migrationBuilder.DropTable(
                name: "Sorties");

            migrationBuilder.DropTable(
                name: "Demandes");
        }
    }
}
