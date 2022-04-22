using FluentAssertions;
using GT.Core.Services.Impl;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GT.Core.Tests.Services
{
	public class GTExperienceLevelServiceTests
	{
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
