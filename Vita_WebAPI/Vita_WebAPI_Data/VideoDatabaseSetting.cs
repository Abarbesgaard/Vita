namespace Vita_WebAPI_Data;

public class VideoDatabaseSetting
{
   public string Host { get; set; }
   public int Port { get; set; }
   public string ConnectionString => $"mongodb://{Host}:{Port}";
}