using FluentAssertions;
using GT.Core.DTO.Impl;
using GT.Core.Services.Impl;
using GT.Data.Data.AppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelpers;
using Xunit;

namespace GT.Core.Tests.Services
{
	public class GTLocationServiceTests
	{
		//[Theory]
		//[InlineData("Stockholm", "id which should be replaced by service")]
		//[InlineData("Örkelljunga", null)]
		//[InlineData("東京", "")]
		//public async Task AddAsync_AddValidNewLocation_Succeeds(string? inputLocationName, string? inputTempId)
		//{
		//	// Arrange
		//	var dto = new LocationDTO()
		//	{
		//		Id = inputTempId,
		//		Name = inputLocationName
		//	};

		//	var mockLogger = new Mock<ILogger<LocationService>>();
		//	var mockRepository = new Mock<IGenericRepository<Location>>();
		//	var callbackResult = new Location();

		//	mockRepository
		//		.Setup(m => m.AddAsync(It.IsAny<Location>()))
		//		.Callback<Location>(inputArgs => callbackResult = inputArgs)
		//		.Returns(Task.FromResult(callbackResult));

		//	var sut = new LocationService(mockLogger.Object, mockRepository.Object);

		//	// Act
		//	var result = await sut.AddAsync(dto);

		//	// Assert
		//	mockRepository.Verify(m => m.AddAsync(It.IsAny<Location>()), Times.Once);

		//	result.Id.Should()
		//		.MatchRegex(RegexSnippets.GetGuidRegex()).And
		//		.Be(callbackResult.Id).And
		//		.NotBe(inputTempId);

		//	result.Name.Should()
		//		.Be(inputLocationName).And
		//		.Be(callbackResult.Name).And
		//		.Be(dto.Name);
		//}

		//[Theory]
		//[InlineData(null, "id which should be replaced by service")]
		//[InlineData("", null)]
		//public async Task AddAsync_AddInvalidNewLocation_FailsAndReturnsNull(string? inputLocationName, string? inputTempId)
		//{
		//	// Arrange
		//	var dto = new LocationDTO()
		//	{
		//		Id = inputTempId,
		//		Name = inputLocationName
		//	};

		//	var mockLogger = new Mock<ILogger<LocationService>>();
		//	var mockRepository = new Mock<IGenericRepository<Location>>();

		//	var sut = new LocationService(mockLogger.Object, mockRepository.Object);

		//	// Act
		//	var result = await sut.AddAsync(dto);

		//	// Assert
		//	mockRepository.Verify(m => m.AddAsync(It.IsAny<Location>()), Times.Never);

		//	result.Should()
		//		.BeNull();
		//}

		//[Fact]
		//public async Task AddAsync_NullReferenceArgument_FailsAndReturnsNull()
		//{
		//	// Arrange
		//	LocationDTO dto = null;

		//	var mockLogger = new Mock<ILogger<LocationService>>();
		//	var mockRepository = new Mock<IGenericRepository<Location>>();

		//	var sut = new LocationService(mockLogger.Object, mockRepository.Object);

		//	// Act
		//	var result = await sut.AddAsync(dto);

		//	// Assert
		//	mockRepository.Verify(m => m.AddAsync(It.IsAny<Location>()), Times.Never);

		//	result.Should()
		//		.BeNull();
		//}

		//[Theory]
		//[InlineData("NameInDb", true)]
		//[InlineData("NameNotInDb1", false)]
		//[InlineData(null, false)]
		//public async Task ExistsByNameAsync_CheckMultipleInputs_ReturnsExpectedValue(
		//	string inputName,
		//	bool expected)
		//{
		//	// Arrange
		//	var mockLogger = new Mock<ILogger<LocationService>>();
		//	var mockRepository = new Mock<IGenericRepository<Location>>();

		//	mockRepository
		//		.Setup(m => m.Get())
		//		.Returns(new List<Location>
		//		{
		//			new Location { Name = "NameInDb" }
		//		}
		//			.AsQueryable()
		//			.BuildMock()
		//		);

		//	var sut = new LocationService(mockLogger.Object, mockRepository.Object);

		//	// Act
		//	var result = await sut.ExistsByNameAsync(inputName);

		//	// Assert
		//	result.Should()
		//		.Be(expected);
		//}

		//[Fact]
		//public async Task GetAllAsync_ThreeEntitiesExistInDB_SucceedsAndReturnsThreeEntities()
		//{
		//	// Arrange
		//	var mockLogger = new Mock<ILogger<LocationService>>();
		//	var mockRepository = new Mock<IGenericRepository<Location>>();

		//	mockRepository
		//		.Setup(m => m.Get())
		//		.Returns(new List<Location>()
		//		 {
		//				new Location {},
		//				new Location {},
		//				new Location {}
		//		 }.AsQueryable().BuildMock()
		//		);

		//	var sut = new LocationService(mockLogger.Object, mockRepository.Object);

		//	// Act
		//	var result = await sut.GetAllAsync();

		//	// Assert
		//	result.Count.Should()
		//		.Be(3);
		//}

		//[Fact]
		//public async Task GetAllAsync_NoEntitiesExistInDB_SucceedsAndReturnsEmptyListOfDTO()
		//{
		//	// Arrange
		//	var mockLogger = new Mock<ILogger<LocationService>>();
		//	var mockRepository = new Mock<IGenericRepository<Location>>();

		//	mockRepository
		//		.Setup(m => m.Get())
		//		.Returns(new List<Location>()
		//		{ }.AsQueryable().BuildMock()
		//		);

		//	var sut = new LocationService(mockLogger.Object, mockRepository.Object);

		//	// Act
		//	var result = await sut.GetAllAsync();

		//	// Assert
		//	result.Count.Should()
		//		.Be(0);
		//}
	}
}
