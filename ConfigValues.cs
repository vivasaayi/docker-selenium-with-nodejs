using System;
using Microsoft.Extensions.Configuration;

class ConfigValues
{
  static IConfigurationRoot _configRoot;

  private static void Init()
  {
    _configRoot = new ConfigurationBuilder()
      .AddEnvironmentVariables()
      .Build();
  }
  public static int GetIntValue(string key, int defaultValue)
  {
    Init();

    var str = _configRoot[key];

    if(string.IsNullOrEmpty(str)){
      return defaultValue;
    }

    var value = _configRoot.GetValue<int>(key);

    return value;
  }
}