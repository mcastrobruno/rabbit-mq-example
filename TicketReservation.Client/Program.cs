using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservation.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Aguardando para inicializar....");
            //Task.Delay(3000).Wait();


            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumerHello = new EventingBasicConsumer(channel);
                consumerHello.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [Consumer Hello] Received {0}", message);
                };

                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumerHello);


                var consumerTeste = new EventingBasicConsumer(channel);
                consumerTeste.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [Consumer Teste] Received {0}", message);
                };

                channel.BasicConsume(queue: "teste",
                                     autoAck: true,
                                     consumer: consumerTeste);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }


    }
}
