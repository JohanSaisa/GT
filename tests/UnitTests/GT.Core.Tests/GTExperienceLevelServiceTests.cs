using GT.Core.Services.Impl;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GT.Core.Tests
{
	public class GTExperienceLevelServiceTests
	{

		[Fact]
		public async Task ExistsByNameAsync_CheckForValidExperienceLevel_Succeeds()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<GTExperienceLevelService>>();
			var mockRepository = new Mock<IGTGenericRepository<ExperienceLevel>>();

			var sut = new GTExperienceLevelService(mockLogger.Object, mockRepository.Object);

			mockRepository.Setup(m => m.GetAll().AnyAsync()
				.Returns(true));

			// Act
			var expected = true;
			var result = sut.ExistsByNameAsync("ValidNameDB");
						
			// Assert
		}
	}
}
