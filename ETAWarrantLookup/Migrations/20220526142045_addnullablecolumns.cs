using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETAWarrantLookup.Migrations
{
    public partial class addnullablecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Subscriptions",
                type: "decimal(6,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "500b5af2-b299-4b67-b5aa-72c6ff204927");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5d4b2137-2371-4b8f-a4cd-5cada29b3a27", "AQAAAAEAACcQAAAAECi2FqYrCbGtSVcqyu5eNDskvf+NcxTLEYWAknVmGZwJ5gi6F+8YFrII/FKYvZqn9g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentAmount",
                table: "Subscriptions",
                type: "decimal(6,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "c65d9500-8037-4bcd-b2ad-e4492c734e4a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "46fd8c61-16aa-4619-b4f8-5541a831b3af", "AQAAAAEAACcQAAAAENE2ia1ydx6ospABSzpxbxYTWc/1qiQQ+0zz+WYP0IyPm/uoi4J4GlZlZTuETs40kA==" });
        }
    }
}
