using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq" };
        System.Threading.Thread.Sleep(60000);
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);
            
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
           
        }
    }

    /*
    private IConnection ConnectoToRabbitMQ(ConnectionFactory factory, ushort numberOfRetries)
    {
        try
        {
            var conn = factory.CreateConnection();
            return conn;
        }
        catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException e)
        {
            Console.WriteLine(e);
            if (numberOfRetries == 0)
                throw;

            Thread.Sleep(60000);
            ConnectoToRabbitMQ(factory, --numberOfRetries);
            return null;
        }

    }
    */

}
