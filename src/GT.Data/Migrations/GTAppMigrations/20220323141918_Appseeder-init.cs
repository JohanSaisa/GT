using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GT.Data.Migrations.GTAppMigrations
{
    public partial class Appseederinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ZipCode = table.Column<string>(type: "varchar(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressCompany",
                columns: table => new
                {
                    AddressesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompaniesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressCompany", x => new { x.AddressesId, x.CompaniesId });
                    table.ForeignKey(
                        name: "FK_AddressCompany_Addresses_AddressesId",
                        column: x => x.AddressesId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressCompany_Companies_CompaniesId",
                        column: x => x.CompaniesId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListingTitle = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    EmployerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SalaryMin = table.Column<int>(type: "int", nullable: true),
                    SalaryMax = table.Column<int>(type: "int", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FTE = table.Column<bool>(type: "bit", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Listings_Companies_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ListingInquiries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageTitle = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    MessageBody = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    LinkedInLink = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ListingIDId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingInquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingInquiries_Listings_ListingIDId",
                        column: x => x.ListingIDId,
                        principalTable: "Listings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressCompany_CompaniesId",
                table: "AddressCompany",
                column: "CompaniesId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingInquiries_ListingIDId",
                table: "ListingInquiries",
                column: "ListingIDId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_AddressId",
                table: "Listings",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_EmployerId",
                table: "Listings",
                column: "EmployerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressCompany");

            migrationBuilder.DropTable(
                name: "ListingInquiries");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
