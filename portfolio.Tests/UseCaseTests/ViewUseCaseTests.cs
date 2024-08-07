using Db;
using Entities;
using NSubstitute;
using UseCases;

namespace portfolio.Tests.UseCaseTests;

public class ViewUseCaseTests
{

    [Fact]
    public async void Given_an_id_must_be_able_to_retrieve_it()
    {
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new ViewUseCase(databaseContext);

        var investmentID = Guid.NewGuid().ToString();
        databaseContext.Get(investmentID).Returns( async i => {
            return await Task.FromResult(new Investment{
                Name = "any",
                Expiration = DateTime.UnixEpoch
            });
        });
        
        var result = await usecase.View(investmentID);

        Assert.IsType<Investment>(result);
    }

      [Fact]
    public async void Given_an_id_if_not_found_must_return_empty_investment()
    {
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new ViewUseCase(databaseContext);

        var investmentID = Guid.NewGuid().ToString();
        databaseContext.Get(investmentID).Returns( async i => {
            return await Task.FromResult(new Investment());
        });
        
        var result = await usecase.View(investmentID);

        Assert.True(result.IsEmpty());
    }
}