﻿using System;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var random = new Random();

var messageId = 1;
while (true)
{
    var PublishingTime = random.Next(1, 4);

    var message = $"sending messageId: {messageId}";

    var encodedMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("", "letterbox", null, encodedMessage);

    Console.WriteLine($"Published message: {message}");

    Task.Delay(TimeSpan.FromSeconds(PublishingTime)).Wait();

    messageId++;
}