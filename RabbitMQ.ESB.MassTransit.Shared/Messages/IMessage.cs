namespace RabbitMQ.ESB.MassTransit.Shared.Messages
{
    /// <summary>
    /// Uygulamalardaki mesaj formatını belirler. (sipariş oluşturma, sipariş iptal, stok güncelleme vb.)
    /// </summary>
    public interface IMessage
    {
        public string Text { get; set; }
    }
}
