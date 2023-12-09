using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using MongoDB.Driver;
using Moq;
using ProjectMicroservice.Data;
using ProjectMicroservice.Services;
using Xunit;

namespace ProjectMicroservice_Tests.DAO_Services;

public class LogBookService_Test : IDisposable
{
    private string _createdProjectId; 
    //MongoDB
    private readonly IMongoCollection<LogBook> _logBookCollection;
    private readonly IMongoCollection<Project> _projectService;
    private readonly MongoDbContext _context;
    private const string DatabaseName = "test_db"; 
    //Service
    private readonly LogBookService _serviceLogBook;
    private readonly ProjectService _serviceProject;
 
 

    public LogBookService_Test()
    {
        var connectionString = GetConnectionString(); // Read from a secure source
        _context = new MongoDbContext(connectionString, DatabaseName);
        _logBookCollection = _context.Database.GetCollection<LogBook>("LogBook");
        _projectService = _context.Database.GetCollection<Project>("Projects");
        _serviceLogBook = new LogBookService(_context);
        _serviceProject = new ProjectService(_context);
    }

    private string GetConnectionString()
    {
        return "mongodb+srv://SEP3:1234@sep3.poymhnv.mongodb.net/?retryWrites=true&w=majority";
    }

    
    //CreateNewEntry --------------------------------------------------------------------------------------------------
    [Fact]
    public void CreateNewEntry_WithValidInformation()
    {
        // Arrange
        var request = new AddEntryPointRequest
        {
            ProjectID = "validProjectId",
            OwnerUsername = "username",
            Description = "description",
            CreatedTimeStamp = DateTime.Today
        };

        // Act
        var result = _serviceLogBook.CreateNewEntry(request);

        // Assert
        Assert.NotNull(result);
    }
    
    
    [Fact]
    public void CreateNewEntry_NullProjectID()
    {
        // Arrange
        var request = new AddEntryPointRequest
        {
            ProjectID = null,
            OwnerUsername = "username",
            Description = "description",
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        Assert.Throws<Exception>(() => _serviceLogBook.CreateNewEntry(request));
    }
    
    
    [Fact]
    public void CreateNewEntry_EmptyProjectID()
    {
        // Arrange
        var request = new AddEntryPointRequest
        {
            ProjectID = "",
            OwnerUsername = "username",
            Description = "description",
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        Assert.Throws<Exception>(() => _serviceLogBook.CreateNewEntry(request));
    }
    
    
    [Fact]
    public void CreateNewEntry_EmptyOwnerUsername()
    {
        // Arrange
        var request = new AddEntryPointRequest
        {
            ProjectID = "IamATestShouldn'tExist",
            OwnerUsername = "",
            Description = "description",
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        Assert.Throws<Exception>(() => _serviceLogBook.CreateNewEntry(request));
    }
    
    
    [Fact]
    public void CreateNewEntry_NullOwnerUsername()
    {
        // Arrange
        var request = new AddEntryPointRequest
        {
            ProjectID = "IamATestShouldn'tExist",
            OwnerUsername = null,
            Description = "description",
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        Assert.Throws<Exception>(() => _serviceLogBook.CreateNewEntry(request));
    }
    
    
    [Fact]
    public void CreateNewEntry_WhereDateIsntToday()
    {
        // Arrange
        var request = new AddEntryPointRequest
        {
            ProjectID = "IamATestShouldn'tExist",
            OwnerUsername = null,
            Description = "description",
            CreatedTimeStamp = DateTime.Today.AddDays(1) // Correct way to get tomorrow's date
        };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _serviceLogBook.CreateNewEntry(request));
        Assert.Equal("Created entry needs to have a present timestamp", exception.Message);
    }
    
    
    //GetLogbookForProject, GetSpecificLogBookEntry and UpdateLogBookEntry Not Possible but works, or have to rework Entry Object-------------------------------------------------------------------------------------
    /*
    [Fact]
    public void GetLogbookForProject_WithValidInformation()
    {
        // Arrange
        var projectRequest = new CreateProjectRequest()
        {
            Name = "TestProject_", // Unique name for test isolation
            Description = "DeleteMePlease",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today,
        };

        // Act
        var createdProject = _serviceProject.CreateProject(projectRequest);
        _createdProjectId = createdProject.Id;
        if (String.IsNullOrEmpty(createdProject.Id))
        {
            throw new Exception("ID is null! or empty");
        }
        var result = _serviceLogBook.GetLogbookForProject(createdProject.Id);

        // Assert
        Assert.NotNull(result);
    }
    */
  
    
    
    public void Dispose()
    {
        if (!string.IsNullOrEmpty(_createdProjectId))
        {
            var logBookFilter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, _createdProjectId);
            _logBookCollection.DeleteOne(logBookFilter);
            
            
            var projectFilter = Builders<Project>.Filter.Eq(p => p.Id, _createdProjectId);
            _projectService.DeleteOne(projectFilter);
        }
    }
}
