using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace ParkingManagement
{
    class DatabaseHandler
    {
        private const string connectionString = "mongodb+srv://admin:secure@cluster0.konqe.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
        private static DatabaseHandler instance;
        private readonly MongoClient client;
        private readonly IMongoDatabase database;
        DatabaseHandler()
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            client = new MongoClient(settings);
            database = client.GetDatabase("ParkingManagement");
        }
        public static DatabaseHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseHandler();
                }
                return instance;
            }
        }
        public IMongoDatabase Database { get { return database; } }
        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return database.GetCollection<BsonDocument>(collectionName);
        }
    }

}