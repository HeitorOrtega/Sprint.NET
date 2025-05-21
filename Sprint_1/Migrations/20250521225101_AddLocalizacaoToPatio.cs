using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sprint_1.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizacaoToPatio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LOCALIZACAO",
                table: "PATIO",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LOCALIZACAO",
                table: "PATIO");
        }
    }
}
