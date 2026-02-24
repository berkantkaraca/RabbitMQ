using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// 1- Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://LINK");

// 2- Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// 3- Queue Oluşturma
// Consumer'daki kuyruk publisher'daki ile birebir aynı yapılandırmada tanımlanmalıdır.
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// 4- Queue'dan Mesaj Okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer: consumer);

consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yerdir!
    //e.Body : Kuyruktaki mesajın verisini bütünsel olarak getirecektir.
    //e.Body.Span veya e.Body.ToArray() : Kuyrukdaki mesajın byte verisini getirecektir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();
