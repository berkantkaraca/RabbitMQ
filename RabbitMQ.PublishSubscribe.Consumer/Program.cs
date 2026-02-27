using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Publish/Subscribe (Pub/Sub) Tasarımı
ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchangeName = "example-pub-sub-exchange";

channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: exchangeName,
    routingKey: string.Empty
);

// tüm consumer'lar o anda sadece 1 mesaj işleyebilir ve boyut sınırı yoktur.
channel.BasicQos(
    prefetchCount: 1,
    prefetchSize: 0,
    global: false
);

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: queueName,
    autoAck: false,
    consumer: consumer
);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();
