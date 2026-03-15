using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessages;

namespace RabbitMQ.ESB.MassTransit.RequestResponsePattern.Consumer.Consumers
{
    public class ReqeustMessageConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            // Consumer gelen mesajı yazdırıp response döner
            Console.WriteLine(context.Message.Text);
            await context.RespondAsync<ResponseMessage>(new() { Text = $"{context.Message.MessageNo}. response to request" });
        }
    }
}
