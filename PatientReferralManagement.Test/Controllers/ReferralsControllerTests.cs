using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PatientReferralManagementAPI.Controllers;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;
using PatientReferralManagementAPI.Services;
using static PatientReferralManagementAPI.Validators.Exceptions;

namespace PatientReferralManagement.Test.Controllers
{
    public class ReferralsControllerTests
    {
        private readonly Mock<IReferralRepository> _repoMock = new();
        private readonly Mock<IPatientRepository> _patientRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<ReferralService>> _loggerMock = new();

        private readonly ReferralService _service;
        private readonly ReferralsController _controller;

        public ReferralsControllerTests()
        {
            _service = new ReferralService(
                _repoMock.Object,
                _patientRepoMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );

            _controller = new ReferralsController(_service);
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            _patientRepoMock.Setup(x => x.GetByIdAsync(1))
                            .ReturnsAsync(new Patient());

            _repoMock.Setup(x => x.CreateAsync(It.IsAny<Referral>()))
                     .ReturnsAsync(new Referral());

            _mapperMock.Setup(x => x.Map<ReferralResponseDto>(It.IsAny<Referral>()))
                       .Returns(new ReferralResponseDto());

            var result = await _controller.Create(new CreateReferralDto { PatientId = 1 });

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Create_ShouldThrow_WhenPatientNotFound()
        {
            _patientRepoMock.Setup(x => x.GetByIdAsync(1))
                            .ReturnsAsync((Patient)null);

            Func<Task> act = async () =>
                await _controller.Create(new CreateReferralDto { PatientId = 1 });

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetById_ShouldReturnOk()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync(new Referral());

            _mapperMock.Setup(x => x.Map<ReferralResponseDto>(It.IsAny<Referral>()))
                       .Returns(new ReferralResponseDto());

            var result = await _controller.GetById(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetById_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync((Referral)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _controller.GetById(1));
        }

        [Fact]
        public async Task GetAll_ShouldReturnPaged()
        {
            _repoMock.Setup(x => x.GetAllAsync(1, 10))
                     .ReturnsAsync(new List<Referral>());

            _repoMock.Setup(x => x.CountAsync()).ReturnsAsync(0);

            _mapperMock.Setup(x => x.Map<IEnumerable<ReferralResponseDto>>(It.IsAny<IEnumerable<Referral>>()))
                       .Returns(new List<ReferralResponseDto>());

            var result = await _controller.GetAll(1, 10);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync(new Referral());

            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Referral>()))
                     .ReturnsAsync(new Referral());

            _mapperMock.Setup(x => x.Map<ReferralResponseDto>(It.IsAny<Referral>()))
                       .Returns(new ReferralResponseDto());

            var result = await _controller.Update(1, new UpdateReferralDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Update_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync((Referral)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _controller.Update(1, new UpdateReferralDto()));
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync(new Referral());

            var result = await _controller.Delete(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Delete_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync((Referral)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _controller.Delete(1));
        }
    }
}
