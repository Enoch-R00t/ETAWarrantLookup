using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETAWarrantLookup.Migrations
{
    public partial class AddSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "b4c63dfc-d8a2-4fca-807b-385a4c47d7bc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ad638e51-9a2c-4281-b31f-fd7e28651f56", "AQAAAAEAACcQAAAAEHV6kDDhtVEouBhNxVB0WJiBWLw9SLkoJCrue2YkVhpdnnvW3utJFMTIDnWDlZ9z8Q==" });
        }
    }
}
