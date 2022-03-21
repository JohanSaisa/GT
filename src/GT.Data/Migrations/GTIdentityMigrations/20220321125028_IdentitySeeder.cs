using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GT.Data.Migrations.GTIdentityMigrations
{
    public partial class IdentitySeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15767280-4770-481e-83e4-e80bd3f1c99c", "9011e89a-d640-4eb2-9132-555676bec2f7", "GTUser", "GTUser" },
                    { "3b40801b-ec95-47ed-9a36-17f67576e131", "c11076d7-3272-4823-86c7-55624604f426", "GTAdministrator", "GTAdministrator" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0780eaf3-6d18-4566-b737-bcf58c5c863c", 0, "a379d2fc-ee14-4b18-9230-4c003128af2e", "GTadmin@admin.com", false, null, null, false, null, null, null, "AQAAAAEAACcQAAAAEPGxIfB1maxXNY8E2wyVDqy/rhexaahthwP/WK9qGFc57xDus5J4vGIEBkavdxLj5Q==", null, false, "e469238a-3df3-40c3-8465-f560fa5c15e8", false, "GTadmin@admin.com" },
                    { "336672c1-58bb-4277-9ffa-20967eeb36e5", 0, "e7de8177-29a0-4b26-ae5b-d8ee20d42f3d", "GTuser@user.com", false, null, null, false, null, null, null, "AQAAAAEAACcQAAAAEFf24eQAk7f4sZQE0AjvLFdDJyXeXHSPAjjt7bPhoifmLmAhlYZ1NO6pwicrYn1uFQ==", null, false, "963e123e-59d7-434e-90f3-8ba1d6d46065", false, "GTuser@user.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3b40801b-ec95-47ed-9a36-17f67576e131", "0780eaf3-6d18-4566-b737-bcf58c5c863c" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "15767280-4770-481e-83e4-e80bd3f1c99c", "336672c1-58bb-4277-9ffa-20967eeb36e5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3b40801b-ec95-47ed-9a36-17f67576e131", "0780eaf3-6d18-4566-b737-bcf58c5c863c" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "15767280-4770-481e-83e4-e80bd3f1c99c", "336672c1-58bb-4277-9ffa-20967eeb36e5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15767280-4770-481e-83e4-e80bd3f1c99c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b40801b-ec95-47ed-9a36-17f67576e131");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0780eaf3-6d18-4566-b737-bcf58c5c863c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "336672c1-58bb-4277-9ffa-20967eeb36e5");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
