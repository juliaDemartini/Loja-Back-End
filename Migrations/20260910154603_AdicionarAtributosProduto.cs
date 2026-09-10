using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarAtributosProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Produtos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estilo",
                table: "Produtos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lavagem",
                table: "Produtos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Provador3dDisponivel",
                table: "Produtos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Produtos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Estilo",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Lavagem",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Provador3dDisponivel",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Produtos");
        }
    }
}
