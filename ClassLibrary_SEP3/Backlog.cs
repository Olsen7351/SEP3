using System;

namespace ProjectMicroservice.Models
{
    public class Backlog
    {
        public int Id { get; init; }
        public int ProjectId { get; init; }
    
        public string Description { get;set; }

        public Backlog(string description)
        {
            Description = description;
        }
    }
    
    
}
