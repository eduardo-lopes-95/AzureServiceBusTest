using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SampleShared;

namespace SampleAppReceiver
{
    class Program
    {
        const string connString = "Endpoint=sb://sampleservicebusun.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dwy5cHj6iLVPNmdj73AYzky1HXm6NDuh2X7mgF7uSCk=";
        static IQueueClient qClient;
        static async Task Main(string[] args)
        {
            qClient = new QueueClient(connString, "personqueue");
            var msgOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete =  false,
            };

            qClient.RegisterMessageHandler(ProcessMessageAsync, msgOptions);

            Console.ReadLine();
            await qClient.CloseAsync();
        }

        private static async Task ProcessMessageAsync(Message msg, CancellationToken token)
        {
            var jsonBody = Encoding.UTF8.GetString(msg.Body);
            var personObj = JsonSerializer.Deserialize<Person>(jsonBody);
            Console.WriteLine($"First Name: {personObj.FirstName}\n"
                +$"Last Name: {personObj.LastName}\n"
                +$"Email: {personObj.Email}");
            
            await qClient.CompleteAsync(msg.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine($"Something went wrong, {args.Exception}");
            return Task.CompletedTask;
        }
    }
}
