using RabbitMQ.Client;
using System.Text;

// Publish/Subscribe (Pub/Sub) Tasarımı
ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchangeName = "example-pub-sub-exchange";

channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);

    byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);

    channel.BasicPublish(
        exchange: exchangeName,
        routingKey: string.Empty,
        body: message
    );
}

Console.Read();
