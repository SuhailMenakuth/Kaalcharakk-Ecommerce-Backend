using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kaalcharakk.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductModelRemovedOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Offer",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OfferEndingDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OfferStartingDate",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 20, 10, 15, 21, 594, DateTimeKind.Utc).AddTicks(983),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 19, 17, 17, 39, 962, DateTimeKind.Utc).AddTicks(6138));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 19, 17, 17, 39, 962, DateTimeKind.Utc).AddTicks(6138),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 20, 10, 15, 21, 594, DateTimeKind.Utc).AddTicks(983));

            migrationBuilder.AddColumn<decimal>(
                name: "Offer",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "OfferEndingDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OfferStartingDate",
                table: "Products",
                type: "datetime2",
                nullable: true);
        }
    }
}
