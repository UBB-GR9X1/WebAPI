using ClassLibrary.Model;

namespace WebClient.Models
{
    public class LogFilterViewModel
    {
        public List<LogEntryModel> logs { get; set; }
        public string user_id { get; set; }
        public ActionType? selected_action_type { get; set; }
        public DateTime? selected_date { get; set; }
        public List<ActionType> action_types => Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

        public LogFilterViewModel()
        {
            logs = new List<LogEntryModel>();
        }
    }
}