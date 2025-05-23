using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using ClassLibrary.Repository;
using WinUI.Model;
using WinUI.Service;

namespace WinUi_Test
{
    [TestClass]
    public class PatientServiceTests
    {
        private Mock<IPatientRepository> _mockRepo;
        private Mock<ILoggerService> _mockLogger;
        private PatientService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPatientRepository>();
            _mockLogger = new Mock<ILoggerService>();
            _service = new PatientService(_mockRepo.Object, _mockLogger.Object);
        }

        [TestMethod]
        [TestCategory("PatientService")]
        [Description("Should load patient info when data is valid")]
        public async Task LoadPatientInfoByUserId_ValidData_ReturnsTrue()
        {
            var patient = new Patient { userId = 1 };
            var user = new User { userId = 1, name = "John", username = "john123", password = "pass", mail = "mail@test.com", birthDate = DateOnly.MinValue, cnp = "000", address = "addr", phoneNumber = "123", registrationDate = DateTime.Now, role = "user" };

            _mockRepo.Setup(r => r.getPatientByUserIdAsync(1)).ReturnsAsync(patient);
            _mockRepo.Setup(r => r.getAllUserAsync()).ReturnsAsync(new List<User> { user });

            var result = await _service.loadPatientInfoByUserId(1);

            Assert.IsTrue(result);
            Assert.AreEqual(1, _service.patientInfo.userId);
            Assert.AreEqual("John", _service.patientInfo.patientName);
        }

        [TestMethod]
        [TestCategory("PatientService")]
        [Description("Should return false when patient or user is null")]
        public async Task LoadPatientInfoByUserId_NullData_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.getPatientByUserIdAsync(1)).ReturnsAsync((Patient)null);
            _mockRepo.Setup(r => r.getAllUserAsync()).ReturnsAsync(new List<User>());

            var result = await _service.loadPatientInfoByUserId(1);

            Assert.IsFalse(result);
            Assert.AreEqual(PatientJointModel.Default.userId, _service.patientInfo.userId);
        }

        [TestMethod]
        [TestCategory("PatientService")]
        [Description("Should load all valid patients into patientList")]
        public async Task LoadAllPatients_ValidData_PopulatesList()
        {
            var patients = new List<Patient> { new Patient { userId = 1 } };
            var users = new List<User> { new User { userId = 1, name = "Test", username = "u", password = "p", mail = "m", birthDate = DateOnly.MinValue, cnp = "c", address = "a", phoneNumber = "123", registrationDate = DateTime.Now, role = "r" } };

            _mockRepo.Setup(r => r.getAllPatientsAsync()).ReturnsAsync(patients);
            _mockRepo.Setup(r => r.getAllUserAsync()).ReturnsAsync(users);

            var result = await _service.loadAllPatients();

            Assert.IsTrue(result);
            Assert.AreEqual(1, _service.patientList.Count);
        }

        [TestMethod]
        [TestCategory("PatientService")]
        [Description("Should update name when patient and user are found")]
        public async Task UpdateName_ValidUser_ReturnsTrue()
        {
            var patient = new Patient { userId = 1 };
            var user = new User { userId = 1, name = "Old Name" };

            _mockRepo.Setup(r => r.getPatientByUserIdAsync(1)).ReturnsAsync(patient);
            _mockRepo.Setup(r => r.getAllUserAsync()).ReturnsAsync(new List<User> { user });
            _mockRepo.Setup(r => r.updatePatientAsync(patient, user)).Returns(Task.CompletedTask);

            var result = await _service.updateName(1, "New Name");

            Assert.IsTrue(result);
            Assert.AreEqual("New Name", user.name);
        }

        [TestMethod]
        [TestCategory("PatientService")]
        [Description("Should return false when updating name but user not found")]
        public async Task UpdateName_UserNotFound_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.getPatientByUserIdAsync(1)).ReturnsAsync((Patient)null);
            _mockRepo.Setup(r => r.getAllUserAsync()).ReturnsAsync(new List<User>());

            var result = await _service.updateName(1, "Name");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("PatientService")]
        [Description("Should call logAction and return its result")]
        public async Task LogUpdate_CallsLoggerService_ReturnsTrue()
        {
            _mockLogger.Setup(l => l.logAction(1, ActionType.LOGIN)).ReturnsAsync(true);

            var result = await _service.logUpdate(1, ActionType.LOGIN);

            Assert.IsTrue(result);
            _mockLogger.Verify(l => l.logAction(1, ActionType.LOGIN), Times.Once);
        }
    }
}
