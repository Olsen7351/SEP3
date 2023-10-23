using System;
using DefaultNamespace;

namespace ProjectMicroservice.Models
{
    public class Backlog
    {
        public int Id { get; init; }
        public int ProjectId { get; init; }
        public string? Description { get; init; }
        public List<BackLogTask> Tasks { get; set; } = new List<BackLogTask>();
    }
}
