using MongoDB.Driver;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    // Parametresiz kurucu metod eklendi
    public MongoDbContext(string connectionString)
    {
        // MongoDB baðlantýsýný yapacak olan bu metodu istediðiniz gibi doldurabilirsiniz
        var client = new MongoClient("mongodb://elif:elifkeskin@localhost:27017/");
        _database = client.GetDatabase("adventureAlly");
    }
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
