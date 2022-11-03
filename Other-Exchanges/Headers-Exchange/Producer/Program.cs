using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "headerExchange", type: ExchangeType.Headers);

var message = "This message will be sent with headers";

var encodedMessage = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();

properties.Headers = new Dictionary<string, object>{
    {"name", "brian"}
};

channel.BasicPublish("headerExchange", "", properties, encodedMessage);

Console.WriteLine($"Published message: {message}");