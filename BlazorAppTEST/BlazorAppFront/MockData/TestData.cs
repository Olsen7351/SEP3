// MockData/TestData.cs

using ProjectMicroservice.Models;

namespace BlazorAppTEST.MockData;

public static class TestData
{
    public static List<Project> Projects => new List<Project>
    {
        new Project { ProjectID = 1, Name = "Project A", Description = "Description for Project A", StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(20) },
        new Project { ProjectID = 2, Name = "Project B", Description = "Description for Project B", StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(30) },
    };
}