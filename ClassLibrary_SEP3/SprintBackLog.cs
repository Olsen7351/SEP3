using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3
{
    public class SprintBacklog
    {
        public string ProjectID { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SprintBacklogID { get; set; }

        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Task> Tasks { get; set; }

    }
}
