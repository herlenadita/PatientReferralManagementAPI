using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;
using PatientReferralManagementAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PatientReferralManagementAPI.Validators.Exceptions;

namespace PatientReferralManagement.Test.Services
{
    public class ReferralServiceTests
    {
        private readonly Mock<IReferralRepository> _repoMock = new();
        private readonly Mock<IPatientRepository> _patientRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<ReferralService>> _loggerMock = new();

        private readonly ReferralService _service;

        public ReferralServiceTests()
        {
            _service = new ReferralService(
                _repoMock.Object,
                _patientRepoMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCreate_WhenPatientExists()
        {
            var dto = new CreateReferralDto { PatientId = 1 };

            _patientRepoMock.Setup(x => x.GetByIdAsync(1))
                            .ReturnsAsync(new Patient());

            var referral = new Referral { ReferralId = 1 };

            _repoMock.Setup(x => x.CreateAsync(It.IsAny<Referral>()))
                     .ReturnsAsync(referral);

            _mapperMock.Setup(x => x.Map<ReferralResponseDto>(referral))
                       .Returns(new ReferralResponseDto { ReferralId = 1 });

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenPatientNotFound()
        {
            _patientRepoMock.Setup(x => x.GetByIdAsync(1))
                            .ReturnsAsync((Patient)null);

            Func<Task> act = async () => await _service.CreateAsync(new CreateReferralDto { PatientId = 1 });

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturn()
        {
            var referral = new Referral { ReferralId = 1 };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(referral);
            _mapperMock.Setup(x => x.Map<ReferralResponseDto>(referral))
                       .Returns(new ReferralResponseDto());

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Referral)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByPatientIdAsync_ShouldReturnPaged()
        {
            _patientRepoMock.Setup(x => x.GetByIdAsync(1))
                            .ReturnsAsync(new Patient());

            _repoMock.Setup(x => x.GetByPatientIdAsync(1, 1, 10))
                     .ReturnsAsync(new List<Referral>());

            _repoMock.Setup(x => x.CountByPatientIdAsync(1))
                     .ReturnsAsync(0);

            _mapperMock.Setup(x => x.Map<IEnumerable<ReferralResponseDto>>(It.IsAny<IEnumerable<Referral>>()))
                       .Returns(new List<ReferralResponseDto>());

            var result = await _service.GetByPatientIdAsync(1, 1, 10);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByPatientIdAsync_ShouldThrow_WhenPatientNotFound()
        {
            _patientRepoMock.Setup(x => x.GetByIdAsync(1))
                            .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetByPatientIdAsync(1, 1, 10));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdate()
        {
            var referral = new Referral();

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(referral);
            _repoMock.Setup(x => x.UpdateAsync(referral)).ReturnsAsync(referral);

            _mapperMock.Setup(x => x.Map<ReferralResponseDto>(referral))
                       .Returns(new ReferralResponseDto());

            var result = await _service.UpdateAsync(1, new UpdateReferralDto());

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Referral)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.UpdateAsync(1, new UpdateReferralDto()));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            var referral = new Referral();

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(referral);

            await _service.DeleteAsync(1);

            _repoMock.Verify(x => x.DeleteAsync(referral), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Referral)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteAsync(1));
        }
    }
}
