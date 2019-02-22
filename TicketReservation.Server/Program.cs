using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservation.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = string.Empty;

            Console.WriteLine("Inicializando Server...");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Console.WriteLine("Server inciado.");

                Console.WriteLine("Inicializando Fila...");
                channel.QueueDeclare(queue: "hello",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
                Console.WriteLine($"Fila [hello] iniciada.");

                channel.QueueDeclare(queue: "teste",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                Console.WriteLine($"Fila [teste] iniciada.");



                while (result.ToUpper() != "E")
                {
                    Console.WriteLine("Press [E] to exit.");
                    Console.WriteLine("Press [H] to send Message to Hello queue.");
                    Console.WriteLine("Press [T] to send Message to Teste queue.");
                    result = Console.ReadLine();

                    if (result.ToUpper() == "E")
                        break;
                    else if (result.ToUpper() == "H")
                    {
                        SendMessage(channel, "hello");
                    }
                    else if (result.ToUpper() == "T")
                    {
                        SendMessage(channel, "teste");
                    }
                }
            }
        }


        static void SendMessage(IModel channel, string queue)
        {
            string message = "Hello World";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: queue,
                basicProperties: null,
                body: body);

            Console.WriteLine($"[x] Sent {message} to {queue} queue");
        }
    }
}
