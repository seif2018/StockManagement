using Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Messaging;
public class KafkaProducer : IMessageProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly string _topic;

    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092"
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
        _topic = configuration["Kafka:Topic"] ?? "stock-events";
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, object message)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(message);
        try
        {
            var result = await _producer.ProduceAsync(topic ?? _topic, new Message<string, string> { Key = default!, Value = json });
            _logger.LogInformation("Message produit: {Offset}", result.Offset);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la production du message");
        }
    }
}
