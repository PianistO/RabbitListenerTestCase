using ConsumerWorkerService.Consumers;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitListener.Core.Commands;

namespace RabbitListenerTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task is_consumer_functional()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> 
            {
                {"ProjectConfig:ServiceName", "RabbitListener"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<UrlCommandConsumer>();
            })
            .AddTransient<IConfiguration>(x => new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build())
            .BuildServiceProvider(true);
            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            var client = harness.GetRequestClient<UrlCommand>();
            await client.GetResponse<UrlCommand>(new
            {
                Url = "tempUrl"
            });
 
            NUnit.Framework.Assert.IsTrue(await harness.Sent.Any<UrlCommand>());
            NUnit.Framework.Assert.IsTrue(await harness.Consumed.Any<UrlCommand>());
            var consumerHarness = harness.GetConsumerHarness<UrlCommandConsumer>();
            NUnit.Framework.Assert.That(await consumerHarness.Consumed.Any<UrlCommand>());
        }
    }
}