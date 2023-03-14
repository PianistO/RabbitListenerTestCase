using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Consumer.Model;
using Newtonsoft.Json;

namespace Consumer
{
    public class RabbitMQConsumer
    {
        public RabbitMQConsumer()
        {
            string serviceName = "RabbitListener";
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5673") // app settings ekle bunu oradan çeksin. dockerfile da yazman lazım 
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("urls",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            string deserializedMessage = "";
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var utf8Reader = new Utf8JsonReader(body);
                deserializedMessage = System.Text.Json.JsonSerializer.Deserialize<string>(ref utf8Reader)!;
                Console.WriteLine(message);

            
            channel.BasicConsume("urls",
                    autoAck: true,
                    consumer);
            Console.ReadLine();
            var statusCode = GetStatusCode(deserializedMessage);
            LogModel logModel = (new LogModel {
                StatusCode = statusCode,
                ServiceName = serviceName,
                Url = deserializedMessage,
            });
            AddResultsToLog(logModel);
            };
        }
        private int GetStatusCode(string url)
        {
            using var client = new HttpClient();
            
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(url).Result; // client ı mocklayarak 400 500 filan ne döndü diyebilirsin.

            return (int)response.StatusCode;
        }
        private void AddResultsToLog(LogModel logModel)
        {
            var jsonLog = JsonConvert.SerializeObject(logModel);
            new Logger().Info(jsonLog);
        }
    }
}
