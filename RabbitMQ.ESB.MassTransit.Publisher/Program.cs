using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

string rabbitMQUri = "amqps://bdlaeprn:dlImO7vIf_5g50SuB7I4BL1Zpj479rox@cow.rmq2.cloudamqp.com/bdlaeprn";

string queueName = "example-queue";

// Çalışılacak sunucuya bağlanarak bir bus oluşturuyoruz.
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

// Send ile mesaj gönderme işleminde hedef kuyruğa mesaj iletilir.
// Publish ile mesaj gönderme işleminde ise kuruğu dinleyen tüm tüketiciler tarafından yakalanır.
ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));

Console.Write("Gönderilecek mesaj : ");
string message = Console.ReadLine();
await sendEndpoint.Send<IMessage>(new ExampleMessage()
{
    Text = message
});

Console.Read();
