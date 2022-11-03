using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare("simpleHashing", "x-consistent-hash");

var message = "hello hash the routing and pass me on pleas";

var encodedMessage = Encoding.UTF8.GetBytes(message);

var routingKey = "Hash me!";

channel.BasicPublish("simpleHashing", "Hash me!", null, encodedMessage);

Console.WriteLine($"Published message: {message}");