using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chamcong.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiveDateToBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveDate",
                table: "Batches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "Batches",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiveDate",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "Batches");
        }
    }
}
