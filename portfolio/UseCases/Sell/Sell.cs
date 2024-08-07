namespace UseCases;

public class SellUseCase : ISell
{
    private readonly IView _viewUseCase;
    private readonly Db.IInvestmentsContext _investmentsContext;

    public SellUseCase(IView viewUseCase,Db.IInvestmentsContext investmentsContext)
    {
        _viewUseCase = viewUseCase;
        _investmentsContext = investmentsContext;
    }

    public async Task<Entities.User> Sell(string username, string investmentID)
    {
        var user = await _investmentsContext.GetUser(username);
        if (user == null){
            return new Entities.User();
        }
        
        var investment = await _viewUseCase.View(investmentID);

        var userInvestment = user.Investments.FirstOrDefault(i => i.Key == investment.Name);
        if (userInvestment.Value == 1){
            user?.Investments.Remove(investment.Name);
        } else {
            user.Investments[investment.Name]= userInvestment.Value - 1;
        }

        await _investmentsContext.Update(user);
        return user;
    }
}