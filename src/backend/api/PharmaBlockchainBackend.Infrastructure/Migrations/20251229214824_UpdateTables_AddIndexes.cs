using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaBlockchainBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables_AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProtocolStep_CmoId",
                table: "ProtocolStep");

            migrationBuilder.DropIndex(
                name: "IX_Pallet_Code",
                table: "Pallet");

            migrationBuilder.DropIndex(
                name: "IX_Package_Code",
                table: "Package");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Pallet_Code",
                table: "Pallet",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Package_Code",
                table: "Package",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolStep_CmoId_ProtocolType",
                table: "ProtocolStep",
                columns: new[] { "CmoId", "ProtocolType" });

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolStep_ProtocolType_PalletId",
                table: "ProtocolStep",
                columns: new[] { "ProtocolType", "PalletId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProtocolStep_CmoId_ProtocolType",
                table: "ProtocolStep");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolStep_ProtocolType_PalletId",
                table: "ProtocolStep");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Pallet_Code",
                table: "Pallet");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Package_Code",
                table: "Package");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolStep_CmoId",
                table: "ProtocolStep",
                column: "CmoId");

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
    }
}
