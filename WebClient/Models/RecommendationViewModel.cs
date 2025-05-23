using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebClient.Models
{
    public class RecommendationViewModel
    {
        public string SelectedSymptomStart { get; set; }
        public string SelectedDiscomfortArea { get; set; }
        public string SelectedSymptomPrimary { get; set; }
        public string SelectedSymptomSecondary { get; set; }
        public string SelectedSymptomTertiary { get; set; }

        public IEnumerable<SelectListItem> SymptomStartOptions { get; set; }
        public IEnumerable<SelectListItem> DiscomfortAreaOptions { get; set; }
        public IEnumerable<SelectListItem> SymptomTypeOptions { get; set; }

        public string DoctorName { get; set; }
        public string DoctorDepartment { get; set; }
        public string DoctorRating { get; set; }
    }

}
