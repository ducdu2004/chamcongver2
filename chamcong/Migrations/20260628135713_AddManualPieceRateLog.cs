using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chamcong.Migrations
{
    /// <inheritdoc />
    public partial class AddManualPieceRateLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductionLogs_BundleId",
                table: "ProductionLogs");

            migrationBuilder.AlterColumn<int>(
                name: "BundleId",
                table: "ProductionLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GarmentPartId",
                table: "ProductionLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductionLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SizeOrTable",
                table: "ProductionLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "ProductionLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLogs_BundleId",
                table: "ProductionLogs",
                column: "BundleId",
                unique: true,
                filter: "[BundleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLogs_GarmentPartId",
                table: "ProductionLogs",
                column: "GarmentPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLogs_ProductId",
                table: "ProductionLogs",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionLogs_GarmentParts_GarmentPartId",
                table: "ProductionLogs",
                column: "GarmentPartId",
                principalTable: "GarmentParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionLogs_Products_ProductId",
                table: "ProductionLogs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionLogs_GarmentParts_GarmentPartId",
                table: "ProductionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionLogs_Products_ProductId",
                table: "ProductionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProductionLogs_BundleId",
                table: "ProductionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProductionLogs_GarmentPartId",
                table: "ProductionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProductionLogs_ProductId",
                table: "ProductionLogs");

            migrationBuilder.DropColumn(
                name: "GarmentPartId",
                table: "ProductionLogs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductionLogs");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductionLogs");

            migrationBuilder.DropColumn(
                name: "SizeOrTable",
                table: "ProductionLogs");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "ProductionLogs");

            migrationBuilder.AlterColumn<int>(
                name: "BundleId",
                table: "ProductionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLogs_BundleId",
                table: "ProductionLogs",
                column: "BundleId",
                unique: true);
        }
    }
}
