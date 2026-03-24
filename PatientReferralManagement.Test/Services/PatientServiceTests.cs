using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;
using PatientReferralManagementAPI.Services;
using static PatientReferralManagementAPI.Validators.Exceptions;

namespace PatientReferralManagement.Test.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<PatientService>> _loggerMock = new();

        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _service = new PatientService(
                _repoMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePatient()
        {
            var dto = new CreateUpdatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = "2000-01-01"
            };

            var patient = new Patient { PatientId = 1 };
            var response = new PatientResponseDto { PatientId = 1 };

            _repoMock.Setup(x => x.CreateAsync(It.IsAny<Patient>()))
                     .ReturnsAsync(patient);

            _mapperMock.Setup(x => x.Map<PatientResponseDto>(patient))
                       .Returns(response);

            var result = await _service.CreateAsync(dto);

            result.PatientId.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnData()
        {
            var patient = new Patient { PatientId = 1 };
            var response = new PatientResponseDto { PatientId = 1 };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(patient);
            _mapperMock.Setup(x => x.Map<PatientResponseDto>(patient)).Returns(response);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Patient)null);

            Func<Task> act = async () => await _service.GetByIdAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            var patient = new Patient { PatientId = 1 };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(patient);

            await _service.DeleteAsync(1);

            _repoMock.Verify(x => x.DeleteAsync(patient), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Patient)null);

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdate()
        {
            var dto = new CreateUpdatePatientDto
            {
                FirstName = "Updated",
                LastName = "Name",
                DateOfBirth = "2000-01-01"
            };

            var patient = new Patient { PatientId = 1 };
            var updated = new Patient { PatientId = 1 };
            var response = new PatientResponseDto { PatientId = 1 };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(patient);
            _repoMock.Setup(x => x.UpdateAsync(patient)).ReturnsAsync(updated);
            _mapperMock.Setup(x => x.Map<PatientResponseDto>(updated)).Returns(response);

            var result = await _service.UpdateAsync(1, dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Patient)null);

            Func<Task> act = async () => await _service.UpdateAsync(1, new CreateUpdatePatientDto());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedData()
        {
            var data = new List<Patient> { new Patient { PatientId = 1 } };

            _repoMock.Setup(x => x.GetAllAsync(1, 10)).ReturnsAsync(data);
            _repoMock.Setup(x => x.CountAsync()).ReturnsAsync(1);

            _mapperMock.Setup(x => x.Map<IEnumerable<PatientResponseDto>>(data))
                       .Returns(new List<PatientResponseDto>());

            var result = await _service.GetAllAsync(1, 10);

            result.Total.Should().Be(1);
        }
    }
}
