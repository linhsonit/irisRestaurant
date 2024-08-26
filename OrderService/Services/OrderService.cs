using Newtonsoft.Json;
using OrderService.Data;
using OrderService.Model;
using OrderService.Utility;
using RabbitMQ.Client;
using System.Text;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _dbContext;

        public OrderService(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> AddOrderItemAsync(Order order)
        {
            var result = _dbContext.Orders.Add(order);

            order.Status = "NEW";
            await _dbContext.SaveChangesAsync();

            SendOrderItem(order);

            return result.Entity;
        }

        public bool SendOrderItem(Order order)
        {
            var RabbitMQServer = "";
            var RabbitMQUserName = "";
            var RabbitMQPassword = "";

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                RabbitMQServer = Environment.GetEnvironmentVariable("RABBIT_MQ_SERVER");
                RabbitMQUserName = Environment.GetEnvironmentVariable("RABBIT_MQ_USERNAME");
                RabbitMQPassword = Environment.GetEnvironmentVariable("RABBIT_MQ_PASSWORD");
            }
            else
            {
                RabbitMQServer = StaticConfigurationManager.AppSetting["RabbitMQ:RabbitURL"];
                RabbitMQUserName = StaticConfigurationManager.AppSetting["RabbitMQ:Username"];
                RabbitMQPassword = StaticConfigurationManager.AppSetting["RabbitMQ:Password"];
            }

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = RabbitMQServer,
                    UserName = RabbitMQUserName,
                    Password = RabbitMQPassword
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(StaticConfigurationManager.AppSetting["RabbitMqSettings:ExchangeName"],
                        StaticConfigurationManager.AppSetting["RabbitMqSettings:ExchhangeType"]);

                    channel.QueueDeclare(queue: StaticConfigurationManager.AppSetting["RabbitMqSettings:QueueName"],
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.QueueBind(
                        queue: StaticConfigurationManager.AppSetting["RabbitMqSettings:QueueName"],
                        exchange: StaticConfigurationManager.AppSetting["RabbitMqSettings:ExchangeName"],
                        routingKey: StaticConfigurationManager.AppSetting["RabbitMqSettings:RouteKey"]);

                    var orderJson = JsonConvert.SerializeObject(order);
                    var body = Encoding.UTF8.GetBytes(orderJson);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: StaticConfigurationManager.AppSetting["RabbitMqSettings:ExchangeName"],
                                         routingKey: StaticConfigurationManager.AppSetting["RabbitMqSettings:RouteKey"],
                                         basicProperties: properties,
                                         body: body);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error]: {e.Message} {e.StackTrace}");
            }

            return false;
        }
    }
}
