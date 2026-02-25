using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1. Adım: Publisher'da ki exchange ile birebir aynı isim ve type'a sahip bir exchange tanımlanmalıdır!
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2. Adım: Publisher tarafından belirli bir routing key ile gönderilen mesajları tüketebilmek için, bu routing key’e bağlı bir kuyruk oluşturup ilgili exchange’e bind etmemiz gerekir.
string queueName = channel.QueueDeclare().QueueName;

//3. Adım: routing key'e karşılık gelen mesajları queue'ye yönlendirir
channel.QueueBind(
    queue: queueName,
    exchange: "direct-exchange-example",
    routingKey: "direct-queue-example"
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
