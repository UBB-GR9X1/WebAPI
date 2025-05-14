using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinUI.Service;
using WinUI.Repository;
using ClassLibrary.Domain;

namespace WinUi_Test.Tests
{
    [TestClass]
    public class PatientServiceTests
    {
        private Mock<IPatientRepository> _mockRepository;
        private PatientService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IPatientRepository>();
            _service = new PatientService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task updateWeight_validPatient_updatesSuccessfully()
        {
            int userId = 1;
            var patient = new Patient { userId = userId };

            _mockRepository.Setup(r => r.getPatientByUserIdAsync(userId)).ReturnsAsync(patient);
            _mockRepository.Setup(r => r.addPatientAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

            var result = await _service.updateWeight(userId, 85);

            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.addPatientAsync(It.Is<Patient>(p => p.weight == 85)), Times.Once);
        }

        [TestMethod]
        public async Task updateHeight_invalidPatient_returnsFalse()
        {
            int userId = 2;

            _mockRepository.Setup(r => r.getPatientByUserIdAsync(userId)).ReturnsAsync((Patient)null);

            var result = await _service.updateHeight(userId, 180);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task updateEmergencyContact_validPatient_updatesSuccessfully()
        {
            int userId = 3;
            var patient = new Patient { userId = userId };

            _mockRepository.Setup(r => r.getPatientByUserIdAsync(userId)).ReturnsAsync(patient);
            _mockRepository.Setup(r => r.addPatientAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

            var result = await _service.updateEmergencyContact(userId, "123-456-7890");

            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.addPatientAsync(It.Is<Patient>(p => p.EmergencyContact == "123-456-7890")), Times.Once);
        }
    }
}
