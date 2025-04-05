using MongoDB.Driver;
namespace ICT2106WebApp.mod1Grp3
{
	public class MongoDbService
	{
		private readonly IMongoDatabase _database;

		public MongoDbService()
		{
			string connectionString =
				"mongodb+srv://2301915:eD8eNLFZtqLAZRdM@inf2112.e9qpy.mongodb.net/?appName=inf2112";
			string databaseName = "inf2112";

			var client = new MongoClient(connectionString);
			_database = client.GetDatabase(databaseName);
		}

		public IMongoDatabase Database => _database;

		// use this to access collection e.g. GetCollection<Docx>("docxCollection")
		public IMongoCollection<T> GetCollection<T>(string name)
		{
			return _database.GetCollection<T>(name);
		}
	}
}