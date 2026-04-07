using Application.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.Messaging;

public class NullMessageProducer : IMessageProducer
{
    public Task ProduceAsync(string topic, object message) => Task.CompletedTask;
}