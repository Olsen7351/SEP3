using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservice.DataTransferObjects
{
    public class CreateTaskRequest
    {
        [Required]
        [MaxLength(100)] // Adjust based on your DB constraints
        public string Title { get; init; }

        [MaxLength(500)] // Adjust based on your DB constraints
        public string? Description { get; init; }

        [Required]
        public ProjectMicroservice.Models.TaskStatus Status { get; init; }
    }
}
