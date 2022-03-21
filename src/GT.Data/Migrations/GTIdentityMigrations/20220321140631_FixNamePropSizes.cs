using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GT.Data.Migrations.GTIdentityMigrations
{
    public partial class FixNamePropSizes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(70)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(70)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "23a74b02-5c0c-4ab8-9c51-82491a845f9a", "3585ae56-5abf-45f2-9bfc-9b480ed7e417", "GTAdministrator", "GTAdministrator" },
                    { "65abb35c-f44b-4333-af79-5e4b8efc5f63", "d24e2bbe-cd08-4a2b-88ef-8dd54c48240e", "GTUser", "GTUser" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "ac286f22-4370-4d0d-ae2e-8c6f319641ab", 0, "605581e4-9e3d-4463-b508-79c32dba92b9", "GTadmin@admin.com", false, null, null, false, null, null, null, "AQAAAAEAACcQAAAAEFc4GERUFKk/uVLmqHQd21KTFxa8uQxj9L57bR0Z9GbKKgznrpCS/OInBDrjLkMHPg==", null, false, "c8d46e8d-2291-4f07-815d-4a2aae1994e6", false, "GTadmin@admin.com" },
                    { "e80a7444-b165-4f03-b19e-932d78ae92cd", 0, "45c613e1-8f76-4bc5-90e6-2be57d53d13a", "GTuser@user.com", false, null, null, false, null, null, null, "AQAAAAEAACcQAAAAEPQR3Zyv4f9eKlXS4Wl5QF5VOSuRfsGGhwvRqItrK+vAW4J210rJWO1e8icAAIW0DQ==", null, false, "b56ed2f8-4ee9-403c-9f2d-6e5ab5ea76fc", false, "GTuser@user.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "23a74b02-5c0c-4ab8-9c51-82491a845f9a", "ac286f22-4370-4d0d-ae2e-8c6f319641ab" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "65abb35c-f44b-4333-af79-5e4b8efc5f63", "e80a7444-b165-4f03-b19e-932d78ae92cd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "23a74b02-5c0c-4ab8-9c51-82491a845f9a", "ac286f22-4370-4d0d-ae2e-8c6f319641ab" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "65abb35c-f44b-4333-af79-5e4b8efc5f63", "e80a7444-b165-4f03-b19e-932d78ae92cd" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23a74b02-5c0c-4ab8-9c51-82491a845f9a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65abb35c-f44b-4333-af79-5e4b8efc5f63");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ac286f22-4370-4d0d-ae2e-8c6f319641ab");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e80a7444-b165-4f03-b19e-932d78ae92cd");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldNullable: true);

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
    }
}
