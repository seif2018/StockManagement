using System.Threading.Tasks;

namespace Application.Interfaces;
public interface IMessageProducer
{
    Task ProduceAsync(string topic, object message);
}
