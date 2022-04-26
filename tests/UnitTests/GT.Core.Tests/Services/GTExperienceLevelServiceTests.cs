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
		[InlineData("東京", "")]
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
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData(null, "")]
		public async Task AddAsync_AddInValidExperienceLevel_FailsAndReturnsNull(string inputExperienceLevelName, string inputTempId)
		{
			// Arrange
			var dto = new ExperienceLevelDTO()
			{
				Id = inputTempId,
				Name = inputExperienceLevelName
			};

			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			mockRepository.Verify(m => m.AddAsync(It.IsAny<ExperienceLevel>()), Times.Never);

			result.Should()
				.BeNull();
		}

		[Theory]
		[InlineData("    Intermediate    ", "Intermediate")]
		[InlineData("    Entry level    ", "Entry level")]
		public async Task AddAsync_CheckIfMethodTrimInputName_TrimsLeadingAndTrailingWhitespaces(string inputNameWithWhitespaces, string expected)
		{
			// Arrange
			var dto = new ExperienceLevelDTO()
			{
				Name = inputNameWithWhitespaces
			};

			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();
			var callbackResult = new ExperienceLevel();

			mockRepository
				.Setup(m => m.AddAsync(It.IsAny<ExperienceLevel>()))
				.Callback<ExperienceLevel>(inputArgs => callbackResult = inputArgs);

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			result.Name.Should()
				.Be(expected);
		}

		[Fact]
		public async Task AddAsync_NullReferenceArgument_FailsAndReturnsNull()
		{
			// Arrange
			ExperienceLevelDTO dto = null;

			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			mockRepository.Verify(m => m.AddAsync(It.IsAny<ExperienceLevel>()), Times.Never);

			result.Should()
				.BeNull();
		}

		[Theory]
		[InlineData("NameInDb", true)]
		[InlineData("NameNotInDb1", false)]
		[InlineData(null, false)]
		public async Task ExistsByNameAsync_CheckMultipleInputs_ReturnsExpectedValue(string inputName, bool expected)
		{
			// Arrange
			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();

			mockRepository
				.Setup(m => m.Get())
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

		[Fact]
		public async Task GetAllAsync_ThreeEntitiesExistInDB_SucceedsAndReturnsThreeEntities()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<ExperienceLevel>()
				 {
						new ExperienceLevel {},
						new ExperienceLevel {},
						new ExperienceLevel {}
				 }.AsQueryable().BuildMock()
				);

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.GetAllAsync();

			// Assert
			result.Count.Should()
				.Be(3);
		}

		[Fact]
		public async Task GetAllAsync_NoEntitiesExistInDB_SucceedsAndReturnsEmptyListOfDTO()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<ExperienceLevel>()
				{ }.AsQueryable().BuildMock()
				);

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.GetAllAsync();

			// Assert
			result.Count.Should()
				.Be(0);
		}
	}
}
