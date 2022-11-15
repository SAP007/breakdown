using Microsoft.AspNetCore.Mvc;

namespace Message.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;

    public MessageController(ILogger<MessageController> logger)
    {
        _logger = logger;
    }


    //endpointet er api/message/

    [HttpGet]
    public IActionResult Get()
    {
        Console.WriteLine("About to send a message");
        Worker _worker = new Worker();
        _worker.QueueName = "Weather";
        _worker.SendMessage("GetWeatherForecast");
        
        Console.WriteLine("I just sent a message");
        return StatusCode(200);
    }
}

