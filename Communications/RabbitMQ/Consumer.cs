using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace APIs.Library.Communications.RabbitMQ
{
    public static class Consumer<T>
    {
        /// <summary>
        /// Thread lock object.
        /// </summary>
        private static readonly object ThreadLock = new object();

        /// <summary>
        /// Connection manager.
        /// </summary>
        private static ConnectionManager ConnectionManager { get; set; }
        

        private static Action<T> ConsumerActor { get; set; }


        /// <summary>
        /// Initialize and set connection manager.
        /// </summary>
        /// <param name="connectionManager"></param>
        public static void Initialize(ConnectionManager connectionManager)
            => ConnectionManager = connectionManager;


        /// <summary>
        /// Start RabbitMQ Consumer.
        /// </summary>
        /// <param name="consumerActor"></param>
        public static void StartConsumer(Action<T> consumerActor)
        {
            //if (ConnectionManager is null) throw new ArgumentNullException(@"ConnectionManager is null!");
            //if (ConnectionManager.Channel is null) throw new ArgumentNullException(@"ConnectionManager.Channel is null!");
            //ConsumerActor = consumerActor;

            //ConnectionManager.Channel.BasicQos(10, 10, false);

            //var consumer = new EventingBasicConsumer(ConnectionManager.Channel);
            //consumer.Received += (sender, e) =>
            //{
                
            //    var bodyBytes = e.Body.ToArray();
            //    var jsonString = Encoding.UTF8.GetString(bodyBytes);
            //    var obj = JsonConvert.DeserializeObject<T>(jsonString);
            //    ConsumerActor(obj);
            //};
            //ConnectionManager.Channel.BasicConsume(ConnectionManager.QueueName, autoAck: false, consumer);
        }


        /// <summary>
        /// Consume the message on send.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consume(
              object sender
            , BasicDeliverEventArgs e)
        {
            lock (ThreadLock)
            {
                var bodyBytes = e.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(bodyBytes);
                var obj = JsonConvert.DeserializeObject<T>(jsonString);
                ConsumerActor(obj);
            }
        }
    }
}