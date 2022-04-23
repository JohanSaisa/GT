﻿using FluentAssertions;
using GT.Core.DTO.Impl;
using GT.Core.Services.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
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
		//private Task<List<ListingOverviewDTO>> GetAsync(IListingFilterModel? filter = null);
		[Fact]
		public async Task GetAllAsync_EmptyFilterModel_SucceedsAndReturnsAllThreeEntities()
		{
			// Arrange

			// Act

			// Assert

			throw new NotImplementedException();
		}

		[Fact]
		public async Task GetAllAsync_AllFilterOptions_SucceedsAndReturnsMatch()
		{
			// Arrange

			// Act

			// Assert

			throw new NotImplementedException();
		}

		[Fact]
		public async Task GetByIdAsync_InputsValidId_Succeeds()
		{
			// Arrange
			var idOfListingWhichExistsInDb = "92f44091-1f99-400c-b18d-b2789eac5c81";

			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			mockListingRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Listing>() { new Listing { Id = idOfListingWhichExistsInDb } }
				.AsQueryable()
				.BuildMock()
				);

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
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
			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			mockListingRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Listing>() { new Listing { Id = "92f44091-1f99-400c-b18d-b2789eac5c81" } }
				.AsQueryable()
				.BuildMock()
				);

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
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

		//private Task<ListingDTO?> AddAsync(ListingDTO listingDTO, string signedInUserId);
		[Fact]
		public async Task AddAsync_AddValidNewListing_Succeeds()
		{
			// Arrange



			// Act

			// Assert

			throw new NotImplementedException();
		}

		[Fact]
		public async Task AddAsync_AddInvalidNewListing_FailsAndReturnsNull()
		{
			// Arrange

			// Act

			// Assert

			throw new NotImplementedException();
		}

		// Needs to check if it triggers creation of new objects
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

			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			mockListingRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Listing>() { entityInDb }.AsQueryable().BuildMock());

			mockCompanyRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Company>() { new Company { Name = "New Company" } }
				.AsQueryable().BuildMock());

			mockLocationRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Location>() { new Location { Name = "New Location" } }
				.AsQueryable().BuildMock());

			mockExperienceLevelRepository
				.Setup(m => m.GetAll())
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

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.UpdateAsync(inputUpdatedDTO, inputUpdatedDTO.Id);

			// Assert
			mockCompanyService.Verify(m => m.AddAsync(It.IsAny<CompanyDTO>()), Times.Once);
			mockLocationService.Verify(m => m.AddAsync(It.IsAny<LocationDTO>()), Times.Once);
			mockExperienceLevelService.Verify(m => m.AddAsync(It.IsAny<ExperienceLevelDTO>()), Times.Once);

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
			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
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
			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
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
			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			var exampleIdInDB = "92f44091-1f99-400c-b18d-b2789eac5c81";

			mockListingRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Listing>() {
				new Listing() { Id = exampleIdInDB }
				}
				.AsQueryable()
				.BuildMock()
				);

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.DeleteAsync(exampleIdInDB);

			// Assert
			mockListingRepository.Verify(m => m.DeleteAsync(It.IsAny<string>()), Times.Once);
		}

		[Theory]
		[InlineData("ex id which is not in the database")]
		[InlineData("")]
		[InlineData(null)]
		public async Task DeleteAsync_MultipleInvalidIdInputs_Fails(string inputIdNotInDB)
		{
			// Arrange
			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			mockListingRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Listing>() {
				new Listing() { Id = "92f44091-1f99-400c-b18d-b2789eac5c81" }
				}
				.AsQueryable()
				.BuildMock()
				);

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
				mockListingRepository.Object,
				mockCompanyRepository.Object,
				mockLocationRepository.Object,
				mockExperienceLevelRepository.Object,
				mockInquiryRepository.Object
				);

			// Act
			await sut.DeleteAsync(inputIdNotInDB);

			// Assert
			mockListingRepository.Verify(m => m.DeleteAsync(It.IsAny<string>()), Times.Never);
		}

		//private Task<bool> ExistsByIdAsync(string listingId);
		[Theory]
		[InlineData("92f44091-1f99-400c-b18d-b2789eac5c81", true)]
		[InlineData("IdNotInDb", false)]
		[InlineData(null, false)]
		public async Task ExistsByNameAsync_CheckMultipleInputs_ReturnsExpectedValue(string inputId, bool expected)
		{
			// Arrange
			var mockLogger = new Mock<ILogger<GTListingService>>();
			var mockCompanyService = new Mock<IGTCompanyService>();
			var mockExperienceLevelService = new Mock<IGTExperienceLevelService>();
			var mockLocationService = new Mock<IGTLocationService>();
			var mockListingRepository = new Mock<IGTGenericRepository<Listing>>();
			var mockCompanyRepository = new Mock<IGTGenericRepository<Company>>();
			var mockLocationRepository = new Mock<IGTGenericRepository<Location>>();
			var mockExperienceLevelRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var mockInquiryRepository = new Mock<IGTGenericRepository<ListingInquiry>>();

			mockListingRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Listing>() {
				new Listing() { Id = "92f44091-1f99-400c-b18d-b2789eac5c81" }
				}
				.AsQueryable()
				.BuildMock()
				);

			var sut = new GTListingService(
				mockLogger.Object,
				mockCompanyService.Object,
				mockExperienceLevelService.Object,
				mockLocationService.Object,
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
