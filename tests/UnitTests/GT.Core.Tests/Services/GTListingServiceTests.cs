using FluentAssertions;
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
using System.Text;
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

		//private Task<ListingDTO?> GetByIdAsync(string listingId);
		[Fact]
		public async Task GetByIdAsync_s_Succeeds()
		{
			// Arrange

			// Act

			// Assert

			throw new NotImplementedException();
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

		//private Task UpdateAsync(ListingDTO listingDTO, string listingId);
		[Fact]
		public async Task UpdateAsync_s_Succeeds()
		{
			// Arrange

			// Act

			// Assert

			throw new NotImplementedException();
		}

		//private Task DeleteAsync(string listingId);
		[Fact]
		public async Task DeleteAsync_s_Succeeds()
		{
			// Arrange

			// Act

			// Assert

			throw new NotImplementedException();
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
