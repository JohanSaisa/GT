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
		[Fact]
		public async Task AddAsync_AddValidNewLocation_Succeeds()
		{
			// Arrange
			var mockServiceLogger = new Mock<ILogger<GTLocationService>>();
			var mockRepository = new Mock<IGTGenericRepository<Location>>();
			var locationAsInputArgument = new Location();

			mockRepository
				.Setup(m => m.AddAsync(It.IsAny<Location>()))
				.Callback<Location>(location => locationAsInputArgument = location)
				.Returns(Task.FromResult(locationAsInputArgument));

			var systemUnderTest = new GTLocationService(mockServiceLogger.Object, mockRepository.Object);
			var inputLocationName = "Stockholm";
			var inputLocationTempId = "shouldBeReplacedByGuid";

			var input = new LocationDTO()
			{
				Id = inputLocationTempId,
				Name = inputLocationName
			};

			// Act
			var result = await systemUnderTest.AddAsync(input);

			// Assert
			result.Id.Should().MatchRegex(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$");
			result.Id.Should().NotBe(inputLocationTempId);
			result.Name.Should().Be(inputLocationName);
			result.Name.Should().Be(locationAsInputArgument.Name);

			mockRepository.Verify(m => m.AddAsync(It.IsAny<Location>()), Times.Once);
		}
	}
}
