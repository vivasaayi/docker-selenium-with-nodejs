using System;
using System.Collections.Generic;
using Docker.DotNet;
using Docker.DotNet.Models;

class DockerProxy
{
  private static List<string> StartedContainers = new List<string>();
  private static DockerClient _dockerClient;
  public static void InitDockerClient()
  {
    if (_dockerClient != null)
    {
      return;
    }

    _dockerClient = new DockerClientConfiguration(
        new Uri("unix:///var/run/docker.sock"))
         .CreateClient();
  }

  public static void StartContainer()
  {
    var env = new List<string>();
    env.Add("NOVNC=true");

    var response = _dockerClient.Containers.CreateContainerAsync(
      new CreateContainerParameters()
      {
        Image = "vivasaayi/selenium-with-nodejs:latest",
        Env = env
      }).GetAwaiter().GetResult();

    Console.WriteLine("Created Container ID:" + response.ID);

    _dockerClient.Containers.StartContainerAsync(response.ID,
      new ContainerStartParameters { }).GetAwaiter().GetResult();

    Console.WriteLine("Started Container ID:" + response.ID);

    StartedContainers.Add(response.ID);
  }

  public static int GetNumberOfContainersStarted()
  {
    var containers = ListContainers();
    var runningContainers = 0;

    foreach (var container in containers)
    {
      if (!StartedContainers.Contains(container.Key))
      {
        continue;
      }

      if(container.Value.State == "running"){
        Console.WriteLine(container.Key + "is running");
        runningContainers++;
        continue;
      } else {
        Console.WriteLine(container.Key + "is exited");
      }
    }

    return runningContainers;
  }

  public static Dictionary<string, ContainerListResponse> ListContainers()
  {
    InitDockerClient();

    IList<ContainerListResponse> containers = _dockerClient.Containers.ListContainersAsync(
      new ContainersListParameters()
      {
        Limit = 10,
      }).GetAwaiter().GetResult();

    var containersDict = new Dictionary<string, ContainerListResponse>();

    foreach (var container in containers)
    {
      containersDict.Add(container.ID, container);
    }

    return containersDict;
  }
}