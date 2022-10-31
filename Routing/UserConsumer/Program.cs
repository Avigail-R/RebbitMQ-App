using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myTopicExchange", ExchangeType.Topic);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "myTopicExchange",
 routingKey: "user.#");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (Model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    System.Console.WriteLine($"User - Message Received: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

System.Console.WriteLine("User - Consuming");

System.Console.ReadKey();