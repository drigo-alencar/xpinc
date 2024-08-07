using Db;
using DTO;
using Entities;
using NSubstitute;
using UseCases;

namespace portfolio.Tests.UseCaseTests;

public class UpdateUseCaseTests{
    
    [Fact]
    public async void Given_a_investmentID_it_must_update_it(){
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new UpdateUseCase(databaseContext);

        var investmentID = Guid.NewGuid().ToString();
        var investment = new Entities.Investment{Name = "asd", Expiration = DateTime.UnixEpoch};
        databaseContext.When(u=> u.Update(investmentID, Arg.Any<Entities.Investment>())).Do(async i => {
            investment.Name = "any";
            investment.Expiration = new DateTime(2024,8,4);
        });
        
        var result = await usecase.Update(investmentID, investment);

        Assert.IsType<Entities.Investment>(result);
        Assert.Equal("any", result.Name);
        Assert.Equal(new DateTime(2024,8,4), result.Expiration);
    }
}