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
   public string ConnectionString => "mmongodb+srv://benjamintrue0549:ItIzIYLr7Q6JH6rd@vitahuscluster.db7b1.mongodb.net/";
}

public class ActivityDatabaseSetting
{
   public string? Host { get; set; }
   public int Port { get; set; }
   public string? DatabaseName { get; set; }
   public string ConnectionString => "mongodb+srv://benjamintrue0549:ItIzIYLr7Q6JH6rd@vitahuscluster.db7b1.mongodb.net/";
}