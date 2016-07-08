using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KryptonAPI.DataContracts.JobScheduler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQTester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var factory1 = new ConnectionFactory() { HostName = "localhost" };
            factory1.Port = 8080;
            var connection1 = factory1.CreateConnection();
            var publisherChannel = connection1.CreateModel();
            
            publisherChannel.QueueDeclare(queue: "task_queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            
            var properties = publisherChannel.CreateBasicProperties();
            properties.Persistent = true;

            System.Console.WriteLine("sending first item to the queue");

            var item = new QueueJobItem(5);
            var jsonItem = JsonConvert.SerializeObject(item);
            
            publisherChannel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: Encoding.UTF8.GetBytes(jsonItem));
            
            /*
            var factory2 = new ConnectionFactory() { HostName = "localhost" };
            factory2.Port = 8080;
            var connection2 = factory2.CreateConnection();

            var consumerChannel1 = connection2.CreateModel();
            var result1 = consumerChannel1.BasicGet("task_queue", true);
            
            System.Console.WriteLine("Message count:" + result1.MessageCount);
            System.Console.WriteLine("Message 1: " + Encoding.UTF8.GetString(result1.Body));

            /*
            System.Console.WriteLine("No ack, creating new channel");

            var consumerChannel2 = connection2.CreateModel();
            var result2 = consumerChannel2.BasicGet("task_queue", true);
            
            if(result2 == null) System.Console.WriteLine("No messages found");
            else {
                System.Console.WriteLine("Message count:" + result2.MessageCount);
                System.Console.WriteLine("Message 1: " + Encoding.UTF8.GetString(result2.Body));
            }

            System.Console.WriteLine("Disposing consumeChannel1 after keypress");
            Console.ReadKey();
            consumerChannel1.Dispose();
            System.Console.WriteLine("Querying for messages again after keypress");
            Console.ReadKey();

            result2 = consumerChannel2.BasicGet("task_queue", true);
            if(result2 == null) System.Console.WriteLine("No messages found");
            else {
                System.Console.WriteLine("Message count:" + result2.MessageCount);
                System.Console.WriteLine("Message 1: " + Encoding.UTF8.GetString(result2.Body));
            }
*/
            /*
            using(var channel = connection.CreateModel())
            {
                var res = channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                System.Console.WriteLine("Message count: " + res.MessageCount);

                //var defConsumer = new DefaultBasicConsumer(channel);

                // var consumer = new EventingBasicConsumer(channel);
                // consumer.Received += (model, ea) =>
                // {
                //     var body = ea.Body;
                //     var message = Encoding.UTF8.GetString(body);
                //     Console.WriteLine(" [x] Received {0}", message);
                // };
                // channel.BasicConsume(queue: "hello",
                //                     noAck: true,
                //                     consumer: consumer);
                
                var result = channel.BasicGet("hello", false);

                if(result != null){
                    var message = Encoding.UTF8.GetString(result.Body);
                    Console.WriteLine(" [x] Received {0}", message);
                    channel.BasicAck(result.DeliveryTag, false);
                }
                

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            */
        }
    }
}
