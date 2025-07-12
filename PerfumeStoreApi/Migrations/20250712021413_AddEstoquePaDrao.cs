using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PerfumeStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEstoquePaDrao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Estoque",
                columns: new[] { "Id", "DataCriacao", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Produtos novos", "Estoque de Novos" },
                    { 2, new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Produtos seminovos ou usados", "Estoque de Usados" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Estoque",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Estoque",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
