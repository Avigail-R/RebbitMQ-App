using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare("simpleHashing", "x-consistent-hash");

channel.QueueDeclare("letterbox1");
channel.QueueDeclare("letterbox2");

channel.QueueBind("letterbox1", "simpleHashing", "1"); //25%
channel.QueueBind("letterbox2", "simpleHashing", "3"); //75%

var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue1 Received new message: {message}");
};
channel.BasicConsume(queue: "letterbox1", autoAck: true, consumer: consumer1);


var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue2 Received new message: {message}");
};
channel.BasicConsume(queue: "letterbox2", autoAck: true, consumer: consumer2);



Console.WriteLine("Consuming");

Console.ReadKey();