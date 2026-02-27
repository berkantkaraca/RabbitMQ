using RabbitMQ.Client;
using System.Text;

// P2P (Point-to-Point) Tasarımı
ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string queueName = "example-p2p-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false
 );

byte[] message = Encoding.UTF8.GetBytes("merhaba");

channel.BasicPublish(
    exchange: string.Empty,
    routingKey: queueName,
    body: message
);

Console.Read();
