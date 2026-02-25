using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic-exchange-example", type: ExchangeType.Topic);

Console.Write("Dinlenecek topic formatını belirtiniz : ");
string topic = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: "topic-exchange-example",
    routingKey: topic
);

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer
);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();

/*

*.weather: Bu ifade, herhangi bir kelime ile başlayan ve ardından “weather” ile biten routing key değerlerini temsil eder. Örneğin, “usa.weather” veya “europe.weather” gibi.

#.news: Bu ifade, herhangi bir sayıda kelime ile başlayan ve ardından “news” ile biten routing key değerlerini temsil eder. Örneğin, “usa.news”, “europe.news” veya "xyz.abc.news” gibi.

Aslında her ikisi de aynı şey fakat #. karakterinde birden fazla kelimeyi temsil ederken *. karakteri tek bir kelimeyi temsil ediyor.

*/
