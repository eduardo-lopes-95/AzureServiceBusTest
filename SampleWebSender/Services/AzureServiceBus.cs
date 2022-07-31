using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using SampleShared;

namespace SampleWebSender.Services
{
    public class AzureServiceBus : IAzureServiceBus
    {
        public AzureServiceBus(IConfiguration iConfiguration)
        {
            this.IConfiguration = iConfiguration;

        }
        public IConfiguration IConfiguration { get; }

        public async Task SendMessageAsync(Person personMessage, string queueName)
        {
            var conn = IConfiguration.GetConnectionString("AzureServiceBusConnection");
            var qClient = new QueueClient(conn, queueName);
            var msgBody = JsonSerializer.Serialize(personMessage);
            var msg = new Message(Encoding.UTF8.GetBytes(msgBody));
            await qClient.SendAsync(msg);
        }
    }
}