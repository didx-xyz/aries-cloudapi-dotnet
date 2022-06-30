using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AriesCloudAPI.EntityFramework.Migrations.PersistedStateDb
{
    public partial class PersistedTenantDb_initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersistedTenants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedTenants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schemas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Aries_SchemaID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Aries_TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Aries_ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FkTenant1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tenant2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Tenants_FkTenant1Id",
                        column: x => x.FkTenant1Id,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantSchemas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkSchemaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Aries_CredentialDefinitionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSchemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantSchemas_Schemas_FkSchemaId",
                        column: x => x.FkSchemaId,
                        principalTable: "Schemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantSchemas_Tenants_FkTenantId",
                        column: x => x.FkTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_FkTenant1Id",
                table: "Connections",
                column: "FkTenant1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSchemas_FkSchemaId",
                table: "TenantSchemas",
                column: "FkSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSchemas_FkTenantId",
                table: "TenantSchemas",
                column: "FkTenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "PersistedTenants");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TenantSchemas");

            migrationBuilder.DropTable(
                name: "Schemas");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
