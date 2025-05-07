using System.Collections.Generic;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Service
{
    /// <summary>
    /// Interface for doctor search operations and sorting functionality
    /// </summary>
    public interface ISearchDoctorsService
    {
        List<DoctorModel> AvailableDoctors { get; }

        Task LoadDoctors(string searchTerm);

        List<DoctorModel> GetSearchedDoctors();

        List<DoctorModel> GetDoctorsSortedBy(SortCriteria sortCriteria);
    }
}