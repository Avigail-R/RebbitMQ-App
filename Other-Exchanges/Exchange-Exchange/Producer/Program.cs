using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "firstExchange", type: ExchangeType.Direct);
channel.ExchangeDeclare(exchange: "secondExchange", type: ExchangeType.Fanout);

channel.ExchangeBind("secondExchange", "firstExchange", "");

var message = "This message has gone through multiple exchanges";

var encodedMessage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("firstExchange", "", null, encodedMessage);

Console.WriteLine($"Published message: {message}");