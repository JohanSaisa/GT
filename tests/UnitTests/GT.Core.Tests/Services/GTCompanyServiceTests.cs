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
	public class GTCompanyServiceTests
	{
		[Theory]
		[InlineData("Google", "", "")]
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
		[InlineData("     ")]
		[InlineData("123-abc")]
		public async Task DeleteAsync_InvalidInputId_Fails(string inputCompanyId)
		{
			//	Arrange
			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company()
					{
						Id = "0059707d-4533-45b5-953b-676c39fab8a8"
					}
				}.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			await sut.DeleteAsync(inputCompanyId);

			//	Assert
			mockRepository.Verify(m => m.DeleteAsync(It.IsAny<Company>()), Times.Never);
		}

		[Fact]
		public async Task DeleteAsync_ValidInputId_Succeeds()
		{
			//	Arrange
			string inputCompanyId = "Valid companyId";

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company()
					{
						Id = inputCompanyId
					}
				}.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			await sut.DeleteAsync(inputCompanyId);

			//	Assert
			mockRepository.Verify(m => m.DeleteAsync(It.IsAny<Company>()), Times.Once);
		}

		[Fact]
		public async Task ExistsByNameAsync_ValidInputName_Succeeds()
		{
			//	Arrange
			string inputCompanyName = "Valid companyName";

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company()
					{
						Name = inputCompanyName
					}
				}.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			var result = await sut.ExistsByNameAsync(inputCompanyName);

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Once);

			result.Should()
				.BeTrue();
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public async Task ExistsByNameAsync_InvalidInputName_Fails(string inputCompanyName)
		{
			//	Arrange
			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			await sut.ExistsByNameAsync(inputCompanyName);

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Never);
		}

		[Fact]
		public async Task ExistsByNameAsync_ValidNameDoesNotExistInDB_SucceedsButReturnsFalse()
		{
			//	Arrange
			string inputCompanyName = "Valid companyName";

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();
			var callBackResult = new Company();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company()
					{
						Name = "Google"
					}
				}.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			var result = await sut.ExistsByNameAsync(inputCompanyName);

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Once);

			result.Should()
				.BeFalse();
		}

		[Fact]
		public async Task GetAllAsync_ThreeEntitiesExistInDB_SucceedsAndReturnsThreeEntities()
		{
			//	Arrange
			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company(),
					new Company(),
					new Company(),
				}
				.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			var result = await sut.GetAllAsync();

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Once);

			result.Count.Should()
				.Be(3);
		}

		[Fact]
		public async Task GetAllAsync_EmptyDB_SucceedsButReturnsEmptyList()
		{
			//	Arrange
			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>(){}
				.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			var result = await sut.GetAllAsync();

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Once);

			result.Count.Should()
				.Be(0);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public async Task GetByIdAsync_InvalidCompanyId_FailsAndReturnsNull(string inputCompanyId)
		{
			//	Arrange
			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			var result = await sut.GetByIdAsync(inputCompanyId);

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Never);

			result.Should()
				.BeNull();
		}

		[Theory]
		[InlineData("validCompanyId")]
		[InlineData("    validCompanyId   ")]
		public async Task GetByIdAsync_ValidCompanyId_SucceedsAndReturnsDTO(string inputCompanyId)
		{
			//	Arrange
			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company()
					{
						Id = "validCompanyId"
					}
				}.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			var result = await sut.GetByIdAsync(inputCompanyId);

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Once);

			result.Id.Should()
				.Be("validCompanyId");
		}

		[Fact]
		public async Task GetByIdAsync_ValidCompanyIdButNoMatchInDB_SucceedsAndReturnsNull()
		{
			//	Arrange
			string inputCompanyId = "ValidInputId";
			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company()
					{
						Id = "validCompanyId"
					}
				}.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			//	Act
			var result = await sut.GetByIdAsync(inputCompanyId);

			//	Assert
			mockRepository.Verify(m => m.Get(), Times.Once);

			result.Should()
				.BeNull();
		}

		[Fact]
		public async Task GetByIdAsync_CheckIfMethodTrimInputId_TrimsLeadingAndTrailingWhitespaces()
		{
			// Arrange
			string inputCompanyId = "    validCompanyId    ";

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();

			mockRepository
				.Setup(m => m.Get())
				.Returns(new List<Company>()
				{
					new Company()
					{
						Id = "validCompanyId"
					}
				}.AsQueryable().BuildMock());

			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.GetByIdAsync(inputCompanyId);

			// Assert
			result.Id.Should()
				.Be(inputCompanyId.Trim());
		}

		[Fact]
		public async Task UpdateAsync_CheckIfDTOIdAndInputIdMatch_FailsReturnFalse()
		{
			// Arrange
			CompanyDTO dto = new CompanyDTO() { Id = "dtoId"};
			string inputId = "inputId";

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();


			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.UpdateAsync(dto, inputId);

			// Assert
			mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Company>(), inputId), Times.Never);

			result.Should()
				.BeFalse();
		}

		[Fact]
		public async Task UpdateAsync_CheckIfDTOIdAndInputIdMatch_SucceedsAndReturnTrue()
		{
			// Arrange
			CompanyDTO dto = new CompanyDTO() { Id = "companyId" };
			string inputId = "companyId";

			var mockLogger = new Mock<ILogger<GTCompanyService>>();
			var mockRepository = new Mock<IGTGenericRepository<Company>>();


			var sut = new GTCompanyService(mockLogger.Object, mockRepository.Object);

			// Act
			var result = await sut.UpdateAsync(dto, inputId);

			// Assert
			mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Company>(), inputId), Times.Once);

			result.Should()
				.BeTrue();
		}

		public async Task UpdateAsync_InvalidId_FailsReturnFalse() { }

		public async Task UpdateAsync_ValidDTO_SucceedsReturnTrue() { }
	}
}
