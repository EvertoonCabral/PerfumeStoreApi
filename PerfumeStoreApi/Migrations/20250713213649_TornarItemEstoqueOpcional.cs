using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerfumeStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class TornarItemEstoqueOpcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoEstoque_ItemEstoque_ItemEstoqueId",
                table: "MovimentacaoEstoque");

            migrationBuilder.AlterColumn<int>(
                name: "ItemEstoqueId",
                table: "MovimentacaoEstoque",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoEstoque_ItemEstoque_ItemEstoqueId",
                table: "MovimentacaoEstoque",
                column: "ItemEstoqueId",
                principalTable: "ItemEstoque",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoEstoque_ItemEstoque_ItemEstoqueId",
                table: "MovimentacaoEstoque");

            migrationBuilder.AlterColumn<int>(
                name: "ItemEstoqueId",
                table: "MovimentacaoEstoque",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoEstoque_ItemEstoque_ItemEstoqueId",
                table: "MovimentacaoEstoque",
                column: "ItemEstoqueId",
                principalTable: "ItemEstoque",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
