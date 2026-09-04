using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRastreioEDataAtualizacaoAoPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoRastreio",
                table: "Pedidos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Pedidos",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoRastreio",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "Pedidos");
        }
    }
}
