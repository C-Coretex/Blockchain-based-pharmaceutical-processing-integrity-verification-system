using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmaBlockchainBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultData_AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Hash",
                table: "ProtocolStep",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.InsertData(
                table: "Cmo",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1d4dba2c-da8e-4518-8bbc-83cd9052e65b"), "MedCore Manufacturing" },
                    { new Guid("d525062a-0c4b-4e1a-a5fe-6e1f567530f3"), "HelixPharm Services" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pallet_Code",
                table: "Pallet",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Package_Code",
                table: "Package",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pallet_Code",
                table: "Pallet");

            migrationBuilder.DropIndex(
                name: "IX_Package_Code",
                table: "Package");

            migrationBuilder.DeleteData(
                table: "Cmo",
                keyColumn: "Id",
                keyValue: new Guid("1d4dba2c-da8e-4518-8bbc-83cd9052e65b"));

            migrationBuilder.DeleteData(
                table: "Cmo",
                keyColumn: "Id",
                keyValue: new Guid("d525062a-0c4b-4e1a-a5fe-6e1f567530f3"));

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "ProtocolStep");
        }
    }
}
