using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class Verzija1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaninarskaDrustva",
                columns: table => new
                {
                    IDPlaninarskogDrustva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImePlaninarskogDrustva = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Drzava = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    BrojClana = table.Column<int>(type: "int", nullable: false),
                    GodisnjaClanarina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaninarskaDrustva", x => x.IDPlaninarskogDrustva);
                });

            migrationBuilder.CreateTable(
                name: "Planine",
                columns: table => new
                {
                    IDPlanine = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImePlanine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Drzava = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaksimalnaVisina = table.Column<int>(type: "int", nullable: false),
                    ImeNajvisegVrha = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TezinaPlanine = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planine", x => x.IDPlanine);
                });

            migrationBuilder.CreateTable(
                name: "Planinar",
                columns: table => new
                {
                    IDPlaninara = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    JMBG = table.Column<int>(type: "int", nullable: false),
                    Spremnost = table.Column<int>(type: "int", nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Drzava = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IDPlaninarskogDrustva1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planinar", x => x.IDPlaninara);
                    table.ForeignKey(
                        name: "FK_Planinar_PlaninarskaDrustva_IDPlaninarskogDrustva1",
                        column: x => x.IDPlaninarskogDrustva1,
                        principalTable: "PlaninarskaDrustva",
                        principalColumn: "IDPlaninarskogDrustva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dogadjaji",
                columns: table => new
                {
                    IDDogadjaja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeDogadjaja = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImeVrhaDogadjaja = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DatumOdrzavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlaninaIDPlanine = table.Column<int>(type: "int", nullable: false),
                    TezinaUspona = table.Column<int>(type: "int", nullable: false),
                    BrojUcesnika = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogadjaji", x => x.IDDogadjaja);
                    table.ForeignKey(
                        name: "FK_Dogadjaji_Planine_PlaninaIDPlanine",
                        column: x => x.PlaninaIDPlanine,
                        principalTable: "Planine",
                        principalColumn: "IDPlanine",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaninariDogadjaji",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaninariIDPlaninara = table.Column<int>(type: "int", nullable: true),
                    DogadjajiIDDogadjaja = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaninariDogadjaji", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlaninariDogadjaji_Dogadjaji_DogadjajiIDDogadjaja",
                        column: x => x.DogadjajiIDDogadjaja,
                        principalTable: "Dogadjaji",
                        principalColumn: "IDDogadjaja",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaninariDogadjaji_Planinar_PlaninariIDPlaninara",
                        column: x => x.PlaninariIDPlaninara,
                        principalTable: "Planinar",
                        principalColumn: "IDPlaninara",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaninarskDrustvaDogadjaji",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaninarskaDrustvaIDPlaninarskogDrustva = table.Column<int>(type: "int", nullable: true),
                    DogadjajiIDDogadjaja = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaninarskDrustvaDogadjaji", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlaninarskDrustvaDogadjaji_Dogadjaji_DogadjajiIDDogadjaja",
                        column: x => x.DogadjajiIDDogadjaja,
                        principalTable: "Dogadjaji",
                        principalColumn: "IDDogadjaja",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaninarskDrustvaDogadjaji_PlaninarskaDrustva_PlaninarskaDrustvaIDPlaninarskogDrustva",
                        column: x => x.PlaninarskaDrustvaIDPlaninarskogDrustva,
                        principalTable: "PlaninarskaDrustva",
                        principalColumn: "IDPlaninarskogDrustva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dogadjaji_PlaninaIDPlanine",
                table: "Dogadjaji",
                column: "PlaninaIDPlanine");

            migrationBuilder.CreateIndex(
                name: "IX_Planinar_IDPlaninarskogDrustva1",
                table: "Planinar",
                column: "IDPlaninarskogDrustva1");

            migrationBuilder.CreateIndex(
                name: "IX_PlaninariDogadjaji_DogadjajiIDDogadjaja",
                table: "PlaninariDogadjaji",
                column: "DogadjajiIDDogadjaja");

            migrationBuilder.CreateIndex(
                name: "IX_PlaninariDogadjaji_PlaninariIDPlaninara",
                table: "PlaninariDogadjaji",
                column: "PlaninariIDPlaninara");

            migrationBuilder.CreateIndex(
                name: "IX_PlaninarskDrustvaDogadjaji_DogadjajiIDDogadjaja",
                table: "PlaninarskDrustvaDogadjaji",
                column: "DogadjajiIDDogadjaja");

            migrationBuilder.CreateIndex(
                name: "IX_PlaninarskDrustvaDogadjaji_PlaninarskaDrustvaIDPlaninarskogDrustva",
                table: "PlaninarskDrustvaDogadjaji",
                column: "PlaninarskaDrustvaIDPlaninarskogDrustva");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaninariDogadjaji");

            migrationBuilder.DropTable(
                name: "PlaninarskDrustvaDogadjaji");

            migrationBuilder.DropTable(
                name: "Planinar");

            migrationBuilder.DropTable(
                name: "Dogadjaji");

            migrationBuilder.DropTable(
                name: "PlaninarskaDrustva");

            migrationBuilder.DropTable(
                name: "Planine");
        }
    }
}
