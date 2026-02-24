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
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

// 4- Queue'dan Mesaj Okuma
EventingBasicConsumer consumer = new(channel);

// autoAck: false => Consume edilecek mesajın consumerdan bilgi gelmedikçe rabbitmq tarafından silinmeyeceği anlamına gelir. Yani mesajın başarılı bir şekilde işlendiği bilgisini consumerdan bekler. Eğer autoAck: true olursa, mesaj tüketildikten hemen sonra kuyruktan silinir, bu da mesaj kaybına yol açabilir.
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer: consumer);

// Mesaj işleme konfigürasyonu
// prefetchSize: 0 => Bir consumer tarafından alınabilecek en büyük mesaj boyutunu byte cinsinden belirler. 0, sınırsız demektir.
// prefetchCount: 1 => Bir consumer tarafından aynı anda işlenebilecek mesaj sayısını belirler.
// global: false => Bu ayar, prefetchCount ve prefetchSize ayarlarının sadece bu kanal için geçerli olduğunu belirtir. Eğer global: true olursa, bu ayarlar tüm kanallar için geçerli olur.
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yerdir!
    //e.Body : Kuyruktaki mesajın verisini bütünsel olarak getirecektir.
    //e.Body.Span veya e.Body.ToArray() : Kuyrukdaki mesajın byte verisini getirecektir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    // Mesajın başarılı bir şekilde işlendiği bilgisini RabbitMQ'ya bildirme
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);

    // BasicNack : Mesajın işlenemediği bilgisini RabbitMQ'ya bildirme
    // requeue: true => Mesajın tekrar kuyruğa atılmasını sağlar.
    // requeue: false => Mesajın kuyruğa atılmadan silinmesini sağlar.
    // channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true);
};

Console.Read();
