using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GT.Data.Migrations.GTAppMigrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ExperienceLevels_ExperienceLevelId",
                table: "Listings");

            migrationBuilder.AlterColumn<string>(
                name: "ExperienceLevelId",
                table: "Listings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Listings",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ExperienceLevels_ExperienceLevelId",
                table: "Listings",
                column: "ExperienceLevelId",
                principalTable: "ExperienceLevels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ExperienceLevels_ExperienceLevelId",
                table: "Listings");

            migrationBuilder.AlterColumn<string>(
                name: "ExperienceLevelId",
                table: "Listings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Listings",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ExperienceLevels_ExperienceLevelId",
                table: "Listings",
                column: "ExperienceLevelId",
                principalTable: "ExperienceLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
