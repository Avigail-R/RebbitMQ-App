using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myRoutingExchange", ExchangeType.Direct);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "myRoutingExchange", routingKey: "analyticsOnly");
channel.QueueBind(queue: queueName, exchange: "myRoutingExchange", routingKey: "both");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (Model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    System.Console.WriteLine($"Analytics - Message Received: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

System.Console.WriteLine("Analytics - Consuming");

System.Console.ReadKey();