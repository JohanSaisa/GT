using FluentAssertions;
using GT.Core.DTO.Impl;
using GT.Core.Services.Impl;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GT.Core.Tests
{
	public class GTLocationServiceTests
	{
		[Theory]
		[InlineData("Stockholm", "shouldBeReplacedByGuid")]
		[InlineData("Örkelljunga", null)]
		[InlineData("東京", null)]
		public async Task AddAsync_AddValidNewLocation_Succeeds(string? inputLocationName, string? inputLocationTempId)
		{
			// Arrange
			var input = new LocationDTO()
			{
				Id = inputLocationTempId,
				Name = inputLocationName
			};

			var mockServiceLogger = new Mock<ILogger<GTLocationService>>();
			var mockRepository = new Mock<IGTGenericRepository<Location>>();
			var locationSentToRepository = new Location();

			mockRepository
				.Setup(m => m.AddAsync(It.IsAny<Location>()))
				.Callback<Location>(location => locationSentToRepository = location)
				.Returns(Task.FromResult(locationSentToRepository));

			var sut = new GTLocationService(mockServiceLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(input);

			// Assert
			result.Id.Should().MatchRegex(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$");
			result.Id.Should().NotBe(inputLocationTempId);
			result.Name.Should().Be(inputLocationName);
			mockRepository.Verify(m => m.AddAsync(It.IsAny<Location>()), Times.Once);
		}
	}
}
