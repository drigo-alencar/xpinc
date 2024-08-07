namespace UseCases;

public class UpdateUseCase : IUpdate
{
    private readonly Db.IInvestmentsContext _investmentsContext;
    public UpdateUseCase(Db.IInvestmentsContext investmentsContext)
    {
        _investmentsContext = investmentsContext;
    }
    public async Task<Entities.Investment> Update(string investmentID, Entities.Investment investment)
    {
        investment.ID = investmentID;
        await _investmentsContext.Update(investmentID, investment);
        return investment;
    }
}