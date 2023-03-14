using ConsumerWorkerService.Consumers;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RabbitListener.Core.Commands;

namespace RabbitListenerTest
{
    [TestFixture]
    public class ConsumerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task is_consumer_functional()
        {
            await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<UrlCommandConsumer>();
            }).BuildServiceProvider(true);
            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            var client = harness.GetRequestClient<UrlCommand>();

            NUnit.Framework.Assert.IsTrue(await harness.Sent.Any<UrlCommand>());
            NUnit.Framework.Assert.IsTrue(await harness.Consumed.Any<UrlCommand>());
            var consumerHarness = harness.GetConsumerHarness<UrlCommandConsumer>();
            NUnit.Framework.Assert.That(await consumerHarness.Consumed.Any<UrlCommand>());
        }

    }
}
