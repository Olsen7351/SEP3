using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservice.DataTransferObjects
{
    public class CreateBacklogRequest
    {
        [MaxLength(1000)] // TODO: Change to reflect actual DB constraint
        public string? Description { get; init; }
    }
}
