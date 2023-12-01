using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
<<<<<<< HEAD
using BlazorAppTEST.Services.Auth;
=======
using BlazorAppTEST.Services.Interface;
>>>>>>> Test3

namespace BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using System.Net.Http;


public class BacklogService :IBacklogService
{

    private List<Task?> tasks = new List<Task?>();
    //HTTPClient
    private readonly HttpClient httpClient;

    public BacklogService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        //Add JWT token to the header
        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserService.Jwt);
    }
    
    //Post
    public async Task<Backlog> CreateBacklog(Backlog backlog)
    {
        string createBacklogToJson = JsonSerializer.Serialize(backlog);
        StringContent contentBacklog = new StringContent(createBacklogToJson, Encoding.UTF8, "application/json");
   
        HttpResponseMessage response = await httpClient.PostAsync("/api/Backlog", contentBacklog);
        string responseContent = await response.Content.ReadAsStringAsync();
    
        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<Backlog>(responseContent);
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }
    }



 

  
}