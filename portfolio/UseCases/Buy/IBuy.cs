namespace UseCases;

public interface IBuy {
    Task<Entities.User> Buy(string username, string investmentID);
}