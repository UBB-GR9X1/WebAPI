using ClassLibrary.Domain;
using ClassLibrary.Repository;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebApiTests;

[TestClass]
public class PatientControllerTest
{
    [TestMethod]
    public async Task getAllPatients_withValidController_returnsListOfPatients()
    {
        // Arrange
        var _mock_repo = new Mock<IPatientRepository>();
        var _fake_patients = new List<Patient>
        {
            new Patient { userId = 1, bloodType = "AB+", EmergencyContact = "Mom", allergies = "cats, dogs", weight = 72.6, height = 167 },
            new Patient { userId = 2, bloodType = "A-", EmergencyContact = "Dad", allergies = "cats, dogs", weight = 92.6, height = 199 }
        };
        _mock_repo.Setup(_repo => _repo.getAllPatientsAsync()).ReturnsAsync(_fake_patients);

        var _controller = new PatientController(_mock_repo.Object);

        // Act
        var _result = await _controller.getAllPatients();
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_patients = _ok_result.Value as List<Patient>;
        Assert.AreEqual(2, _returned_patients.Count);
    }

    [TestMethod]
    public async Task getPatientById_withValidPatientId_returnsPatient()
    {
        // Arrange
        var _patient_id = 2;
        var _mock_repo = new Mock<IPatientRepository>();
        var _fake_patient = new Patient { userId = 2, bloodType = "A-", EmergencyContact = "Dad", allergies = "cats, dogs", weight = 92.6, height = 199 };

        _mock_repo.Setup(_repo => _repo.getPatientByUserIdAsync(_patient_id)).ReturnsAsync(_fake_patient);

        var _controller = new PatientController(_mock_repo.Object);

        // Act
        var _result = await _controller.getPatientById(_patient_id);
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_patient = _ok_result.Value as Patient;
        Assert.AreEqual(2, _returned_patient.userId);
    }

    [TestMethod]
    public async Task createPatient_withValidPatient_returnsCreatedAtAction()
    {
        // Arrange
        var _mock_repo = new Mock<IPatientRepository>();
        var _fake_patient = new Patient { userId = 2, bloodType = "A-", EmergencyContact = "Dad", allergies = "cats, dogs", weight = 92.6, height = 199 };
        _mock_repo.Setup(repo => repo.addPatientAsync(_fake_patient)).Returns(Task.CompletedTask);

        var _controller = new PatientController(_mock_repo.Object);

        // Act
        var _result = await _controller.createPatient(_fake_patient);
        var _created_at = _result as CreatedAtActionResult;

        // Assert
        Assert.IsNotNull(_created_at);
        Assert.AreEqual(_fake_patient.userId, ((Patient)_created_at.Value).userId);
    }

    [TestMethod]
    public async Task deletePatient_withValidPatientId_returnsNoContent()
    {
        // Arrange
        var _mock_repo = new Mock<IPatientRepository>();
        int _patient_id = 1;

        // No setup needed if the method succeeds (completes without exception)
        _mock_repo.Setup(_repo => _repo.deletePatientAsync(_patient_id)).Returns(Task.CompletedTask);

        var _controller = new PatientController(_mock_repo.Object);

        // Act
        var _result = await _controller.deletePatient(_patient_id);

        // Assert
        Assert.IsInstanceOfType(_result, typeof(NoContentResult));
        _mock_repo.Verify(_repo => _repo.deletePatientAsync(_patient_id), Times.Once);
    }
}
