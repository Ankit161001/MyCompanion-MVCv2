using MyCompanion.Models.Domain;

namespace MyCompanion.Models
{
    public class ViewPostedJobsModel
    {
        public User User { get; set; }
        public List<Job> PostedJobs { get; set; }
    }
}
