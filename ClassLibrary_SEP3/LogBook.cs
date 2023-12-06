using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3;

public class LogBook
{
    [Required]
    public string ProjectID;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string logBookID;

    public List<LogBookEntryPoints> LogBookEntryPoints { get; set; }
}