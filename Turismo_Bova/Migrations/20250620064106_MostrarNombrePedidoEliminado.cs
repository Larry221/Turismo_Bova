using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turismo_Bova.Migrations
{
    /// <inheritdoc />
    public partial class MostrarNombrePedidoEliminado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "Pedido_Producto",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "Pedido_Producto");
        }
    }
}
