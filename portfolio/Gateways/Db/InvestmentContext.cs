using Db;
using Entities;
using MongoDB.Driver;

namespace Db;

public class InvestmentsContext: IInvestmentsContext{
    private const string DATABASE = "XPInc";
    private const string INVESTMENTS_COLLECTION = "Investments";
    private const string USER_COLLECTION = "Users";
    private readonly IMongoCollection<Entities.Investment> _investmentCollection;
    private readonly IMongoCollection<Entities.User> _userCollection;

    public InvestmentsContext(Db.Settings settings)
    {
        var mongoClient = new MongoClient(settings.ConnectionString);
        _investmentCollection = mongoClient.GetDatabase(DATABASE).GetCollection<Entities.Investment>(INVESTMENTS_COLLECTION);
        _userCollection = mongoClient.GetDatabase(DATABASE).GetCollection<Entities.User>(USER_COLLECTION);
    }

    public async Task Create(Entities.Investment investment)
    {
        await _investmentCollection.InsertOneAsync(investment);
    }

    public async Task<User> Create(User user)
    {
        await _userCollection.InsertOneAsync(user);
        return user;
    }

    public async Task<Entities.Investment> Get(string investmentID) {
        return await _investmentCollection.Find(i => i.ID == investmentID).FirstOrDefaultAsync();
    }

    public async Task<Entities.User> GetUser(string username)
    {
        return await _userCollection.Find(u => u.Name == username).FirstOrDefaultAsync();
    }

    public async Task Update(string investmentID, Entities.Investment investment)
    {
        await _investmentCollection.ReplaceOneAsync(i => i.ID == investmentID, investment, new ReplaceOptions { IsUpsert=true});
    }

    public async Task Update(Entities.User user)
    {
       await _userCollection.ReplaceOneAsync(u => u.Name == user.Name, user, new ReplaceOptions{IsUpsert = true});
    }
    
}