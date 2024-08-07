namespace UseCases;

public class ViewUseCase : IView
{
    private readonly Db.IInvestmentsContext _investmentContext;
    public ViewUseCase(Db.IInvestmentsContext investmentsContext)
    {
        _investmentContext = investmentsContext;
    }
    public async Task<Entities.Investment> View(string investmentID)
    {
        return await _investmentContext.Get(investmentID);
    }

    // works with tuples
    // private (int, string) test(){
    //     return (1, "x");
    // }
}