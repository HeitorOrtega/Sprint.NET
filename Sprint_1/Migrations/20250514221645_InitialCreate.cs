using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sprint_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chaveiro_Motos_MotoId",
                table: "Chaveiro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chaveiro",
                table: "Chaveiro");

            migrationBuilder.RenameTable(
                name: "Chaveiro",
                newName: "CHAVEIRO");

            migrationBuilder.RenameIndex(
                name: "IX_Chaveiro_MotoId",
                table: "CHAVEIRO",
                newName: "IX_CHAVEIRO_MotoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CHAVEIRO",
                table: "CHAVEIRO",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CHAVEIRO_Motos_MotoId",
                table: "CHAVEIRO",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CHAVEIRO_Motos_MotoId",
                table: "CHAVEIRO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CHAVEIRO",
                table: "CHAVEIRO");

            migrationBuilder.RenameTable(
                name: "CHAVEIRO",
                newName: "Chaveiro");

            migrationBuilder.RenameIndex(
                name: "IX_CHAVEIRO_MotoId",
                table: "Chaveiro",
                newName: "IX_Chaveiro_MotoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chaveiro",
                table: "Chaveiro",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chaveiro_Motos_MotoId",
                table: "Chaveiro",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
