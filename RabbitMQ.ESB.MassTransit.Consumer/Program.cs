using MassTransit;
using RabbitMQ.ESB.MassTransit.Consumer.Consumers;

string rabbitMQUri = "amqps://bdlaeprn:dlImO7vIf_5g50SuB7I4BL1Zpj479rox@cow.rmq2.cloudamqp.com/bdlaeprn";

string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);

    factory.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();