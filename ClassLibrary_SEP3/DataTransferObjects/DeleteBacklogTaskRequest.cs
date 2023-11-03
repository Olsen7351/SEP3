using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class DeleteBacklogTaskRequest
{
    [Required]
    public int ProjectId { get; init; }

    [Required]
    public string BacklogId { get; init;}

    [Required]
    public ObjectId TaskId { get; init; }
}