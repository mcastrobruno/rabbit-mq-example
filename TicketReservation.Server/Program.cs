using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservation.Server
{
    class Program
    {



        static void Main(string[] args)
        {

            Dictionary<string, string> organs = new Dictionary<string, string>
            {
                {"X","Airline" },
                {"Y","ATC" }
            };

            string result = string.Empty;

            Console.WriteLine("Inicializando Server...");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Console.WriteLine("Server inciado.");
                while (result.ToUpper() != "E")
                {
                    Console.WriteLine("Press [E] to exit.");
                    Console.WriteLine("Press [X] to send Message to Airline.");
                    Console.WriteLine("Press [Y] to send Message to ATC.");
                    result = Console.ReadLine();

                    SendMessage(channel, organs[result.ToUpper()]);

                }
            }
        }


        static void SendMessage(IModel channel, string queue)
        {
            string message = "Hello World";
            var body = Encoding.UTF8.GetBytes(message);

            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "organ", queue }
            };


            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            properties.Headers = dictionary;


            channel.BasicPublish(exchange: "amq.headers",
                "",
                basicProperties: properties,
                body: body);

            Console.WriteLine($"[x] Sent {message} to {queue} queue");
        }
    }
}
