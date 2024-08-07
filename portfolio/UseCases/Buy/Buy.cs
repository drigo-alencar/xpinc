using Entities;

namespace UseCases;

public class BuyUseCase : IBuy
{
    private readonly IView _viewUseCase;
    private readonly Db.IInvestmentsContext _investmentsContext;

    public BuyUseCase(IView viewUseCase,Db.IInvestmentsContext investmentsContext)
    {
        _viewUseCase = viewUseCase;
        _investmentsContext = investmentsContext;
    }
    
    public async Task<Entities.User> Buy(string username, string investmentID)
    {
        var user = await _investmentsContext.GetUser(username);
        if (user == null){
            user = new Entities.User{Name = username, Type = UserType.Customer};
        }
        
        var investment = await _viewUseCase.View(investmentID);
        if (investment == null){
            return null;
        }

        var userInvestment = user.Investments.FirstOrDefault(i => i.Key == investment.Name);
        user.Investments[investment.Name] = userInvestment.Value + 1;

        await _investmentsContext.Update(user);

        return user;
    }
}