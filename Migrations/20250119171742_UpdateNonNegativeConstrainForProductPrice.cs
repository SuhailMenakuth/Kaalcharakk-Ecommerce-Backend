using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kaalcharakk.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNonNegativeConstrainForProductPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 19, 17, 17, 39, 962, DateTimeKind.Utc).AddTicks(6138),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 19, 17, 15, 0, 933, DateTimeKind.Utc).AddTicks(1027));

            migrationBuilder.AddCheckConstraint(
                name: "Product_Price_Nonnegative",
                table: "Products",
                sql: "[Price] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Product_Price_Nonnegative",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 19, 17, 15, 0, 933, DateTimeKind.Utc).AddTicks(1027),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 19, 17, 17, 39, 962, DateTimeKind.Utc).AddTicks(6138));
        }
    }
}
