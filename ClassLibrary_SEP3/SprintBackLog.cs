using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_SEP3
{
    public class SprintBacklog
    {
        public string ProjectId { get; set; }
        public string SprintBacklogId { get; set; }

        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Task> Tasks { get; set; }

    }
}
