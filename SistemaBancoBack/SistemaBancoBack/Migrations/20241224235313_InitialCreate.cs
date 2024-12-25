using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaBancoBack.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    CodigoCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroTarjeta = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    LimiteCredito = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SaldoDisponible = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.CodigoCliente);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    CodigoConfiguracion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PorcentajeInteres = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PorcentajeSaldoMinimo = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.CodigoConfiguracion);
                });

            migrationBuilder.CreateTable(
                name: "TipoTransaccion",
                columns: table => new
                {
                    CodigoTipoTransaccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTransaccion", x => x.CodigoTipoTransaccion);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion",
                columns: table => new
                {
                    CodigoTransaccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoCliente = table.Column<int>(type: "int", nullable: false),
                    CodigoTipoTransaccion = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion", x => x.CodigoTransaccion);
                    table.ForeignKey(
                        name: "FK_Transaccion_Cliente_CodigoCliente",
                        column: x => x.CodigoCliente,
                        principalTable: "Cliente",
                        principalColumn: "CodigoCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaccion_TipoTransaccion_CodigoTipoTransaccion",
                        column: x => x.CodigoTipoTransaccion,
                        principalTable: "TipoTransaccion",
                        principalColumn: "CodigoTipoTransaccion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_CodigoCliente",
                table: "Transaccion",
                column: "CodigoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_CodigoTipoTransaccion",
                table: "Transaccion",
                column: "CodigoTipoTransaccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropTable(
                name: "Transaccion");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "TipoTransaccion");
        }
    }
}
