using MassTransit;
using MassTransit.Transports.Fabric;
using RabbitListener.Core.Configurations;
using System.Configuration;
using System.Net;
using RabbitListener.Core;
using ConsumerWorkerService.Consumers;
namespace ConsumerWorkerService.Extensions
{
    public static class RabbitMqExtension
    {
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(opt => {
                var rabbitMqConfiguration = configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();

                opt.AddConsumer<UrlCommandConsumer>();

                opt.UsingRabbitMq((context, factory) => {
                    factory.Host(rabbitMqConfiguration.Url, configurator => {
                        configurator.Username(rabbitMqConfiguration.Username);
                        configurator.Password(rabbitMqConfiguration.Password);
                    });

                    factory.ReceiveEndpoint(rabbitMqConfiguration.UrlQueue,
                        e => { e.ConfigureConsumer<UrlCommandConsumer>(context); });
                });
            });
        }
    }
}
