using Confluent.Kafka;
using Kurier.Common.Interfaces;
using Kurier.OrderService.Kafka;
using Kurier.RedisStorage;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace Kurier.OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order service", Version = "v1" }); });

            builder.Services.AddSingleton<IProducer<string, string>>(sp =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092",
                    EnableIdempotence = true,
                    Acks = Acks.All
                };
                return new ProducerBuilder<string, string>(config).Build();
            });

            builder.Services.AddSingleton<IConsumer<string, string>>(sp =>
            {
                var config = new ConsumerConfig
                {
                    GroupId = "order-service-group",
                    BootstrapServers = "localhost:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };
                return new ConsumerBuilder<string, string>(config).Build();
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect("localhost:6379"));
            builder.Services.AddSingleton<IOrderStorage, RedisOrderStorage>();

            builder.Services.AddHostedService<KafkaConsumerHandler>();
            builder.Services.AddSingleton<KafkaProducerHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}