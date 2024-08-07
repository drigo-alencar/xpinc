
namespace Db;

public interface IInvestmentsContext {
    Task Create(Entities.Investment investment);
    Task<Entities.User> Create(Entities.User user);
    Task<Entities.Investment> Get(string investmentID);
    Task Update(string investmentID, Entities.Investment investment);
    Task Update(Entities.User user);
    Task<Entities.User> GetUser(string username);
}