using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turismo_Bova.Migrations
{
    /// <inheritdoc />
    public partial class AsignacionEmpleado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId",
                table: "Asignacion_Ruta_Vehiculo_Horario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_Ruta_Vehiculo_Horario_EmpleadoId",
                table: "Asignacion_Ruta_Vehiculo_Horario",
                column: "EmpleadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Ruta_Vehiculo_Horario_Empleado_EmpleadoId",
                table: "Asignacion_Ruta_Vehiculo_Horario",
                column: "EmpleadoId",
                principalTable: "Empleado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Ruta_Vehiculo_Horario_Empleado_EmpleadoId",
                table: "Asignacion_Ruta_Vehiculo_Horario");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_Ruta_Vehiculo_Horario_EmpleadoId",
                table: "Asignacion_Ruta_Vehiculo_Horario");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "Asignacion_Ruta_Vehiculo_Horario");
        }
    }
}
