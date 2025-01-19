using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kaalcharakk.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNonNegativeConstrainForProductStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 19, 17, 15, 0, 933, DateTimeKind.Utc).AddTicks(1027),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 9, 4, 15, 58, 382, DateTimeKind.Utc).AddTicks(8540));

            migrationBuilder.AddCheckConstraint(
                name: "Product_Stock_NonNegative",
                table: "Products",
                sql: "[Stock] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Product_Stock_NonNegative",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 9, 4, 15, 58, 382, DateTimeKind.Utc).AddTicks(8540),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 19, 17, 15, 0, 933, DateTimeKind.Utc).AddTicks(1027));
        }
    }
}
