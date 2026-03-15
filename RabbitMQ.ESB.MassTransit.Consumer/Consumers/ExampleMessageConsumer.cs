using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

namespace RabbitMQ.ESB.MassTransit.Consumer.Consumers
{
    /// <summary>
    /// Kuyruğa IMessage türünde bir mesaj geldiğinde bu sınıf tarafından karşılanır ve Consume metodu çalıştırılır.
    /// </summary>
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Gelen mesaj : {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
