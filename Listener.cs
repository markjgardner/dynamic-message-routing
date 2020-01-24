using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace router 
{
  class Listener
  {
    MessageReceiver client;

    public Listener(string ConnectionString, string TopicName, string SubscriptionName) 
    {
      this.client = new MessageReceiver(ConnectionString, EntityNameHelper.FormatSubscriptionPath(TopicName, SubscriptionName));
    }

    public async Task Listen()
    {
      while (true) 
      {
        var message = await client.ReceiveAsync();
        if (message != null)
          Console.WriteLine(Encoding.UTF8.GetString(message.Body));
      }
    }
  }
}