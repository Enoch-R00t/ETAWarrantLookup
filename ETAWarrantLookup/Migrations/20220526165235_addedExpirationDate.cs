using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETAWarrantLookup.Migrations
{
    public partial class addedExpirationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentExpirationDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "f7116bf1-2c88-448e-b947-759cbb5b7b4f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "88b2cdfb-b8db-49c1-8c21-d491d21ec378", "AQAAAAEAACcQAAAAEKwRDBHOrH+OpghJPuH8f/hovkyHL2ggJwyX6aXC5Y8lh6N2S0kGDpyy9pAfEys+cQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentExpirationDate",
                table: "Subscriptions");

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
    }
}
