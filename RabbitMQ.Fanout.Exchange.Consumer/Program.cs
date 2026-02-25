using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://bdlaeprn:eoEmqqj5PqEbqaCteKcYcI-Z6AelM7Ck@cow.rmq2.cloudamqp.com/bdlaeprn");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

Console.Write("Kuyruk adını giriniz : ");
string queueName = Console.ReadLine();

channel.QueueDeclare(
    queue: queueName,
    exclusive: false
);
// exclusive (QueueDeclare)
// true → Queue sadece o connection’a özeldir, connection kapanınca silinir.
// false → Queue herkes tarafından kullanılabilir, kapanınca silinmez.

// Kuyruğu exchange’e bind işlemi
channel.QueueBind(
    queue: queueName,
    exchange: "fanout-exchange-example",
    routingKey: string.Empty
);

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer
);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();
