using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;

namespace router 
{
  class Program 
  {
    static void Main (string[] args) 
    {
      //Each topic subscription is registered as an environmentvariable with the prefix "SUBSCRIPTION_ENDPOINT_"
      // The value of the variable is a pipe-delimited (|) combination of connectionstring, topic and subscription names
      // Ex: SUBSCRIPTION_ENDPOINT_MYTOPIC1="Endpoint=sb://mybus.servicebus.windows.net/;SharedAccessKeyName=MyKey;SharedAccessKey=MyKey|MyTopic|MySubscription"
      var builder = new ConfigurationBuilder ().AddEnvironmentVariables ("SUBSCRIPTION_ENDPOINT_");
      IConfiguration config = builder.Build ();

      var listeners = new List<Task> ();

      foreach (var sub in config.AsEnumerable ()) {
        //Split the connection string, topic name and subscription name for initializing the receiver
        var tokens = sub.Value.Split ("|");
        //Register the client
        var l = new Listener(tokens[0],tokens[1],tokens[2]);
        listeners.Add(l.Listen());
        Console.WriteLine($"Listening to {tokens[1]}/{tokens[2]}");
      }

      //Wait forever
      Task.WhenAll(listeners).Wait();
    }    
  }
}