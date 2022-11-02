using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

var replayQueue = channel.QueueDeclare(queue: "", durable: true );
channel.QueueDeclare("request-queue", exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (Model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    System.Console.WriteLine($"Reply Received: {message}");
};

channel.BasicConsume(queue: replayQueue.QueueName, autoAck: true, consumer: consumer);

var message = "can I request a reply";
var body = Encoding.UTF8.GetBytes(message);
var properties = channel.CreateBasicProperties();
properties.ReplyTo = replayQueue.QueueName;
properties.CorrelationId = Guid.NewGuid().ToString();
channel.BasicPublish("", "request-queue", properties, body);
System.Console.WriteLine($"Sending Request: {properties.CorrelationId}");

System.Console.WriteLine("Started client");

System.Console.ReadKey();
