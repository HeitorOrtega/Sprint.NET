using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sprint_1.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        // 🔹 Constantes para evitar repetição
        private const string TipoNumber = "NUMBER(19)";
        private const string TipoTexto = "NVARCHAR2(2000)";
        private const string TipoData = "TIMESTAMP(7)";

        private const string OracleIdentity = "Oracle:Identity";
        private const string OracleIdentityConfig = "START WITH 1 INCREMENT BY 1";

        private const string TabelaMotos = "Motos";
        private const string TabelaPatio = "PATIO";
        private const string TabelaChaveiro = "Chaveiro";
        private const string TabelaChaveiroUpper = "CHAVEIRO"; // caso você precise no plural/upper

        private const string TabelaFuncionario = "FUNCIONARIO";
        private const string TabelaMotoPatio = "MotoPatio";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: TabelaMotos,
                columns: table => new
                {
                    Id = table.Column<long>(type: TipoNumber, nullable: false)
                        .Annotation(OracleIdentity, OracleIdentityConfig),
                    Cor = table.Column<string>(type: TipoTexto, nullable: false),
                    Placa = table.Column<string>(type: TipoTexto, nullable: false),
                    DataFabricacao = table.Column<DateTime>(type: TipoData, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: TabelaPatio,
                columns: table => new
                {
                    ID = table.Column<long>(type: TipoNumber, nullable: false)
                        .Annotation(OracleIdentity, OracleIdentityConfig)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: TabelaChaveiro,
                columns: table => new
                {
                    Id = table.Column<long>(type: TipoNumber, nullable: false)
                        .Annotation(OracleIdentity, OracleIdentityConfig),
                    Dispositivo = table.Column<string>(type: TipoTexto, nullable: false),
                    MotoId = table.Column<long>(type: TipoNumber, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chaveiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chaveiro_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: TabelaMotos,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: TabelaFuncionario,
                columns: table => new
                {
                    Id = table.Column<long>(type: TipoNumber, nullable: false)
                        .Annotation(OracleIdentity, OracleIdentityConfig),
                    NOME = table.Column<string>(type: TipoTexto, nullable: false),
                    CPF = table.Column<string>(type: TipoTexto, nullable: false),
                    EMAIL = table.Column<string>(type: TipoTexto, nullable: false),
                    RG = table.Column<string>(type: TipoTexto, nullable: false),
                    TELEFONE = table.Column<string>(type: TipoTexto, nullable: false),
                    PatioId = table.Column<long>(type: TipoNumber, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCIONARIO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FUNCIONARIO_PATIO_PatioId",
                        column: x => x.PatioId,
                        principalTable: TabelaPatio,
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: TabelaMotoPatio,
                columns: table => new
                {
                    MotosId = table.Column<long>(type: TipoNumber, nullable: false),
                    PatiosId = table.Column<long>(type: TipoNumber, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotoPatio", x => new { x.MotosId, x.PatiosId });
                    table.ForeignKey(
                        name: "FK_MotoPatio_Motos_MotosId",
                        column: x => x.MotosId,
                        principalTable: TabelaMotos,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotoPatio_PATIO_PatiosId",
                        column: x => x.PatiosId,
                        principalTable: TabelaPatio,
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chaveiro_MotoId",
                table: TabelaChaveiro,
                column: "MotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FUNCIONARIO_PatioId",
                table: TabelaFuncionario,
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_MotoPatio_PatiosId",
                table: TabelaMotoPatio,
                column: "PatiosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: TabelaChaveiro);
            migrationBuilder.DropTable(name: TabelaFuncionario);
            migrationBuilder.DropTable(name: TabelaMotoPatio);
            migrationBuilder.DropTable(name: TabelaMotos);
            migrationBuilder.DropTable(name: TabelaPatio);
        }
    }
}
