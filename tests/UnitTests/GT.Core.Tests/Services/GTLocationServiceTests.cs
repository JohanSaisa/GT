using FluentAssertions;
using GT.Core.DTO.Impl;
using GT.Core.Services.Impl;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using TestHelpers;
using Xunit;

namespace GT.Core.Tests.Services
{
	public class GTLocationServiceTests
	{
		[Theory]
		[InlineData("Stockholm", "id which should be replaced by service")]
		[InlineData("Örkelljunga", null)]
		[InlineData("東京", null)]
		public async Task AddAsync_AddValidNewLocation_Succeeds(string? inputLocationName, string? inputTempId)
		{
			// Arrange
			var dto = new LocationDTO()
			{
				Id = inputTempId,
				Name = inputLocationName
			};

			var mockLogger = new Mock<ILogger<GTLocationService>>();
			var mockRepository = new Mock<IGTGenericRepository<Location>>();
			var callbackResult = new Location();

			mockRepository
				.Setup(m => m.AddAsync(It.IsAny<Location>()))
				.Callback<Location>(inputArgs => callbackResult = inputArgs)
				.Returns(Task.FromResult(callbackResult));

			var sut = new GTLocationService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			mockRepository.Verify(m => m.AddAsync(It.IsAny<Location>()), Times.Once);

			result.Id.Should()
				.MatchRegex(RegexSnippets.GetGuidRegex()).And
				.Be(callbackResult.Id).And
				.NotBe(inputTempId);

			result.Name.Should()
				.Be(inputLocationName).And
				.Be(callbackResult.Name).And
				.Be(dto.Name);
		}
	}
}
