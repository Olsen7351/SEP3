using System.Net;
using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class LogBook_Test
{

    private readonly Mock<HttpClient> mockHttpClient;
    private readonly LogBookService logBookService;

    public LogBook_Test()
    {
        mockHttpClient = new Mock<HttpClient>();
        logBookService = new LogBookService(mockHttpClient.Object);
    }


    //Get Entries-------------------------------------------------------------------------------------------------------
    //Valid Information
    [Fact]
    public async Task GetEntriesForLogBook_WithValidProjectId()
    {
        // Arrange
        var mockResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("...") // your expected content here
        };
        var handler = new MockHttpMessageHandler(mockResponseMessage);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost/"), // Set the base address to your API
        };
        var logBookService = new LogBookService(httpClient);

        // Act
        var result = await logBookService.GetEntriesForLogBook("validProjectId");

        // Assert
        Assert.IsType<OkResult>(result);
    }

    
    //Null
    [Fact]
    public async Task GetEntriesForLogBook_WhenProjectIdIsNull()
    {
        // Arrange
        string projectId = null;

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => logBookService.GetEntriesForLogBook(projectId));
    }

    
    //Empty
    [Fact]
    public async Task GetEntriesForLogBook_WhenProjectIdIsEmpty()
    {
        // Arrange
        string projectId = "";

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => logBookService.GetEntriesForLogBook(projectId));
    }
    
    
    
    //Create Entries -------------------------------------------------------------------------------------------------------
    //Valid Information
    [Fact]
    public async Task CreateNewEntryToLogBook_WithValidEntry()
    {
        // Arrange
        AddEntryPointRequest validEntry = new AddEntryPointRequest()
        {
            ProjectID = "bsajfbasf",
            OwnerUsername = "James",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };

        var mockResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        var handler = new MockHttpMessageHandler(mockResponseMessage);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost/") // Set the base address to your API
        };
        var logBookService = new LogBookService(httpClient);

        // Act
        var result = await logBookService.CreateNewEntryToLogBook(validEntry);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    
    
    [Fact]
    public async Task CreateNewEntryToLogBook_WhenOwnerUsernameIsNull()
    {
        // Arrange
        AddEntryPointRequest entryPoints = new AddEntryPointRequest()
        {
            ProjectID = "asfbujahsf",
            OwnerUsername = null,
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => logBookService.CreateNewEntryToLogBook(entryPoints));
    }
    
    [Fact]
    public async Task CreateNewEntryToLogBook_WhenOwnerUsernameIsEmpty()
    {
        // Arrange
        AddEntryPointRequest entryPoints = new AddEntryPointRequest()
        {
            OwnerUsername = "",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => logBookService.CreateNewEntryToLogBook(entryPoints));
    }

    [Fact]
    public async Task CreateNewEntryToLogBook_WhenDescriptionIsNull()
    {
        // Arrange
        AddEntryPointRequest entryPoints = new AddEntryPointRequest()
        {
            ProjectID = "ASFbjasf",
            OwnerUsername = "James",
            Description = null,
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => logBookService.CreateNewEntryToLogBook(entryPoints));
    }
    
    
    
    [Fact]
    public async Task CreateNewEntryToLogBook_WhenDescriptionIsEmpty()
    {
        // Arrange
        AddEntryPointRequest entryPoints = new AddEntryPointRequest()
        {
            ProjectID = "DAHbfjhsa",
            OwnerUsername = "James",
            Description = "",
            CreatedTimeStamp = DateTime.Today
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => logBookService.CreateNewEntryToLogBook(entryPoints));
    }
    
    
    [Fact]
    public async Task CreateNewEntryToLogBook_WhenCreatedTimeStampIsNotToday()
    {
        // Arrange
        AddEntryPointRequest entryPoints = new AddEntryPointRequest()
        {
            ProjectID = "ASHFbjasf",
            OwnerUsername = "James",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today.AddDays(-1) // Set to a past date
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => logBookService.CreateNewEntryToLogBook(entryPoints));
    }
    
    
    
    
    
    
    
    
    //Helper --------------------------------------------------------------------------------------------
    public class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> fakeResponses = new Dictionary<Uri, HttpResponseMessage>();

        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            fakeResponses.Add(uri, responseMessage);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (fakeResponses.ContainsKey(request.RequestUri))
            {
                return await Task.FromResult(fakeResponses[request.RequestUri]);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request };
            }
        }
    }

    
    
    
    //Helper
    public class MockHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage _mockResponse;

        public MockHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _mockResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_mockResponse);
        }
    }
}
