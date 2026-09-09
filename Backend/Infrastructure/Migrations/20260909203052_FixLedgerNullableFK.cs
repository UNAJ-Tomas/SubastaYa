using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLedgerNullableFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaccion_Ledgers_Subasta_Subastaid",
                table: "Transaccion_Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Transaccion_Ledgers_Subastaid",
                table: "Transaccion_Ledgers");

            migrationBuilder.DropColumn(
                name: "Subastaid",
                table: "Transaccion_Ledgers");

            migrationBuilder.AlterColumn<int>(
                name: "subasta_id",
                table: "Transaccion_Ledgers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Billetera",
                keyColumn: "id",
                keyValue: 1,
                column: "saldo_total",
                value: 100000m);

            migrationBuilder.UpdateData(
                table: "Puja",
                keyColumn: "id",
                keyValue: 1,
                column: "fecha_puja",
                value: new DateTime(2026, 9, 9, 17, 35, 52, 564, DateTimeKind.Local).AddTicks(304));

            migrationBuilder.UpdateData(
                table: "Puja",
                keyColumn: "id",
                keyValue: 3,
                column: "fecha_puja",
                value: new DateTime(2026, 9, 9, 17, 40, 52, 564, DateTimeKind.Local).AddTicks(323));

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 9, 18, 0, 52, 564, DateTimeKind.Local).AddTicks(351), new DateTime(2026, 9, 9, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(350) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 9, 17, 31, 52, 564, DateTimeKind.Local).AddTicks(354), new DateTime(2026, 9, 9, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(354) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 9, 17, 31, 52, 564, DateTimeKind.Local).AddTicks(360), new DateTime(2026, 9, 10, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(357) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 5, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(363), new DateTime(2026, 9, 2, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(362) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 8, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(366), new DateTime(2026, 9, 2, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(365) });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "fecha", "subasta_id" },
                values: new object[] { new DateTime(2026, 8, 30, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(395), null });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "fecha", "subasta_id" },
                values: new object[] { new DateTime(2026, 8, 30, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(434), null });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "fecha", "subasta_id" },
                values: new object[] { new DateTime(2026, 8, 30, 17, 30, 52, 564, DateTimeKind.Local).AddTicks(435), null });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 4,
                column: "fecha",
                value: new DateTime(2026, 9, 9, 17, 40, 52, 564, DateTimeKind.Local).AddTicks(437));

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Ledgers_billetera_id",
                table: "Transaccion_Ledgers",
                column: "billetera_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Ledgers_subasta_id",
                table: "Transaccion_Ledgers",
                column: "subasta_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaccion_Ledgers_Billetera_billetera_id",
                table: "Transaccion_Ledgers",
                column: "billetera_id",
                principalTable: "Billetera",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaccion_Ledgers_Subasta_subasta_id",
                table: "Transaccion_Ledgers",
                column: "subasta_id",
                principalTable: "Subasta",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaccion_Ledgers_Billetera_billetera_id",
                table: "Transaccion_Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaccion_Ledgers_Subasta_subasta_id",
                table: "Transaccion_Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Transaccion_Ledgers_billetera_id",
                table: "Transaccion_Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Transaccion_Ledgers_subasta_id",
                table: "Transaccion_Ledgers");

            migrationBuilder.AlterColumn<int>(
                name: "subasta_id",
                table: "Transaccion_Ledgers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Subastaid",
                table: "Transaccion_Ledgers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Billetera",
                keyColumn: "id",
                keyValue: 1,
                column: "saldo_total",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Puja",
                keyColumn: "id",
                keyValue: 1,
                column: "fecha_puja",
                value: new DateTime(2026, 9, 6, 12, 32, 50, 274, DateTimeKind.Local).AddTicks(5922));

            migrationBuilder.UpdateData(
                table: "Puja",
                keyColumn: "id",
                keyValue: 3,
                column: "fecha_puja",
                value: new DateTime(2026, 9, 6, 12, 37, 50, 274, DateTimeKind.Local).AddTicks(5942));

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 6, 12, 57, 50, 274, DateTimeKind.Local).AddTicks(5970), new DateTime(2026, 9, 6, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5969) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 6, 12, 28, 50, 274, DateTimeKind.Local).AddTicks(5973), new DateTime(2026, 9, 6, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5972) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 6, 12, 28, 50, 274, DateTimeKind.Local).AddTicks(5978), new DateTime(2026, 9, 7, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5975) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 2, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5981), new DateTime(2026, 8, 30, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5980) });

            migrationBuilder.UpdateData(
                table: "Subasta",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "fecha_fin", "fecha_inicio" },
                values: new object[] { new DateTime(2026, 9, 5, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5984), new DateTime(2026, 8, 30, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5983) });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "Subastaid", "fecha", "subasta_id" },
                values: new object[] { null, new DateTime(2026, 8, 27, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(6007), 0 });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "Subastaid", "fecha", "subasta_id" },
                values: new object[] { null, new DateTime(2026, 8, 27, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(6010), 0 });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "Subastaid", "fecha", "subasta_id" },
                values: new object[] { null, new DateTime(2026, 8, 27, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(6011), 0 });

            migrationBuilder.UpdateData(
                table: "Transaccion_Ledgers",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "Subastaid", "fecha" },
                values: new object[] { null, new DateTime(2026, 9, 6, 12, 37, 50, 274, DateTimeKind.Local).AddTicks(6013) });

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Ledgers_Subastaid",
                table: "Transaccion_Ledgers",
                column: "Subastaid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaccion_Ledgers_Subasta_Subastaid",
                table: "Transaccion_Ledgers",
                column: "Subastaid",
                principalTable: "Subasta",
                principalColumn: "id");
        }
    }
}
