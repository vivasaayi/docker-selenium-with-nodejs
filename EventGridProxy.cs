using System;
using System.Collections.Generic;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

class EventGridProxy
{
  static string topicEndpoint = "https://test-runner-triggers.westus2-1.eventgrid.azure.net/api/events";
  static string topicKey = "8jmLTnFSZg+mcaXrFWhrXtZKThZuBNGrHQksqErpIiU=";

  public static void Publish()
  {
    string topicHostname = new Uri(topicEndpoint).Host;

    TopicCredentials topicCredentials = new TopicCredentials(topicKey);
    EventGridClient client = new EventGridClient(topicCredentials);

    client.PublishEventsAsync(topicHostname, GetEventsList()).GetAwaiter().GetResult();
    Console.Write("Published events to Event Grid.");
  }

  public static IList<EventGridEvent> GetEventsList()
  {
    List<EventGridEvent> eventsList = new List<EventGridEvent>();
    for (int i = 0; i < 1; i++)
    {
      eventsList.Add(new EventGridEvent()
      {
        Id = Guid.NewGuid().ToString(),
        EventType = "Contoso.Items.ItemReceivedEvent",
        Data = "Random Data",
        EventTime = DateTime.Now,
        Subject = "Door1",
        DataVersion = "2.0"
      });

      System.Console.WriteLine("Received Event");
    }
    return eventsList;
  }
}