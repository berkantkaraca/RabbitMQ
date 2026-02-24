using RabbitMQ.Client;
using System.Text;

// 1- Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");

// 2- Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// 3- Queue Oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// 4- Queue'ya Mesaj Gönderme
//RabbitMQ kuyruğa atacağı mesajları byte dizisi türünde kabul eder.
for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
}

Console.Read();
