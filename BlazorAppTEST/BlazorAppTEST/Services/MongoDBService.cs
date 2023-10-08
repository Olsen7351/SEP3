using MongoDB.Driver;
using MongoDB.Bson;
using System;
using ProjectMicroservice.Models;


namespace MainWeb.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Project> _projects;
        private readonly MongoClient _client;

        public MongoDBService(string connectionString, string YourDatabaseName, string YourCollectionName)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            _client = new MongoClient(settings);
            
            var database = _client.GetDatabase(YourDatabaseName);
            _projects = database.GetCollection<Project>(YourCollectionName);
            
            // Optionally, you can use ping to check the connection here
            try
            {
                var result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MongoDB: {ex}");
            }
        }

        public async Task CreateProject(Project project)
        {
            await _projects.InsertOneAsync(project);
        }
    }
}