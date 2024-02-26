using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";

IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "demo-queue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct); //adding exchange
channel.QueueDeclare(queueName, false, false, false, null); //adding queue
channel.QueueBind(queueName, exchangeName, routingKey,null); // binding queue to exchange

//byte[] messageBodyBites = Encoding.UTF8.GetBytes("Hello YouTube Again"); //adding simple message
//channel.BasicPublish(exchangeName, routingKey, null, messageBodyBites); //publishing the message

for(int i = 0; i < 60; i++) 
{
    Console.WriteLine($"Sending Message number {i}");

    byte[] messageBodyBites = Encoding.UTF8.GetBytes($"Message number {i}"); //adding simple message
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBites); //publishing the message
    Thread.Sleep(1000); //simulating 1 sec delay 

}

channel.Close();
cnn.Close();