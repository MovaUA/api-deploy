namespace Api
{
  public interface IMongoDbSettings
  {
    string AuthDb { get; }
    string User { get; }
    string Password { get; }
    string Host { get; }
    int Port { get; }
    string Db { get; }

    /// <summary>
    ///   Milliseconds
    /// </summary>
    int SelectTimeoutMS { get; }
  }
}
