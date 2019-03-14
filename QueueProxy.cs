using System;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

class QueueProxy
{
  private static CloudQueueClient GetClient()
  {
    // Parse the connection string and return a reference to the storage account.
    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
        "DefaultEndpointsProtocol=https;AccountName=testrunner;AccountKey=h3vjkAQ8f81kT74TSSbbHYEdK5jauEgO1WmR3ygO7NY9f7FODvOqd45PjrTqwuQoGfPvfqcMtjbWVI5TWToz+A==");
    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

    return queueClient;
  }

  public static void PushMessages()
  {
    CloudQueueClient queueClient = GetClient();

    // Retrieve a reference to a queue.
    CloudQueue queue = queueClient.GetQueueReference("test-runner-triggers");

    int index = 0;
    while (true)
    {
      // Create a message and add it to the queue.
      CloudQueueMessage message = new CloudQueueMessage("Message::" + index++);
      queue.AddMessageAsync(message).GetAwaiter().GetResult();
      // Console.WriteLine("Message Added to Queue");
      Thread.Sleep(300);
    }
  }

  public static void ConsumeMessages()
  {
    CloudQueueClient queueClient = GetClient();

    // Retrieve a reference to a queue.
    CloudQueue queue = queueClient.GetQueueReference("test-runner-triggers");

    var allowedContainers = ConfigValues.GetIntValue("ALLOWEDCONTAINERS", 4);
    while (true)
    {
      Thread.Sleep(300);

      var runningContainers = DockerProxy.GetNumberOfContainersStarted();

      Console.WriteLine(runningContainers + "/" + allowedContainers + " containers are running...", runningContainers);

      if (runningContainers >= allowedContainers)
      {
        Console.WriteLine(runningContainers + "/" + allowedContainers + " containers are running...");
        continue;
      }

      // Create a message and add it to the queue.
      var messages = queue.GetMessagesAsync(3, TimeSpan.FromMinutes(5), null, null).GetAwaiter().GetResult();

      foreach (CloudQueueMessage message in messages)
      {
        // Process all messages in less than 5 minutes, deleting each message after processing.
        Console.WriteLine("Received:" + message.AsString);
        DockerProxy.StartContainer();
        queue.DeleteMessageAsync(message).GetAwaiter().GetResult();
      }

      Console.WriteLine("Messages Processed from Queue");
    }
  }
}