using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerfumeStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class AtualizandoBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecoUnitario",
                table: "ItemVendas");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "Pagamentos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "Pagamentos");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoUnitario",
                table: "ItemVendas",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
