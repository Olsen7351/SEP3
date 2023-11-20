using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services
{
    public class RequirementService : IRequirementService
    {
        
        //HTTPClient
        private readonly HttpClient httpClient;

        
        public RequirementService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        
        
        //POST
        public async Task AddRequirement(Requirement requirement) 
        {
            // TODO: send a POST request to add a new requirement.
        }

        
        public async Task<List<Requirement>> GetRequirementsForBacklog(int backlogId)
        {
            // TODO: send a GET request to retrieve requirements for a given backlog.
            return null;
        }
    }
}