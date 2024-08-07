namespace UseCases;

public interface ISell {
    Task<Entities.User> Sell(string username, string investmentID);
}