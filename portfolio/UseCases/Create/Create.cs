using MongoDB.Driver;

namespace UseCases;

public class CreateUseCase : ICreate
{
    private readonly Db.IInvestmentsContext _investmentsContext;

    public CreateUseCase(Db.IInvestmentsContext investmentsContext)
    {
        _investmentsContext = investmentsContext;
    }

    public async Task<Entities.Investment> Create(Entities.Investment investment)
    {
        var exists = await _investmentsContext.Get(investment.ID);
        if (exists != null){
            return exists;
        }
        try
        {
            await _investmentsContext.Create(investment);
            return investment;
        }
        catch (Exception)
        {
            return new Entities.Investment();
        }
    }
}