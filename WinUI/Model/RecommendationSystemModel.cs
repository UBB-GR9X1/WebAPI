using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Service;

namespace WinUI.Model
{
    internal class RecommendationSystemModel
    {
        private const int Cardiology = 1;
        private const int Neurology = 2;
        private const int Pediatrics = 3;
        private const int Ophthalmology = 4;
        private const int Gastroenterology = 5;
        private const int Orthopedics = 6;
        private const int Dermatology = 7;

        private readonly RecommendationSystemService doctorService;
        private Dictionary<string, Dictionary<int, int>> symptomToDepartmentScoreMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendationSystemModel"/> class.
        /// </summary>
        /// <param name="doctorService">The doctor service containing all methods needed to find the optimal department.</param>
        public RecommendationSystemModel(RecommendationSystemService doctorService)
        {
            this.doctorService = doctorService;
            this.symptomToDepartmentScoreMapping = new Dictionary<string, Dictionary<int, int>>();
            this.InitializeSymptomToDepartmentScores();
        }

        /// <summary>
        /// Method to recommend doctors.
        /// </summary>
        /// <param name="viewModel">The view model for the model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<RecommendationSystemDoctorJointModel?> RecommendDoctorAsynchronous(ViewModel.RecommendationSystemViewModel viewModel)
        {
            Dictionary<int, int> departmentScores = new Dictionary<int, int>();

            void AddSymptomScore(string symptom)
            {
                if (string.IsNullOrWhiteSpace(symptom))
                {
                    return;
                }

                if (this.symptomToDepartmentScoreMapping.TryGetValue(symptom.Trim(), out var scores))
                {
                    foreach (var keyValuePair in scores)
                    {
                        if (!departmentScores.ContainsKey(keyValuePair.Key))
                        {
                            departmentScores[keyValuePair.Key] = 0;
                        }

                        departmentScores[keyValuePair.Key] += keyValuePair.Value;
                    }
                }
            }

            AddSymptomScore(viewModel.SelectedSymptomStart);
            AddSymptomScore(viewModel.SelectedDiscomfortArea);
            AddSymptomScore(viewModel.SelectedSymptomPrimary);
            AddSymptomScore(viewModel.SelectedSymptomSecondary);
            AddSymptomScore(viewModel.SelectedSymptomTertiary);

            if (departmentScores.Count == 0)
            {
                return null;
            }

            int bestDepartmentId = departmentScores.OrderByDescending(department => department.Value).First().Key;
            var doctors = await this.doctorService.GetDoctorsByDepartment(bestDepartmentId);

            return doctors
                .OrderByDescending(doctor => doctor.GetRegistrationDate())
                .ThenBy(doctor => doctor.GetBirthDate())
                .ThenBy(doctor => doctor.GetDoctorRating())
                .FirstOrDefault();
        }
        private void InitializeSymptomToDepartmentScores()
        {
            this.symptomToDepartmentScoreMapping = new Dictionary<string, Dictionary<int, int>>
            {
                { "Suddenly", new Dictionary<int, int> { { Cardiology, 3 }, { Neurology, 3 }, { Pediatrics, 2 } } },
                { "After Waking Up", new Dictionary<int, int> { { Neurology, 0 }, { Gastroenterology, 3 } } },
                { "After Incident", new Dictionary<int, int> { { Orthopedics, 4 }, { Neurology, 3 } } },
                { "After Meeting Someone", new Dictionary<int, int> { { Dermatology, 5 }, { Ophthalmology, 2 } } },
                { "After Ingestion", new Dictionary<int, int> { { Gastroenterology, 4 }, { Pediatrics, 4 }, { Cardiology, 2 } } },
                { "Eyes", new Dictionary<int, int> { { Ophthalmology, 5 }, { Neurology, 3 }, { Dermatology, 2 } } },
                { "Head", new Dictionary<int, int> { { Neurology, 5 }, { Cardiology, 4 }, { Pediatrics, 3 } } },
                { "Chest", new Dictionary<int, int> { { Cardiology, 6 }, { Neurology, 0 }, { Gastroenterology, 2 } } },
                { "Stomach", new Dictionary<int, int> { { Gastroenterology, 5 }, { Pediatrics, 4 }, { Neurology, 2 } } },
                { "Arm", new Dictionary<int, int> { { Orthopedics, 5 }, { Dermatology, 3 }, { Neurology, 2 } } },
                { "Leg", new Dictionary<int, int> { { Orthopedics, 5 }, { Dermatology, 3 }, { Cardiology, 2 } } },
                { "Pain", new Dictionary<int, int> { { Cardiology, 4 }, { Orthopedics, 4 }, { Neurology, 0 } } },
                { "Numbness", new Dictionary<int, int> { { Neurology, 5 }, { Orthopedics, 3 }, { Cardiology, 2 } } },
                { "Inflammation", new Dictionary<int, int> { { Dermatology, 4 }, { Gastroenterology, 4 }, { Ophthalmology, 2 } } },
                { "Tenderness", new Dictionary<int, int> { { Orthopedics, 4 }, { Gastroenterology, 3 }, { Neurology, 2 } } },
                { "Coloration", new Dictionary<int, int> { { Dermatology, 5 }, { Ophthalmology, 4 }, { Neurology, 1 } } },
            };
        }
    }
}
