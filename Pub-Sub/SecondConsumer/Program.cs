using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

var queueName = channel.QueueDeclare().QueueName;

var consumer = new EventingBasicConsumer(channel);

channel.QueueBind(queue: queueName, exchange: "pubsub", routingKey: "");

consumer.Received += (Model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    System.Console.WriteLine($"SecondConsumer -Received new Message : {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

System.Console.WriteLine("Consuming");

System.Console.ReadKey();