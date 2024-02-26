using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Reciver1 App";

IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "demo-queue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct); //adding exchange
channel.QueueDeclare(queueName, false, false, false, null); //adding queue
channel.QueueBind(queueName, exchangeName, routingKey, null); // binding queue to exchange
channel.BasicQos(0, 1, false); // one message at the time

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(5)).Wait(); //delays reading a nex message for 5 sec

    var body = args.Body.ToArray();

    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message received: {message}");

    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTage = channel.BasicConsume(queueName, false, consumer);

Console.ReadLine();

channel.BasicCancel(consumerTage);
channel.Close();
cnn.Close();