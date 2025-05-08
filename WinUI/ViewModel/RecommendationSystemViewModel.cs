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

            this.SymptomStartOptions = new ObservableCollection<string> { "Suddenly", "After Waking Up", "After Incident", "After Meeting Someone", "After Ingestion" };
            this.DiscomfortAreaOptions = new ObservableCollection<string> { "Head", "Eyes", "Neck", "Stomach", "Chest", "Arm", "Leg", "Back", "Shoulder", "Foot" };
            this.SymptomTypeOptions = new ObservableCollection<string> { "Pain", "Numbness", "Inflammation", "Tenderness", "Coloration", "Itching", "Burning", NO_SYMPTOM_SELECTED };

            this.RecommendCommand = new RelayCommand(async () => await this.RecommendDoctorAsync());
        }

        /// <summary>
        /// This is the event handler for when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets a collection of when did the symptoms start options.
        /// </summary>
        public ObservableCollection<string> SymptomStartOptions { get; }

        /// <summary>
        /// Gets a collection of discomfort areas options.
        /// </summary>
        public ObservableCollection<string> DiscomfortAreaOptions { get; }

        /// <summary>
        /// Gets a collection of symptoms type options.
        /// </summary>
        public ObservableCollection<string> SymptomTypeOptions { get; }

        /// <summary>
        /// Gets or sets a string of when did the symptoms start options.
        /// </summary>
        public string SelectedSymptomStart
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
        public string SelectedDiscomfortArea
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
        public string SelectedSymptomPrimary
        {
            get => this._selected_symptom_primary;
            set
            {
                this._selected_symptom_primary = value;
                this.OnPropertyChanged();
                this.ValidateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets a string of secondary symptoms type options.
        /// </summary>
        public string SelectedSymptomSecondary
        {
            get => this._selected_symptom_secondary;
            set
            {
                this._selected_symptom_secondary = value;
                this.OnPropertyChanged();
                this.ValidateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets a string of tertiary symptoms type options.
        /// </summary>
        public string SelectedSymptomTertiary
        {
            get => this._selected_symptom_tertiary;
            set
            {
                this._selected_symptom_tertiary = value;
                this.OnPropertyChanged();
                this.ValidateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets the doctor's name.
        /// </summary>
        public string DoctorName
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
        public string DoctorDepartment
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
        public string DoctorRating
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
        public ICommand RecommendCommand { get; }

        /// <summary>
        /// Calls the same method from the model.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task<RecommendationSystemDoctorJointModel?> RecommendDoctorBasedOnSymptomsAsync()
        {
            if (!this.ValidateSymptomSelection())
            {
                return null;
            }

            return await this._recommendation_system.RecommendDoctorAsynchronous(this);
        }

        /// <summary>
        /// Validator for symptoms.
        /// </summary>
        /// <returns>True if symptoms are valid False otherwise.</returns>
        public bool ValidateSymptomSelection()
        {
            var symptoms = new List<string?>
            {
                this.SelectedSymptomStart,
                this.SelectedDiscomfortArea,
                this.SelectedSymptomPrimary,
                this.SelectedSymptomSecondary,
                this.SelectedSymptomTertiary,
            };

            var non_empty_symptoms = symptoms.Where(s => !string.IsNullOrEmpty(s)).ToList();
            return non_empty_symptoms.Distinct().Count() == non_empty_symptoms.Count;
        }

        private async Task RecommendDoctorAsync()
        {
            Debug.WriteLine("Starting doctor recommendation...");
            var doctor = await this._recommendation_system.RecommendDoctorAsynchronous(this);
            Debug.WriteLine($"Received doctor: {doctor?.ToString() ?? "null"}");

            if (doctor != null)
            {
                var doctor_name = doctor.GetDoctorName();
                var department = doctor.GetDoctorDepartment();
                var rating = doctor.GetDoctorRating();
                
                Debug.WriteLine($"Doctor details - Name: {doctor_name}, Department: {department}, Rating: {rating}");
                
                this.DoctorName = $"Doctor: {doctor_name}";
                this.DoctorDepartment = $"Department: {department}";
                this.DoctorRating = $"Rating: {rating:0.0}";
                
                Debug.WriteLine($"Updated UI - Name: {this.DoctorName}, Department: {this.DoctorDepartment}, Rating: {this.DoctorRating}");
            }
            else
            {
                Debug.WriteLine("No doctor found");
                this.DoctorName = "No suitable doctor found";
                this.DoctorDepartment = string.Empty;
                this.DoctorRating = string.Empty;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
