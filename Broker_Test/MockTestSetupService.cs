using Moq;
using Broker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Broker_Test
{
    public class MockTestSetupService
    {
        /*public static Mock<IProjektService> CreateMockProjektService()
        {
            var mockProjektService = new Mock<IProjektService>();

            //Set up method behavior for GetProjekt
            mockProjektService.Setup(service => service.GetProjekt(It.IsAny<int>()))
                              .Returns((int projectId) =>
                              {
                                  // Create and return an IActionResult of Ok with a specific Projekt object based on the projectId
                                  if (projectId == 1)
                                  {
                                      return new Projekt
                                      {
                                          Id = 1,
                                          Name = "Project 1",
                                          Beskrivelse = "Description 1",
                                          StartDato = DateTime.Now,
                                          SlutDato = DateTime.Now.AddDays(30),
                                      };
                                  }
                                  else if (projectId == 2)
                                  {
                                      return new Projekt
                                      {
                                          Id = 2,
                                          Name = "Project 2",
                                          Beskrivelse = "Description 2",
                                          StartDato = DateTime.Now,
                                          SlutDato = DateTime.Now.AddDays(45),
                                          // Set other properties as needed
                                      };
                                  }
                                  // If the projectId is not matched, return null or throw an exception
                                  return new NotFoundResult(); // You can modify this behavior as needed
                              });


            // Set up method behavior for CreateProjekt
            mockProjektService.Setup(service => service.CreateProjekt(It.IsAny<Project>()))
                              .Returns((Project createdProjekt) =>
                              {
                                  // Create and return an IActionResult of Ok with the input createdProjekt
                                  return createdProjekt;
                              });

            return mockProjektService;
        }
    }*/

    }
}