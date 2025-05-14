using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinUI.Model;
using System.Diagnostics;

namespace WinUI.ViewModel
{
    internal class RecommendationSystemViewModel : INotifyPropertyChanged
    {
        private const string NO_SYMPTOM_SELECTED = "None";
        private readonly RecommendationSystemModel _recommendation_system;

        private string _selected_symptom_start = string.Empty;
        private string _selected_discomfort_area = string.Empty;
        private string _selected_symptom_primary = NO_SYMPTOM_SELECTED;
        private string _selected_symptom_secondary = NO_SYMPTOM_SELECTED;
        private string _selected_symptom_tertiary = NO_SYMPTOM_SELECTED;

        private string _doctor_name;
        private string _doctor_department;
        private string _doctor_rating;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendationSystemFormViewModel"/> class.
        /// </summary>
        /// <param name="recommendation_system">The model for the view model.</param>
        public RecommendationSystemViewModel(RecommendationSystemModel recommendation_system)
        {
            this._recommendation_system = recommendation_system;

            this._doctor_name = string.Empty;
            this._doctor_department = string.Empty;
            this._doctor_rating = string.Empty;

            this.symptomStartOptions = new ObservableCollection<string> { "Suddenly", "After Waking Up", "After Incident", "After Meeting Someone", "After Ingestion" };
            this.discomfortAreaOptions = new ObservableCollection<string> { "Head", "Eyes", "Neck", "Stomach", "Chest", "Arm", "Leg", "Back", "Shoulder", "Foot" };
            this.symptomTypeOptions = new ObservableCollection<string> { "Pain", "Numbness", "Inflammation", "Tenderness", "Coloration", "Itching", "Burning", NO_SYMPTOM_SELECTED };

            this.recommendCommand = new RelayCommand(async () => await this.recommendDoctorAsync());
        }

        /// <summary>
        /// This is the event handler for when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets a collection of when did the symptoms start options.
        /// </summary>
        public ObservableCollection<string> symptomStartOptions { get; }

        /// <summary>
        /// Gets a collection of discomfort areas options.
        /// </summary>
        public ObservableCollection<string> discomfortAreaOptions { get; }

        /// <summary>
        /// Gets a collection of symptoms type options.
        /// </summary>
        public ObservableCollection<string> symptomTypeOptions { get; }

        /// <summary>
        /// Gets or sets a string of when did the symptoms start options.
        /// </summary>
        public string selectedSymptomStart
        {
            get => this._selected_symptom_start;
            set
            {
                this._selected_symptom_start = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a string of discomfort areas options.
        /// </summary>
        public string selectedDiscomfortArea
        {
            get => this._selected_discomfort_area;
            set
            {
                this._selected_discomfort_area = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a string of primary symptoms type options.
        /// </summary>
        public string selectedSymptomPrimary
        {
            get => this._selected_symptom_primary;
            set
            {
                this._selected_symptom_primary = value;
                this.OnPropertyChanged();
                this.validateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets a string of secondary symptoms type options.
        /// </summary>
        public string selectedSymptomSecondary
        {
            get => this._selected_symptom_secondary;
            set
            {
                this._selected_symptom_secondary = value;
                this.OnPropertyChanged();
                this.validateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets a string of tertiary symptoms type options.
        /// </summary>
        public string selectedSymptomTertiary
        {
            get => this._selected_symptom_tertiary;
            set
            {
                this._selected_symptom_tertiary = value;
                this.OnPropertyChanged();
                this.validateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets the doctor's name.
        /// </summary>
        public string doctorName
        {
            get => this._doctor_name;
            set
            {
                this._doctor_name = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the doctor's department.
        /// </summary>
        public string doctorDepartment
        {
            get => this._doctor_department;
            set
            {
                this._doctor_department = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the doctor's rating.
        /// </summary>
        public string doctorRating
        {
            get => this._doctor_rating;
            set
            {
                this._doctor_rating = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the command when the recommend doctor button is clicked.
        /// </summary>
        public ICommand recommendCommand { get; }

        /// <summary>
        /// Calls the same method from the model.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task<RecommendationSystemDoctorJointModel?> recommendDoctorBasedOnSymptomsAsync()
        {
            if (!this.validateSymptomSelection())
            {
                return null;
            }

            return await this._recommendation_system.recommendDoctorAsynchronous(this);
        }

        /// <summary>
        /// Validator for symptoms.
        /// </summary>
        /// <returns>True if symptoms are valid False otherwise.</returns>
        public bool validateSymptomSelection()
        {
            var symptoms = new List<string?>
            {
                this.selectedSymptomStart,
                this.selectedDiscomfortArea,
                this.selectedSymptomPrimary,
                this.selectedSymptomSecondary,
                this.selectedSymptomTertiary,
            };

            List<string?> non_empty_symptoms = symptoms.Where(s => !string.IsNullOrEmpty(s)).ToList();
            return non_empty_symptoms.Distinct().Count() == non_empty_symptoms.Count;
        }

        private async Task recommendDoctorAsync()
        {
            Debug.WriteLine("Starting doctor recommendation...");
            var doctor = await this._recommendation_system.recommendDoctorAsynchronous(this);
            Debug.WriteLine($"Received doctor: {doctor?.ToString() ?? "null"}");

            if (doctor != null)
            {
                string doctor_name = doctor.getDoctorName();
                string department = doctor.getDoctorDepartment();
                double rating = doctor.getDoctorRating();
                
                Debug.WriteLine($"Doctor details - Name: {doctor_name}, Department: {department}, Rating: {rating}");
                
                this.doctorName = $"Doctor: {doctor_name}";
                this.doctorDepartment = $"Department: {department}";
                this.doctorRating = $"Rating: {rating:0.0}";
                
                Debug.WriteLine($"Updated UI - Name: {this.doctorName}, Department: {this.doctorDepartment}, Rating: {this.doctorRating}");
            }
            else
            {
                Debug.WriteLine("No doctor found");
                this.doctorName = "No suitable doctor found";
                this.doctorDepartment = string.Empty;
                this.doctorRating = string.Empty;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
