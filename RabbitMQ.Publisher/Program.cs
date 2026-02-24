using RabbitMQ.Client;
using System.Text;

// 1- Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");

// 2- Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// 3- Queue Oluşturma
// durable: true => RabbitMQ'nun herhangi bir sebeple kapanması durumunda kuyrukta bulunan mesajların kaybolmaması için kullanılır. Eğer durable: false olursa, RabbitMQ kapanırsa kuyrukta bulunan mesajlar kaybolur.
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

// 4- Queue'ya Mesaj Gönderme
//RabbitMQ kuyruğa atacağı mesajları byte dizisi türünde kabul eder.

// BasicProperties => publish edilen mesajların sunucu kapandığında kaybolmaması için mesajların kalıcı (persistent) olarak işaretlenmesini sağlar. Eğer mesajlar persistent olarak işaretlenmezse, RabbitMQ kapanırsa kuyrukta bulunan mesajlar kaybolur.
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);
}

Console.Read();
