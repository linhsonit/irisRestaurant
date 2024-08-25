using Newtonsoft.Json;
using OrderingService.Data;
using OrderingService.Model;
using OrderingService.Utility;
using RabbitMQ.Client;
using System.Text;

namespace OrderingService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderingDbContext _dbContext;

        public OrderService(OrderingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
        {
            var result = _dbContext.OrderItems.Add(orderItem);
            await _dbContext.SaveChangesAsync();

            SendOrderItem(orderItem);

            return result.Entity;
        }

        public bool SendOrderItem(OrderItem orderItem)
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

                    var productDetail = JsonConvert.SerializeObject(orderItem);
                    var body = Encoding.UTF8.GetBytes(productDetail);

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
