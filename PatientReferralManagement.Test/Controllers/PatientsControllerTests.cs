using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PatientReferralManagementAPI.Controllers;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;
using PatientReferralManagementAPI.Services;
using static PatientReferralManagementAPI.Validators.Exceptions;

namespace PatientReferralManagement.Test.Controllers
{
    public class PatientControllersTests
    {
        private CreateUpdatePatientDto ValidPatientDto() => new()
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = "2000-01-01"
        };

        private CreateReferralDto ValidReferralDto(int patientId = 1) => new()
        {
            PatientId = patientId,
            ReferralSource = "Doctor",
            ReferralType = "General",
            ReferralNote = "Test"
        };

        private Patient PatientEntity(int id = 1) => new()
        {
            PatientId = id,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.Parse("2000-01-01")
        };

        private Referral ReferralEntity(int id = 1) => new()
        {
            ReferralId = id,
            PatientId = 1,
            ReferralSource = "Doctor",
            ReferralType = "General",
            ReferralNote = "Test",
            CreatedDate = DateTime.UtcNow
        };

        [Fact]
        public async Task Patient_Create_ShouldReturnCreated()
        {
            var repoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PatientService>>();

            var service = new PatientService(repoMock.Object, mapperMock.Object, loggerMock.Object);

            var referralService = new Mock<ReferralService>(null, null, null, null).Object;
            var controller = new PatientsController(service, referralService);

            var dto = ValidPatientDto();
            var patient = PatientEntity();

            repoMock.Setup(x => x.CreateAsync(It.IsAny<Patient>()))
                    .ReturnsAsync(patient);

            mapperMock.Setup(x => x.Map<PatientResponseDto>(patient))
                      .Returns(new PatientResponseDto { PatientId = 1 });

            var result = await controller.Create(dto);

            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task Patient_GetById_ShouldReturnOk()
        {
            var repoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PatientService>>();

            var service = new PatientService(repoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new PatientsController(service, new Mock<ReferralService>(null, null, null, null).Object);

            var patient = PatientEntity();

            repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(patient);
            mapperMock.Setup(x => x.Map<PatientResponseDto>(patient)).Returns(new PatientResponseDto());

            var result = await controller.GetById(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Patient_GetById_ShouldThrow_WhenNotFound()
        {
            var repoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PatientService>>();

            var service = new PatientService(repoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new PatientsController(service, new Mock<ReferralService>(null, null, null, null).Object);

            repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Patient)null);

            Func<Task> act = async () => await controller.GetById(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Patient_Update_ShouldReturnOk()
        {
            var repoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PatientService>>();

            var service = new PatientService(repoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new PatientsController(service, new Mock<ReferralService>(null, null, null, null).Object);

            var patient = PatientEntity();

            repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(patient);
            repoMock.Setup(x => x.UpdateAsync(patient)).ReturnsAsync(patient);

            mapperMock.Setup(x => x.Map<PatientResponseDto>(patient))
                      .Returns(new PatientResponseDto());

            var result = await controller.Update(1, ValidPatientDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Patient_Delete_ShouldReturnOk()
        {
            var repoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PatientService>>();

            var service = new PatientService(repoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new PatientsController(service, new Mock<ReferralService>(null, null, null, null).Object);

            repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(PatientEntity());

            var result = await controller.Delete(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        // ======================================================
        // REFERRAL CONTROLLER TEST
        // ======================================================
        [Fact]
        public async Task Referral_Create_ShouldReturnOk()
        {
            var repoMock = new Mock<IReferralRepository>();
            var patientRepoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReferralService>>();

            var service = new ReferralService(repoMock.Object, patientRepoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new ReferralsController(service);

            patientRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(PatientEntity());
            repoMock.Setup(x => x.CreateAsync(It.IsAny<Referral>())).ReturnsAsync(ReferralEntity());

            mapperMock.Setup(x => x.Map<ReferralResponseDto>(It.IsAny<Referral>()))
                      .Returns(new ReferralResponseDto());

            var result = await controller.Create(ValidReferralDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Referral_Create_ShouldThrow_WhenPatientNotFound()
        {
            var repoMock = new Mock<IReferralRepository>();
            var patientRepoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReferralService>>();

            var service = new ReferralService(repoMock.Object, patientRepoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new ReferralsController(service);

            patientRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Patient)null);

            Func<Task> act = async () => await controller.Create(ValidReferralDto());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Referral_GetById_ShouldReturnOk()
        {
            var repoMock = new Mock<IReferralRepository>();
            var patientRepoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReferralService>>();

            var service = new ReferralService(repoMock.Object, patientRepoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new ReferralsController(service);

            repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(ReferralEntity());
            mapperMock.Setup(x => x.Map<ReferralResponseDto>(It.IsAny<Referral>()))
                      .Returns(new ReferralResponseDto());

            var result = await controller.GetById(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Referral_Delete_ShouldReturnOk()
        {
            var repoMock = new Mock<IReferralRepository>();
            var patientRepoMock = new Mock<IPatientRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReferralService>>();

            var service = new ReferralService(repoMock.Object, patientRepoMock.Object, mapperMock.Object, loggerMock.Object);
            var controller = new ReferralsController(service);

            repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(ReferralEntity());

            var result = await controller.Delete(1);

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
