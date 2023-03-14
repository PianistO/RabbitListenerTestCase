using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Consumer;

namespace RabbitListener.Consumer
{
    class Program
    {
        public static RabbitMQConsumer rabbitMQConsumer { get; set; }
        static void Main(string[] args)
        {
            rabbitMQConsumer = new RabbitMQConsumer();
        }
    }
}