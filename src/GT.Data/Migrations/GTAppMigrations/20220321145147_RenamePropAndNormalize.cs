using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GT.Data.Migrations.GTAppMigrations
{
	public partial class RenamePropAndNormalize : Migration
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
					name: "ApplicationUser",
					columns: table => new
					{
						Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
						FirstName = table.Column<string>(type: "nvarchar(70)", nullable: true),
						LastName = table.Column<string>(type: "nvarchar(70)", nullable: true),
						UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
						NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
						Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
						NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
						EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
						PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
						SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
						ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
						PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
						PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
						TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
						LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
						LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
						AccessFailedCount = table.Column<int>(type: "int", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_ApplicationUser", x => x.Id);
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
						CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
											name: "FK_Listings_ApplicationUser_CreatedById",
											column: x => x.CreatedById,
											principalTable: "ApplicationUser",
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
						UserApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
						ListingId = table.Column<string>(type: "nvarchar(450)", nullable: true)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_ListingInquiries", x => x.Id);
						table.ForeignKey(
											name: "FK_ListingInquiries_ApplicationUser_UserApplicantId",
											column: x => x.UserApplicantId,
											principalTable: "ApplicationUser",
											principalColumn: "Id");
						table.ForeignKey(
											name: "FK_ListingInquiries_Listings_ListingId",
											column: x => x.ListingId,
											principalTable: "Listings",
											principalColumn: "Id");
					});

			migrationBuilder.CreateIndex(
					name: "IX_AddressCompany_CompaniesId",
					table: "AddressCompany",
					column: "CompaniesId");

			migrationBuilder.CreateIndex(
					name: "IX_ListingInquiries_ListingId",
					table: "ListingInquiries",
					column: "ListingId");

			migrationBuilder.CreateIndex(
					name: "IX_ListingInquiries_UserApplicantId",
					table: "ListingInquiries",
					column: "UserApplicantId");

			migrationBuilder.CreateIndex(
					name: "IX_Listings_AddressId",
					table: "Listings",
					column: "AddressId");

			migrationBuilder.CreateIndex(
					name: "IX_Listings_CreatedById",
					table: "Listings",
					column: "CreatedById");

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
					name: "ApplicationUser");

			migrationBuilder.DropTable(
					name: "Companies");
		}
	}
}
