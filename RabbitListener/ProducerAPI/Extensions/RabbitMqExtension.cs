using MassTransit;
using RabbitListener.Core.Configurations;
using System.Configuration;

namespace ProducerAPI.Extensions
{
    public static class RabbitMqExtension
    {
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(opt => {
                var rabbitMqConfiguration = configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();

                opt.UsingRabbitMq((context, factory) => {
                    factory.Host(rabbitMqConfiguration.Url, configurator => {
                        configurator.Username(rabbitMqConfiguration.Username);
                        configurator.Password(rabbitMqConfiguration.Password);
                    });
                });
            });
        }
    }
}
