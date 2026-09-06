using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCleanRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url_icono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Auditoria_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    entidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    entidad_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    detalle_json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usuarioid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria_log", x => x.id);
                    table.ForeignKey(
                        name: "FK_Auditoria_log_Usuario_Usuarioid",
                        column: x => x.Usuarioid,
                        principalTable: "Usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Billetera",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    saldo_total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    saldo_retenido = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    saldo_disponible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billetera", x => x.id);
                    table.ForeignKey(
                        name: "FK_Billetera_Usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subasta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vendedor_id = table.Column<int>(type: "int", nullable: false),
                    categoria_id = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url_imagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio_base = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    incremento_minimo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Vendedorid = table.Column<int>(type: "int", nullable: true),
                    Categoriaid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subasta", x => x.id);
                    table.ForeignKey(
                        name: "FK_Subasta_Categoria_Categoriaid",
                        column: x => x.Categoriaid,
                        principalTable: "Categoria",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Subasta_Usuario_Vendedorid",
                        column: x => x.Vendedorid,
                        principalTable: "Usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Puja",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subasta_id = table.Column<int>(type: "int", nullable: false),
                    comprador_id = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    fecha_puja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subastaid = table.Column<int>(type: "int", nullable: true),
                    Compradorid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puja", x => x.id);
                    table.ForeignKey(
                        name: "FK_Puja_Subasta_Subastaid",
                        column: x => x.Subastaid,
                        principalTable: "Subasta",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Puja_Usuario_Compradorid",
                        column: x => x.Compradorid,
                        principalTable: "Usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Transaccion_Ledgers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    billetera_id = table.Column<int>(type: "int", nullable: false),
                    subasta_id = table.Column<int>(type: "int", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subastaid = table.Column<int>(type: "int", nullable: true),
                    Billeteraid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion_Ledgers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transaccion_Ledgers_Billetera_Billeteraid",
                        column: x => x.Billeteraid,
                        principalTable: "Billetera",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transaccion_Ledgers_Subasta_Subastaid",
                        column: x => x.Subastaid,
                        principalTable: "Subasta",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "id", "nombre", "url_icono" },
                values: new object[,]
                {
                    { 1, "Tecnología", "" },
                    { 2, "Coleccionables", "" },
                    { 3, "Indumentaria", "" },
                    { 4, "Vehículos", "" }
                });

            migrationBuilder.InsertData(
                table: "Puja",
                columns: new[] { "id", "Compradorid", "Subastaid", "comprador_id", "fecha_puja", "monto", "subasta_id" },
                values: new object[,]
                {
                    { 1, null, null, 4, new DateTime(2026, 9, 6, 12, 32, 50, 274, DateTimeKind.Local).AddTicks(5922), 500m, 1 },
                    { 3, null, null, 2, new DateTime(2026, 9, 6, 12, 37, 50, 274, DateTimeKind.Local).AddTicks(5942), 45000m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Subasta",
                columns: new[] { "id", "Categoriaid", "Vendedorid", "categoria_id", "descripcion", "estado", "fecha_fin", "fecha_inicio", "incremento_minimo", "precio_base", "titulo", "url_imagen", "vendedor_id" },
                values: new object[,]
                {
                    { 1, null, null, 1, "Celular Samsung", "ACTIVA", new DateTime(2026, 9, 6, 12, 57, 50, 274, DateTimeKind.Local).AddTicks(5970), new DateTime(2026, 9, 6, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5969), 1000m, 30000m, "Celular", "", 1 },
                    { 2, null, null, 2, "Figura de acción de colección", "ACTIVA", new DateTime(2026, 9, 6, 12, 28, 50, 274, DateTimeKind.Local).AddTicks(5973), new DateTime(2026, 9, 6, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5972), 500m, 5000m, "Figura de acción", "", 1 },
                    { 3, null, null, 3, "Camiseta de algodón", "PROGRAMADA", new DateTime(2026, 9, 6, 12, 28, 50, 274, DateTimeKind.Local).AddTicks(5978), new DateTime(2026, 9, 7, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5975), 1000m, 2000m, "Camiseta", "", 1 },
                    { 4, null, null, 4, "Bicicleta de montaña", "ACTIVA", new DateTime(2026, 9, 2, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5981), new DateTime(2026, 8, 30, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5980), 1000m, 10000m, "Bicicleta", "", 1 },
                    { 5, null, null, 1, "Tablet de última generación", "ACTIVA", new DateTime(2026, 9, 5, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5984), new DateTime(2026, 8, 30, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(5983), 1500m, 25000m, "Tablet", "", 1 }
                });

            migrationBuilder.InsertData(
                table: "Transaccion_Ledgers",
                columns: new[] { "id", "Billeteraid", "Subastaid", "billetera_id", "fecha", "monto", "subasta_id", "tipo" },
                values: new object[,]
                {
                    { 1, null, null, 2, new DateTime(2026, 8, 27, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(6007), 150000m, 0, "DEPOSITO" },
                    { 2, null, null, 3, new DateTime(2026, 8, 27, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(6010), 200000m, 0, "DEPOSITO" },
                    { 3, null, null, 4, new DateTime(2026, 8, 27, 12, 27, 50, 274, DateTimeKind.Local).AddTicks(6011), 500m, 0, "DEPOSITO" },
                    { 4, null, null, 2, new DateTime(2026, 9, 6, 12, 37, 50, 274, DateTimeKind.Local).AddTicks(6013), 45000m, 1, "RETENCION" }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "id", "email", "nombre", "password_hash", "rol" },
                values: new object[,]
                {
                    { 1, "vendedor@test.com", "Creador de publicaciones", "", "" },
                    { 2, "comprador1@test.com", "Postor líder", "", "" },
                    { 3, "comprador2@test.com", "Postor habilitado", "", "" },
                    { 4, "sinfondos@test.com", "Sin saldo", "", "" }
                });

            migrationBuilder.InsertData(
                table: "Billetera",
                columns: new[] { "id", "saldo_disponible", "saldo_retenido", "saldo_total", "usuario_id" },
                values: new object[,]
                {
                    { 1, 0m, 0m, 0m, 1 },
                    { 2, 105000m, 45000m, 150000m, 2 },
                    { 3, 200000m, 0m, 200000m, 3 },
                    { 4, 0m, 0m, 500m, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_log_Usuarioid",
                table: "Auditoria_log",
                column: "Usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_Billetera_usuario_id",
                table: "Billetera",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Puja_Compradorid",
                table: "Puja",
                column: "Compradorid");

            migrationBuilder.CreateIndex(
                name: "IX_Puja_Subastaid",
                table: "Puja",
                column: "Subastaid");

            migrationBuilder.CreateIndex(
                name: "IX_Subasta_Categoriaid",
                table: "Subasta",
                column: "Categoriaid");

            migrationBuilder.CreateIndex(
                name: "IX_Subasta_Vendedorid",
                table: "Subasta",
                column: "Vendedorid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Ledgers_Billeteraid",
                table: "Transaccion_Ledgers",
                column: "Billeteraid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Ledgers_Subastaid",
                table: "Transaccion_Ledgers",
                column: "Subastaid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria_log");

            migrationBuilder.DropTable(
                name: "Puja");

            migrationBuilder.DropTable(
                name: "Transaccion_Ledgers");

            migrationBuilder.DropTable(
                name: "Billetera");

            migrationBuilder.DropTable(
                name: "Subasta");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
