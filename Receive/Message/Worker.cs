using System;
using Microsoft.AspNetCore.Connections;
using System.Text;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Message
{
    public class Worker
    {
        public string QueueName { get; set; }

        public Worker()
        {
        }


        public void SendMessage(string message)
        {
            System.Threading.Thread.Sleep(1000);
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string _message = message;
                var body = Encoding.UTF8.GetBytes(_message);

                channel.BasicPublish(exchange: "",
                                     routingKey: QueueName,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", _message);
            }

        }


    }
}

