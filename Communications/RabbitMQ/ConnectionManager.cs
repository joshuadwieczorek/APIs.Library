using System;
using System.Collections.Generic;
using System.Net;
using RabbitMQ.Client;

namespace APIs.Library.Communications.RabbitMQ
{
    public class ConnectionManager
    {
        /// <summary>
        /// RabbitMQ connection.
        /// </summary>
        public IConnection Connection { get; private set; }
        

        /// <summary>
        /// Create RabbitMQ connection and initialize channel.
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="durable"></param>
        /// <param name="exclusive"></param>
        /// <param name="autoDelete"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public IConnection InitializeConnectionAndChannel(
              string queueName
            , string user
            , string password
            , string server
            , int port = 5672
            , bool durable = true
            , bool exclusive = false
            , bool autoDelete = false
            , IDictionary<string, object> arguments = null)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri($"amqp://{user}:{WebUtility.UrlEncode(password)}@{server}:{port}")
            };


            return null;

            //Connection = factory.CreateConnection();
            //Channel = Connection.CreateModel();
            //Channel.QueueDeclare(queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete,
            //    arguments: arguments);
            //return Connection;
        }
    }
}