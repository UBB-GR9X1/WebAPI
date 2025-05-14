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
        private enum Department
        {
            Cardiology = 1,
            Neurology = 2,
            Pediatrics = 3,
            Ophthalmology = 4,
            Gastroenterology = 5,
            Orthopedics = 6,
            Dermatology = 7
        }

        private readonly RecommendationSystemService _doctor_service;
        private Dictionary<string, Dictionary<int, int>> _symptom_to_department_score_mapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendationSystemModel"/> class.
        /// </summary>
        /// <param name="doctor_service">The doctor service containing all methods needed to find the optimal department.</param>
        public RecommendationSystemModel(RecommendationSystemService doctor_service)
        {
            this._doctor_service = doctor_service;
            this._symptom_to_department_score_mapping = new Dictionary<string, Dictionary<int, int>>();
            this.initializeSymptomToDepartmentScores();
        }

        /// <summary>
        /// Method to recommend doctors.
        /// </summary>
        /// <param name="view_model">The view model for the model.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<RecommendationSystemDoctorJointModel?> recommendDoctorAsynchronous(ViewModel.RecommendationSystemViewModel view_model)
        {
            Dictionary<int, int> department_scores = new Dictionary<int, int>();

            void addSymptomScore(string symptom)
            {
                if (string.IsNullOrWhiteSpace(symptom))
                {
                    return;
                }

                if (this._symptom_to_department_score_mapping.TryGetValue(symptom.Trim(), out var scores))
                {
                    foreach (var key_value_pair in scores)
                    {
                        if (!department_scores.ContainsKey(key_value_pair.Key))
                        {
                            department_scores[key_value_pair.Key] = 0;
                        }

                        department_scores[key_value_pair.Key] += key_value_pair.Value;
                    }
                }
            }

            addSymptomScore(view_model.selectedSymptomStart);
            addSymptomScore(view_model.selectedDiscomfortArea);
            addSymptomScore(view_model.selectedSymptomPrimary);
            addSymptomScore(view_model.selectedSymptomSecondary);
            addSymptomScore(view_model.selectedSymptomTertiary);

            if (department_scores.Count == 0)
            {
                return null;
            }

            int best_department_id = department_scores.OrderByDescending(department => department.Value).First().Key;
            var doctors = await this._doctor_service.getDoctorsByDepartment(best_department_id);

            return doctors
                .OrderByDescending(doctor => doctor.getRegistrationDate())
                .ThenBy(doctor => doctor.getBirthDate())
                .ThenBy(doctor => doctor.getDoctorRating())
                .FirstOrDefault();
        }

        private void initializeSymptomToDepartmentScores()
        {
            this._symptom_to_department_score_mapping = new Dictionary<string, Dictionary<int, int>>
            {
                { "Suddenly", new Dictionary<int, int> { { (int)Department.Cardiology, 3 }, { (int)Department.Neurology, 3 }, { (int)Department.Pediatrics, 2 } } },
                { "After Waking Up", new Dictionary<int, int> { { (int)Department.Neurology, 0 }, { (int)Department.Gastroenterology, 3 } } },
                { "After Incident", new Dictionary<int, int> { { (int)Department.Orthopedics, 4 }, { (int)Department.Neurology, 3 } } },
                { "After Meeting Someone", new Dictionary<int, int> { { (int)Department.Dermatology, 5 }, { (int)Department.Ophthalmology, 2 } } },
                { "After Ingestion", new Dictionary<int, int> { { (int)Department.Gastroenterology, 4 }, { (int)Department.Pediatrics, 4 }, { (int)Department.Cardiology, 2 } } },
                { "Eyes", new Dictionary<int, int> { { (int)Department.Ophthalmology, 5 }, { (int)Department.Neurology, 3 }, { (int)Department.Dermatology, 2 } } },
                { "Head", new Dictionary<int, int> { { (int)Department.Neurology, 5 }, { (int)Department.Cardiology, 4 }, { (int)Department.Pediatrics, 3 } } },
                { "Chest", new Dictionary<int, int> { { (int)Department.Cardiology, 6 }, { (int)Department.Neurology, 0 }, { (int)Department.Gastroenterology, 2 } } },
                { "Stomach", new Dictionary<int, int> { { (int)Department.Gastroenterology, 5 }, { (int)Department.Pediatrics, 4 }, { (int)Department.Neurology, 2 } } },
                { "Arm", new Dictionary<int, int> { { (int)Department.Orthopedics, 5 }, { (int)Department.Dermatology, 3 }, { (int)Department.Neurology, 2 } } },
                { "Leg", new Dictionary<int, int> { { (int)Department.Orthopedics, 5 }, { (int)Department.Dermatology, 3 }, { (int)Department.Cardiology, 2 } } },
                { "Pain", new Dictionary<int, int> { { (int)Department.Cardiology, 4 }, { (int)Department.Orthopedics, 4 }, { (int)Department.Neurology, 0 } } },
                { "Numbness", new Dictionary<int, int> { { (int)Department.Neurology, 5 }, { (int)Department.Orthopedics, 3 }, { (int)Department.Cardiology, 2 } } },
                { "Inflammation", new Dictionary<int, int> { { (int)Department.Dermatology, 4 }, { (int)Department.Gastroenterology, 4 }, { (int)Department.Ophthalmology, 2 } } },
                { "Tenderness", new Dictionary<int, int> { { (int)Department.Orthopedics, 4 }, { (int)Department.Gastroenterology, 3 }, { (int)Department.Neurology, 2 } } },
                { "Coloration", new Dictionary<int, int> { { (int)Department.Dermatology, 5 }, { (int)Department.Ophthalmology, 4 }, { (int)Department.Neurology, 1 } } },
            };
        }
    }
}
