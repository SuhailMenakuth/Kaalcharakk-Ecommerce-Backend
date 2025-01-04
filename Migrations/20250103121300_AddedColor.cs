using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kaalcharakk.Migrations
{
    /// <inheritdoc />
    public partial class AddedColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 3, 12, 12, 56, 201, DateTimeKind.Utc).AddTicks(4568),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 3, 10, 53, 53, 250, DateTimeKind.Utc).AddTicks(623));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 3, 10, 53, 53, 250, DateTimeKind.Utc).AddTicks(623),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 3, 12, 12, 56, 201, DateTimeKind.Utc).AddTicks(4568));
        }
    }
}
