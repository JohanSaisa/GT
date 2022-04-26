using FluentAssertions;
using GT.Core.DTO.Impl;
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
using TestHelpers;
using Xunit;

namespace GT.Core.Tests.Services
{
	public class GTCompanyServiceTests
	{
		[Theory]
		[InlineData("Google" , "", "")]
		[InlineData("Rörfabriken32", "id which should be replaced by service", "id which should be replaced with null by service")]
		[InlineData("Kalles Bullfabrik", null, null)]

		public async Task AddAsync_AddValidCompanyDTO_Succeeds(string inputCompanyName, string inputTempId, string inputCompanyLogoId)
		{
			// Arrange
			var dto = new CompanyDTO()
			{
				Id = inputTempId,
				Name = inputCompanyName,
				CompanyLogoId = inputCompanyLogoId
			};

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			mockRepository
				.Setup(m => m.AddAsync(It.IsAny<Company>()))
				.Callback<Company>(inputArgs => callBackResult = inputArgs)
				.Returns(Task.FromResult(callBackResult));

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			result.Id.Should()
				.MatchRegex(RegexSnippets.GetGuidRegex()).And
				.Be(callBackResult.Id).And
				.NotBe(inputTempId);

			result.Name.Should()
				.Be(inputCompanyName).And
				.Be(callBackResult.Name).And
				.Be(dto.Name);

			result.CompanyLogoId.Should()
				.BeNull();

			callBackResult.CompanyLogoId.Should()
				.BeNull();
		}

		[Theory]
		[InlineData(null, "", "")]
		[InlineData(null, "id which should be replaced by service", "id which should be replaced with null by service")]
		[InlineData(null, null, "id which should be replaced with null by service")]
		[InlineData(null, null, null)]
		[InlineData("       ", "       ", "       ")]
		public async Task AddAsync_AddInvalidCompanyDTO_FailsAndReturnsNull(string inputCompanyName, string inputTempId, string inputCompanyLogoId)
		{
			// Arrange
			var dto = new CompanyDTO()
			{
				Id = inputTempId,
				Name = inputCompanyName,
				CompanyLogoId = inputCompanyLogoId
			};

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			mockRepository.Verify(m => m.AddAsync(It.IsAny<Company>()), Times.Never);

			result.Should()
				.BeNull();
		}

		[Theory]
		[InlineData("    CompanyName    ")]
		[InlineData("    Company Name    ")]
		[InlineData("    Company    Name    ")]
		public async Task AddAsync_CheckIfMethodTrimInputName_TrimsLeadingAndTrailingWhitespaces(string inputCompanyName)
		{
			// Arrange
			var dto = new CompanyDTO()
			{
				Name = inputCompanyName,
			};

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			mockRepository
				.Setup(m => m.AddAsync(It.IsAny<Company>()))
				.Callback<Company>(inputArgs => callBackResult = inputArgs)
				.Returns(Task.FromResult(callBackResult));

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			result.Name.Should()
				.Be(inputCompanyName.Trim());

			callBackResult.Name.Should()
				.Be(inputCompanyName.Trim());
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("123-abc")]
		public async Task DeleteAsync_InvalidId_Fails(string inputCompanyId)
		{
			// Arrange
			var dto = new CompanyDTO()
			{
				Id = inputCompanyId
			};

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			mockRepository
				.Setup(m => m.Get(It.IsAny<Company>()))

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.AddAsync(dto);

			// Assert
			mockRepository.Verify(m => m.AddAsync(It.IsAny<Company>()), Times.Never);
		}
	}
}
