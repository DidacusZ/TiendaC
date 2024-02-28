using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gestion_logica_negocio");

            migrationBuilder.EnsureSchema(
                name: "gestion_usuarios");

            migrationBuilder.CreateTable(
                name: "productos",
                schema: "gestion_logica_negocio",
                columns: table => new
                {
                    id_producto = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_producto = table.Column<string>(type: "text", nullable: false),
                    desc_producto = table.Column<string>(type: "text", nullable: false),
                    precio_producto = table.Column<double>(type: "double precision", nullable: false),
                    cant_producto = table.Column<int>(type: "integer", nullable: false),
                    img_producto = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.id_producto);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "gestion_usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email_usuario = table.Column<string>(type: "text", nullable: false),
                    mvl_usuario = table.Column<string>(type: "text", nullable: false),
                    rol_usuario = table.Column<string>(type: "text", nullable: true),
                    clv_usuario = table.Column<string>(type: "text", nullable: false),
                    img_usuario = table.Column<string>(type: "text", nullable: true),
                    token_recuperacion = table.Column<string>(type: "text", nullable: true),
                    fch_expiracion_token = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cuenta_confirmada = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "carritos",
                schema: "gestion_logica_negocio",
                columns: table => new
                {
                    id_carrito = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false),
                    cant_carrito = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carritos", x => x.id_carrito);
                    table.ForeignKey(
                        name: "FK_carritos_usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "gestion_usuarios",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compras",
                schema: "gestion_logica_negocio",
                columns: table => new
                {
                    id_compra = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false),
                    cant_compra = table.Column<int>(type: "integer", nullable: false),
                    fch_compra = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compras", x => x.id_compra);
                    table.ForeignKey(
                        name: "FK_compras_usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "gestion_usuarios",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productos_carritos",
                schema: "gestion_logica_negocio",
                columns: table => new
                {
                    id_carrito = table.Column<long>(type: "bigint", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_carritos", x => new { x.id_carrito, x.id_producto });
                    table.ForeignKey(
                        name: "FK_productos_carritos_carritos_id_carrito",
                        column: x => x.id_carrito,
                        principalSchema: "gestion_logica_negocio",
                        principalTable: "carritos",
                        principalColumn: "id_carrito",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productos_carritos_productos_id_producto",
                        column: x => x.id_producto,
                        principalSchema: "gestion_logica_negocio",
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compras_productos",
                schema: "gestion_logica_negocio",
                columns: table => new
                {
                    id_producto = table.Column<long>(type: "bigint", nullable: false),
                    id_compra = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compras_productos", x => new { x.id_producto, x.id_compra });
                    table.ForeignKey(
                        name: "FK_compras_productos_compras_id_compra",
                        column: x => x.id_compra,
                        principalSchema: "gestion_logica_negocio",
                        principalTable: "compras",
                        principalColumn: "id_compra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_compras_productos_productos_id_producto",
                        column: x => x.id_producto,
                        principalSchema: "gestion_logica_negocio",
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_carritos_id_usuario",
                schema: "gestion_logica_negocio",
                table: "carritos",
                column: "id_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_compras_id_usuario",
                schema: "gestion_logica_negocio",
                table: "compras",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_compras_productos_id_compra",
                schema: "gestion_logica_negocio",
                table: "compras_productos",
                column: "id_compra");

            migrationBuilder.CreateIndex(
                name: "IX_productos_carritos_id_producto",
                schema: "gestion_logica_negocio",
                table: "productos_carritos",
                column: "id_producto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "compras_productos",
                schema: "gestion_logica_negocio");

            migrationBuilder.DropTable(
                name: "productos_carritos",
                schema: "gestion_logica_negocio");

            migrationBuilder.DropTable(
                name: "compras",
                schema: "gestion_logica_negocio");

            migrationBuilder.DropTable(
                name: "carritos",
                schema: "gestion_logica_negocio");

            migrationBuilder.DropTable(
                name: "productos",
                schema: "gestion_logica_negocio");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "gestion_usuarios");
        }
    }
}
