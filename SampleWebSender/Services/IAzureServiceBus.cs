using System.Threading.Tasks;
using SampleShared;

namespace SampleWebSender.Services
{
    public interface IAzureServiceBus
    {
        Task SendMessageAsync(Person personMessage, string queueName);
    }
}