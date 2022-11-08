using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETAWarrantLookup.Migrations
{
    public partial class initialContextCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "785da015-48ef-4450-b684-51702d0e0902");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9ed52625-f950-4904-86aa-a3ee76820e90", "AQAAAAEAACcQAAAAEO0tUPz0LPVRLwszI7glLth7kOu0pum27PRlNMEKmFB+D/Of7Ol2pJDfH4lsR9ozfA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
