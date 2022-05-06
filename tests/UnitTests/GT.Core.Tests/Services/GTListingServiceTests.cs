using FluentAssertions;
using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Impl;
using GT.Data.Data.AppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GT.Core.Tests.Services
{
	public class GTListingServiceTests
	{
		/// <summary>
		/// Tests the filter functionality of the following properties:.<br/>
		/// - FTE <br/>
		/// - SalaryMax<br/>
		/// - SalaryMin<br/>
		///	- IncludeListingsFromDate<br/>
		///	- ExcludeExpiredListings<br/>
		///	- ExperienceLevels<br/>
		/// <br/>
		/// Unable to unit test all filter functionality due to the usage of EF.Functions,<br/>
		/// which only works with Entity Framework and thus are incompatible with Mocks. <br/>
		/// <br/>
		/// The current test does not test the filter finctionality of the following filter model property:<br/>
		/// - KeywordsRawText<br/>
		/// - Location
		/// </summary>
		[Fact]
		public async Task GetAllByFilterAsync_TestPartialFilterFunctionality_SucceedsAndReturnsMatch()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			var listingInDatabase = new Listing()
			{
				Description = "ExampleDescription",
				ListingTitle = "ExampleListingTitle",
				JobTitle = "ExampleJobTitle",
				FTE = true,
				SalaryMax = 10,
				SalaryMin = 1,
				ApplicationDeadline = DateTime.Today.AddMonths(1),
				CreatedDate = DateTime.Now,
				Location = new Location { Name = "ExampleLocationName" },
				Employer = new Company { Name = "ExampleCompanyName" },
				ExperienceLevel = new ExperienceLevel() { Name = "ExampleExperienceLevelName" }
			};

			var partiallyFilledFilterModel = new PostListingFilterDTO
			{
				//KeywordsRawText = "eXaMpLeDescription eXampleListingTitle ExampleJobTitle ExampleCompanyName ExampleLocationName",
				//Location = "ExampleLocationName",
				FTE = true,
				SalaryMax = 20,
				SalaryMin = 8,
				IncludeListingsFromDate = DateTime.Today.AddMonths(-1),
				ExcludeExpiredListings = true,
				ExperienceLevels = new List<string>() { "ExampleExperienceLevelName", "ExampleShouldNotMatter" },
			};

			mockListingRepository
				.Setup(x => x.Get())
				.Returns(new List<Listing> {
					listingInDatabase,
					new Listing(),
					new Listing()
				}.AsQueryable().BuildMock());

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			var result = await sut.GetAllByFilterAsync(partiallyFilledFilterModel);

			// Assert
			result.Count.Should().Be(1);

			result[0].ApplicationDeadline.Should()
				.Be(listingInDatabase.ApplicationDeadline);

			result[0].CreatedDate.Should()
				.Be(listingInDatabase.CreatedDate);

			result[0].EmployerName.Should()
				.Be(listingInDatabase.Employer.Name);

			result[0].ExperienceLevel.Should()
				.Be(listingInDatabase.ExperienceLevel.Name);

			result[0].FTE.Should()
				.Be(listingInDatabase.FTE);

			result[0].Id.Should()
				.Be(listingInDatabase.Id);

			result[0].JobTitle.Should()
				.Be(listingInDatabase.JobTitle);

			result[0].SalaryMax.Should()
				.Be(listingInDatabase.SalaryMax);

			result[0].SalaryMin.Should()
				.Be(listingInDatabase.SalaryMin);
		}

		[Fact]
		public async Task GetAllByFilterAsync_EmptyFilterModel_SucceedsAndReturnsAllEntitiesInDatabase()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			var emptyFilterModel = new PostListingFilterDTO();

			mockListingRepository
				.Setup(x => x.Get())
				.Returns(new List<Listing> {
					new Listing(),
					new Listing(),
					new Listing()
				}.AsQueryable().BuildMock());

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			var result = await sut.GetAllByFilterAsync(emptyFilterModel);

			// Assert
			result.Count.Should().Be(3);
		}

		[Fact]
		public async Task GetByIdAsync_InputsValidId_Succeeds()
		{
			// Arrange
			var idOfListingWhichExistsInDb = "92f44091-1f99-400c-b18d-b2789eac5c81";

			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			mockListingRepository
				.Setup(m => m.Get())
				.Returns(new List<Listing>() { new Listing { Id = idOfListingWhichExistsInDb } }
				.AsQueryable()
				.BuildMock()
				);

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			var result = await sut.GetByIdAsync(idOfListingWhichExistsInDb);

			// Assert
			result.Id.Should()
				.Be(idOfListingWhichExistsInDb);
		}

		[Theory]
		[InlineData("Id which does not exist in Db")]
		[InlineData("")]
		[InlineData(null)]
		public async Task GetByIdAsync_MultipleInvalidIdInputs_FailsAndReturnsNull(string inputId)
		{
			// Arrange
			var listingInDatabase = new Listing()
			{
				Id = "92f44091-1f99-400c-b18d-b2789eac5c81",
				CreatedById = "12a33023-1f13-456a-a20c-a1111abc4a52",
				Description = "ExampleDescription",
				ListingTitle = "ExampleListingTitle",
				JobTitle = "ExampleJobTitle",
				FTE = true,
				SalaryMax = 10,
				SalaryMin = 1,
				ApplicationDeadline = DateTime.Now,
				CreatedDate = DateTime.Now,
				Location = new Location { Name = "ExampleLocationName" },
				Employer = new Company { Name = "ExampleCompanyName" },
				ExperienceLevel = new ExperienceLevel() { Name = "ExampleExperienceLevelName" }
			};

			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			mockListingRepository
				.Setup(m => m.Get())
				.Returns(new List<Listing>() { listingInDatabase }.AsQueryable().BuildMock());

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			var result = await sut.GetByIdAsync(inputId);

			// Assert
			result.Should()
				.BeNull();
		}

		[Fact]
		public async Task AddAsync_AddValidNewListing_Succeeds()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			var inputDTO = new ListingDTO()
			{
				Description = "ExampleDescription",
				ListingTitle = "ExampleListingTitle",
				JobTitle = "ExampleJobTitle",
				FTE = true,
				SalaryMax = 10,
				SalaryMin = 1,
				ApplicationDeadline = new DateTime(2022, 12, 30),
				CreatedDate = null,
				Location = "ExampleLocationName",
				Employer = "ExampleCompanyName",
				ExperienceLevel = "ExampleExperienceLevelName"
			};

			var expectedEntity = new Listing()
			{
				Description = "ExampleDescription",
				ListingTitle = "ExampleListingTitle",
				JobTitle = "ExampleJobTitle",
				FTE = true,
				SalaryMax = 10,
				SalaryMin = 1,
				ApplicationDeadline = DateTime.Today.AddMonths(1),
				Location = new Location { Name = "ExampleLocationName" },
				Employer = new Company { Name = "ExampleCompanyName" },
				ExperienceLevel = new ExperienceLevel() { Name = "ExampleExperienceLevelName" }
			};

			mockListingRepository
				.Setup(m => m.Get())
				.Returns(new List<Listing>() { }.AsQueryable().BuildMock());

			mockCompanyRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>() { new Company { Name = "ExampleCompanyName" } }
				.AsQueryable().BuildMock());

			mockLocationRepository
				.Setup(m => m.Get())
				.Returns(new List<Location>() { new Location { Name = "ExampleLocationName" } }
				.AsQueryable().BuildMock());

			mockExperienceLevelRepository
				.Setup(m => m.Get())
				.Returns(new List<ExperienceLevel>() { new ExperienceLevel { Name = "ExampleExperienceLevelName" } }
				.AsQueryable().BuildMock());

			var callbackListingResult = new Listing();
			mockListingRepository
				.Setup(m => m.AddAsync(It.IsAny<Listing>()))
				.Callback<Listing>(listingInputArg => callbackListingResult = listingInputArg)
				.ReturnsAsync(callbackListingResult);

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act

			var result = await sut.AddAsync(inputDTO, Guid.NewGuid().ToString());

			// Assert

			result.Should().NotBeNull();

			result.CreatedDate.Should()
				.Be(callbackListingResult.CreatedDate).And
				.NotBeNull();

			callbackListingResult.CreatedDate.Should()
				.NotBeNull();

			result.ApplicationDeadline.Should()
				.Be(inputDTO.ApplicationDeadline).And
				.Be(callbackListingResult.ApplicationDeadline);

			result.Description.Should()
				.Be(inputDTO.Description).And
				.Be(callbackListingResult.Description);

			result.Employer.Should()
				.Be(inputDTO.Employer).And
				.Be(callbackListingResult.Employer?.Name);

			result.ExperienceLevel.Should()
				.Be(inputDTO.ExperienceLevel).And
				.Be(callbackListingResult.ExperienceLevel?.Name);

			result.FTE.Should()
				.Be(inputDTO.FTE).And
				.Be(callbackListingResult.FTE);
		}

		[Fact]
		public async Task AddAsync_NullArguments_FailsAndReturnsNull()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			var result = await sut.AddAsync(null, null);

			// Assert
			mockListingRepository.Verify(m => m.AddAsync(It.IsAny<Listing>()), Times.Never);

			result.Should().BeNull();
		}

		[Fact]
		public async Task UpdateAsync_ValidInputAndAllNewEntities_ReturnsUpdatedDto()
		{
			// Arrange
			var entityInDb = new Listing()
			{
				Id = "92f44091-1f99-400c-b18d-b2789eac5c81",
				CreatedById = "12a33023-1f13-456a-a20c-a1111abc4a52",
				Description = "Old Description",
				ListingTitle = "Old ExampleListingTitle",
				JobTitle = "Old JobTitle",
				FTE = true,
				SalaryMax = 10,
				SalaryMin = 1,
				ApplicationDeadline = DateTime.Now,
				CreatedDate = DateTime.Now,
				Location = new Location { Name = "Old Location" },
				Employer = new Company { Name = "Old Company" },
				ExperienceLevel = new ExperienceLevel() { Name = "Old ExperienceLevel" }
			};

			var inputUpdatedDTO = new ListingDTO()
			{
				Id = "92f44091-1f99-400c-b18d-b2789eac5c81",
				Description = "New Description",
				ListingTitle = "New ExampleListingTitle",
				JobTitle = "New JobTitle",
				FTE = false,
				SalaryMax = 20,
				SalaryMin = 2,
				ApplicationDeadline = DateTime.Now,
				CreatedDate = DateTime.Now,
				Location = "New Location",
				Employer = "New Company",
				ExperienceLevel = "New ExperienceLevel"
			};

			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			mockListingRepository
				.Setup(m => m.Get())
				.Returns(new List<Listing>() { entityInDb }.AsQueryable().BuildMock());

			mockCompanyRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>() { new Company { Name = "New Company" } }
				.AsQueryable().BuildMock());

			mockLocationRepository
				.Setup(m => m.Get())
				.Returns(new List<Location>() { new Location { Name = "New Location" } }
				.AsQueryable().BuildMock());

			mockExperienceLevelRepository
				.Setup(m => m.Get())
				.Returns(new List<ExperienceLevel>() { new ExperienceLevel { Name = "New ExperienceLevel" } }
				.AsQueryable().BuildMock());

			var callbackListingResult = new Listing();
			var callbackListingId = string.Empty;
			mockListingRepository
				.Setup(m => m.UpdateAsync(It.IsAny<Listing>(), It.IsAny<string>()))
					.Callback<Listing, string>((listingInputArg, idInputArg) =>
					{
						callbackListingResult = listingInputArg;
						callbackListingId = idInputArg;
					});

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.UpdateAsync(inputUpdatedDTO, inputUpdatedDTO.Id);

			// Assert
			mockListingRepository.Verify(m => m.UpdateAsync(It.IsAny<Listing>(), It.IsAny<string>()), Times.Once);

			callbackListingResult.Id.Should()
				.Be(entityInDb.Id).And
				.Be(callbackListingId);

			callbackListingResult.CreatedById.Should()
				.Be(entityInDb.CreatedById);

			callbackListingResult.FTE.Should()
				.Be(inputUpdatedDTO.FTE);

			callbackListingResult.SalaryMin.Should()
				.Be(inputUpdatedDTO.SalaryMin);

			callbackListingResult.SalaryMax.Should()
				.Be(inputUpdatedDTO.SalaryMax);

			callbackListingResult.ApplicationDeadline.Should()
				.Be(inputUpdatedDTO.ApplicationDeadline);

			callbackListingResult.Description.Should()
				.Be(inputUpdatedDTO.Description);

			callbackListingResult.Location?.Name.Should()
				.Be(inputUpdatedDTO.Location);

			callbackListingResult.Employer?.Name.Should()
				.Be(inputUpdatedDTO.Employer);

			callbackListingResult.ExperienceLevel?.Name.Should()
				.Be(inputUpdatedDTO.ExperienceLevel);
		}

		[Fact]
		public async Task UpdateAsync_NullReferenceArgument_Fails()
		{
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.UpdateAsync(null, null);

			// Assert
			mockListingRepository.Verify(m => m.UpdateAsync(It.IsAny<Listing>(), It.IsAny<string>()), Times.Never);
		}

		[Theory]
		[InlineData("92f44091-1f99-400c-b18d-b2789eac5c81", "12a33023-1f13-456a-a20c-a1111abc4a52")]
		[InlineData(null, "92f44091-1f99-400c-b18d-b2789eac5c81")]
		[InlineData("92f44091-1f99-400c-b18d-b2789eac5c81", null)]
		public async Task UpdateAsync_MultipleUnmatchingIdInputs_Fails(string unmatchingId1, string unmatchingId2)
		{
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.UpdateAsync(new ListingDTO { Id = unmatchingId1 }, unmatchingId2);

			// Assert
			mockListingRepository.Verify(m => m.UpdateAsync(It.IsAny<Listing>(), It.IsAny<string>()), Times.Never);
		}

		[Fact]
		public async Task DeleteAsync_ValidIdInput_Succeeds()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			var exampleIdInDB = "92f44091-1f99-400c-b18d-b2789eac5c81";

			mockListingRepository
				.Setup(m => m.Get())
				.Returns(new List<Listing>() {
				new Listing() { Id = exampleIdInDB }
				}
				.AsQueryable()
				.BuildMock()
				);

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.DeleteAsync(exampleIdInDB);

			// Assert
			mockListingRepository.Verify(m => m.DeleteAsync(It.IsAny<Listing>()), Times.Once);
		}

		[Theory]
		[InlineData("ex id which is not in the database")]
		[InlineData("")]
		[InlineData(null)]
		public async Task DeleteAsync_MultipleInvalidIdInputs_Fails(string inputIdNotInDB)
		{
			// Arrange
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			mockListingRepository
				.Setup(m => m.Get())
				.Returns(new List<Listing>() {
				new Listing() { Id = "92f44091-1f99-400c-b18d-b2789eac5c81" }
				}
				.AsQueryable()
				.BuildMock()
				);

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.DeleteAsync(inputIdNotInDB);

			// Assert
			mockListingRepository.Verify(m => m.DeleteAsync(It.IsAny<Listing>()), Times.Never);
		}

		[Theory]
		[InlineData("92f44091-1f99-400c-b18d-b2789eac5c81", true)]
		[InlineData("IdNotInDb", false)]
		[InlineData(null, false)]
		public async Task ExistsByNameAsync_CheckMultipleInputs_ReturnsExpectedValue(string inputId, bool expected)
		{
			// Arrange
			var mockLogger = new Mock<ILogger<ListingService>>();
			var mockListingRepository = new Mock<IGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGenericRepository<Inquiry>>();

			mockListingRepository
				.Setup(m => m.Get())
				.Returns(new List<Listing>() {
				new Listing() { Id = "92f44091-1f99-400c-b18d-b2789eac5c81" }
				}
				.AsQueryable()
				.BuildMock()
				);

			var sut = new ListingService(
				mockLogger.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			var result = await sut.ExistsByIdAsync(inputId);

			// Assert
			result.Should().Be(expected);
		}
	}
}
