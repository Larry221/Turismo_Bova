using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turismo_Bova.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Producto_Producto_ProductoId",
                table: "Pedido_Producto");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Producto_Producto_ProductoId",
                table: "Pedido_Producto",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Producto_Producto_ProductoId",
                table: "Pedido_Producto");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Producto_Producto_ProductoId",
                table: "Pedido_Producto",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
