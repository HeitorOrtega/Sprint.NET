using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sprint_1.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Cor = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Placa = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataFabricacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PATIO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Chaveiro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Dispositivo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    MotoId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chaveiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chaveiro_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FUNCIONARIO",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    RG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCIONARIO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FUNCIONARIO_PATIO_PatioId",
                        column: x => x.PatioId,
                        principalTable: "PATIO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MotoPatio",
                columns: table => new
                {
                    MotosId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PatiosId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotoPatio", x => new { x.MotosId, x.PatiosId });
                    table.ForeignKey(
                        name: "FK_MotoPatio_Motos_MotosId",
                        column: x => x.MotosId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotoPatio_PATIO_PatiosId",
                        column: x => x.PatiosId,
                        principalTable: "PATIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chaveiro_MotoId",
                table: "Chaveiro",
                column: "MotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FUNCIONARIO_PatioId",
                table: "FUNCIONARIO",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_MotoPatio_PatiosId",
                table: "MotoPatio",
                column: "PatiosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chaveiro");

            migrationBuilder.DropTable(
                name: "FUNCIONARIO");

            migrationBuilder.DropTable(
                name: "MotoPatio");

            migrationBuilder.DropTable(
                name: "Motos");

            migrationBuilder.DropTable(
                name: "PATIO");
        }
    }
}
