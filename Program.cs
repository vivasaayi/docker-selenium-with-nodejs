using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace selenium_with_nodejs
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      // EventGridProxy.Publish();

      DockerProxy.ListContainers();

      var tasks = new List<Task>();

      // var publisherTask = Task.Run(() => QueueProxy.PushMessages());
      var subscriberTask = Task.Run(() => QueueProxy.ConsumeMessages());

      // tasks.Add(publisherTask);
      tasks.Add(subscriberTask);

      Task.WaitAll(tasks.ToArray());
    }
  }
}
