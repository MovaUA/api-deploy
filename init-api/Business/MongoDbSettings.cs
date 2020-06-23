namespace Init.Api.Business
{
	public class MongoDbSettings : IMongoDbSettings
	{
		public string AuthDb { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string Db { get; set; }
		public int SelectTimeoutMS { get; set; }
	}
}
