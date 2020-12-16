using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDeiaLanchesWeb.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idPedido",
                table: "ContasPedidos",
                newName: "IdPedido");

            migrationBuilder.RenameColumn(
                name: "idConta",
                table: "ContasPedidos",
                newName: "IdConta");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ContasPedidos",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPedido",
                table: "Pedidos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPedido",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "IdPedido",
                table: "ContasPedidos",
                newName: "idPedido");

            migrationBuilder.RenameColumn(
                name: "IdConta",
                table: "ContasPedidos",
                newName: "idConta");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ContasPedidos",
                newName: "id");
        }
    }
}
