using ClassLibrary.Model;

namespace WebClient.Models
{
    public class DoctorDashboardViewModel
    {
        public DoctorModel Doctor { get; set; }
        public List<LogEntryModel> RecentLogs { get; set; }
        public DateTime CurrentDate { get; set; }
        public int AppointmentsToday { get; set; }
        public int PendingTasks { get; set; }
    }
} 