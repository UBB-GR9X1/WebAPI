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

namespace WinUI.ViewModel
{
    internal class RecommendationSystemViewModel : INotifyPropertyChanged
    {
        private const string NoSymptomSelected = "None";
        private readonly RecommendationSystemModel recommendationSystem;

        private string selectedSymptomStart = string.Empty;
        private string selectedDiscomfortArea = string.Empty;
        private string selectedSymptomPrimary = NoSymptomSelected;
        private string selectedSymptomSecondary = NoSymptomSelected;
        private string selectedSymptomTertiary = NoSymptomSelected;

        private string doctorName;
        private string doctorDepartment;
        private string doctorRating;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendationSystemFormViewModel"/> class.
        /// </summary>
        /// <param name="recommendationSystem">The model for the view model.</param>
        public RecommendationSystemViewModel(RecommendationSystemModel recommendationSystem)
        {
            this.recommendationSystem = recommendationSystem;

            this.doctorName = string.Empty;
            this.doctorDepartment = string.Empty;
            this.doctorRating = string.Empty;

            this.SymptomStartOptions = new ObservableCollection<string> { "Suddenly", "After Waking Up", "After Incident", "After Meeting Someone", "After Ingestion" };
            this.DiscomfortAreaOptions = new ObservableCollection<string> { "Head", "Eyes", "Neck", "Stomach", "Chest", "Arm", "Leg", "Back", "Shoulder", "Foot" };
            this.SymptomTypeOptions = new ObservableCollection<string> { "Pain", "Numbness", "Inflammation", "Tenderness", "Coloration", "Itching", "Burning", NoSymptomSelected };

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
            get => this.selectedSymptomStart; set
            {
                this.selectedSymptomStart = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a string of discomfort areas options.
        /// </summary>
        public string SelectedDiscomfortArea
        {
            get => this.selectedDiscomfortArea; set
            {
                this.selectedDiscomfortArea = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a string of primary symptoms type options.
        /// </summary>
        public string SelectedSymptomPrimary
        {
            get => this.selectedSymptomPrimary; set
            {
                this.selectedSymptomPrimary = value;
                this.OnPropertyChanged();
                this.ValidateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets a string of secondary symptoms type options.
        /// </summary>
        public string SelectedSymptomSecondary
        {
            get => this.selectedSymptomSecondary; set
            {
                this.selectedSymptomSecondary = value;
                this.OnPropertyChanged();
                this.ValidateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets a string of tertiary symptoms type options.
        /// </summary>
        public string SelectedSymptomTertiary
        {
            get => this.selectedSymptomTertiary; set
            {
                this.selectedSymptomTertiary = value;
                this.OnPropertyChanged();
                this.ValidateSymptomSelection();
            }
        }

        /// <summary>
        /// Gets or sets the doctor's name.
        /// </summary>
        public string DoctorName
        {
            get => this.doctorName; set
            {
                this.doctorName = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the doctor's department.
        /// </summary>
        public string DoctorDepartment
        {
            get => this.doctorDepartment; set
            {
                this.doctorDepartment = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the doctor's rating.
        /// </summary>
        public string DoctorRating
        {
            get => this.doctorRating; set
            {
                this.doctorRating = value;
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

            return await this.recommendationSystem.RecommendDoctorAsynchronous(this);
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

            var nonEmptySymptoms = symptoms.Where(s => !string.IsNullOrEmpty(s)).ToList();
            return nonEmptySymptoms.Distinct().Count() == nonEmptySymptoms.Count;
        }

        private async Task RecommendDoctorAsync()
        {
            var doctor = await this.recommendationSystem.RecommendDoctorAsynchronous(this);

            if (doctor != null)
            {
                this.DoctorName = $"Doctor: {doctor.GetDoctorName()}";
                this.DoctorDepartment = $"Department: {doctor.GetDoctorDepartment()}";
                this.DoctorRating = $"Rating: {doctor.GetDoctorRating():0.0}";
            }
            else
            {
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
