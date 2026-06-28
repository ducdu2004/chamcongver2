using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chamcong.Migrations
{
    /// <inheritdoc />
    public partial class AddGarmentPartUnitPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DefaultUnitPrice",
                table: "GarmentParts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultUnitPrice",
                table: "GarmentParts");
        }
    }
}
