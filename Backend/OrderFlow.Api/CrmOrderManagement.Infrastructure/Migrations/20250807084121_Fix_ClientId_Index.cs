using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmOrderManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_ClientId_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ClientId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 7, 8, 41, 21, 28, DateTimeKind.Utc).AddTicks(4333),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 8, 7, 8, 36, 18, 897, DateTimeKind.Utc).AddTicks(6453));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 8, 41, 21, 30, DateTimeKind.Utc).AddTicks(4182));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 8, 41, 21, 30, DateTimeKind.Utc).AddTicks(4185));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ClientId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 7, 8, 36, 18, 897, DateTimeKind.Utc).AddTicks(6453),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 8, 7, 8, 41, 21, 28, DateTimeKind.Utc).AddTicks(4333));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 8, 36, 18, 899, DateTimeKind.Utc).AddTicks(4018));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 8, 36, 18, 899, DateTimeKind.Utc).AddTicks(4022));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId",
                unique: true);
        }
    }
}
