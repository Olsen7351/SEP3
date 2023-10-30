using System;
using System.ComponentModel.DataAnnotations;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;

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
        public TaskStatus Status { get; init; }
    }
}
