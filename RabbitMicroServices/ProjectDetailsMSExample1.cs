using Newtonsoft.Json;

namespace RabbitMQMicroservices;

public class ProjectDetailsMSExample1
{
    private readonly RabbitMqMicroservice _rabbitMq;

    public ProjectDetailsMSExample1()
    {
        _rabbitMq = new RabbitMqMicroservice();
        _rabbitMq.DeclareQueue("Projects");
        _rabbitMq.StartListening("Projects", Display);
    }

    private void Display(string projectJson)
    {
        var project = JsonConvert.DeserializeObject<Project>(projectJson);
        Console.WriteLine($"Project description is {project.Description}");
    }
}