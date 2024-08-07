using Db;

namespace UseCases;

public class StatementUseCase : IStatement
{
    private readonly IInvestmentsContext _investmentsContext;

    public StatementUseCase(Db.IInvestmentsContext investmentsContext)
    {
        _investmentsContext=investmentsContext;
    }
    public async Task<IDictionary<string, int>> Find(string username)
    {
        var user = await _investmentsContext.GetUser(username);
        return user.Investments;
    }
}
