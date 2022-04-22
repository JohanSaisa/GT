using FluentAssertions;
using GT.Core.DTO.Impl;
using GT.Core.Services.Impl;
using GT.Data.Data.GTAppDb.Entities;
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
	public class GTExperienceLevelServiceTests
	{
		[Theory]
		[InlineData("Senior", "id which should be replaced by service")]
		[InlineData("Junior", null)]
		[InlineData("東京", null)]
		public async Task AddAsync_AddValidExperienceLevel_Succeeds(string inputExperienceLevelName, string inputTempId)
		{
			// Arrange
			var dto = new ExperienceLevelDTO()
			{
				Id = inputTempId,
				Name = inputExperienceLevelName
			};

			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var callbackResult = new ExperienceLevel();

			mockRepository
				.Setup(m => m.AddAsync(It.IsAny<ExperienceLevel>()))
				.Callback<ExperienceLevel>(inputArgs => callbackResult = inputArgs)
				.Returns(Task.FromResult(callbackResult));

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			result.Id.Should()
				.MatchRegex(RegexSnippets.GetGuidRegex()).And
				.Be(callbackResult.Id).And
				.NotBe(inputTempId);

			result.Name.Should()
				.Be(inputExperienceLevelName).And
				.Be(callbackResult.Name).And
				.Be(dto.Name);
		}

		[Theory]
		[InlineData("NameInDb", true)]
		[InlineData("NameNotInDb1", false)]
		[InlineData(null, false)]
		public async Task ExistsByNameAsync_ReturnsExpectedValue_Succeeds(string inputName, bool expected)
		{
			// Arrange
			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();

			mockRepository
				.Setup(m => m.GetAll())
				.Returns(new List<ExperienceLevel>()
				 {
						new ExperienceLevel { Name = "NameInDb" },
				 }.AsQueryable().BuildMock()
				);

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.ExistsByNameAsync(inputName);

			// Assert
			result.Should()
				.Be(expected);
		}
	}
}
