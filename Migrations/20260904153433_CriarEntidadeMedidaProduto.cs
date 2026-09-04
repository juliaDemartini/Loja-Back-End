using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LojaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class CriarEntidadeMedidaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedidasProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProdutoId = table.Column<int>(type: "integer", nullable: false),
                    Tamanho = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BustoMin = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    BustoMax = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    CinturaMin = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    CinturaMax = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    QuadrilMin = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    QuadrilMax = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    Comprimento = table.Column<decimal>(type: "numeric(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedidasProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedidasProdutos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedidasProdutos_ProdutoId",
                table: "MedidasProdutos",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedidasProdutos");
        }
    }
}
