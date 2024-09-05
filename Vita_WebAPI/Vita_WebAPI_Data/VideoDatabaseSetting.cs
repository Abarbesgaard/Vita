namespace Vita_WebAPI_Data;

/// <summary>
/// Defines the settings for the video database.
/// </summary>
public class VideoDatabaseSetting
{
   /// <summary>
   /// The host of the database.
   /// </summary>
   public string? Host { get; set; }
   /// <summary>
   /// The port of the database.
   /// </summary>
   public int Port { get; set; }
   /// <summary>
   /// The name of the database.
   /// </summary>
   public string? DatabaseName { get; set; }
   /// <summary>
   /// The connection string for the database.
   /// </summary>
   public string ConnectionString => $"mongodb://{Host}:{Port}";
}