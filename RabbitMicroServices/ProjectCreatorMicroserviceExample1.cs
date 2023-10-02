using Newtonsoft.Json;

namespace RabbitMQMicroservices;

public class ProjectCreatorMicroserviceExample1
{
   private readonly RabbitMqMicroservice _rabbitMqMicroservices;

   public ProjectCreatorMicroserviceExample1()
   {
      _rabbitMqMicroservices = new RabbitMqMicroservice();
      _rabbitMqMicroservices.DeclareQueue("Projects");
   }

   public void CreateAndSendProject()
   {
      var project = new Project
      {
         Id = new Random().Next(),
         Description = "Testing, this works",
         CreatedDate = DateTime.Now
      };
      var projectJson = JsonConvert.SerializeObject(project);
      _rabbitMqMicroservices.SendMessage("Projects", projectJson);
      Console.WriteLine($"Sent The project : {project.Description}");
   }
   
}