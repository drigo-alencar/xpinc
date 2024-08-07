using Db;
using DTO;
using Entities;
using MongoDB.Driver;
using NSubstitute;
using UseCases;

namespace portfolio.Tests.UseCaseTests;

public class CreateUseCaseTests
{
    [Fact]
    public async void Given_an_investment_creation_it_must_be_successful()
    {
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new CreateUseCase(databaseContext);

        var investment = new Entities.Investment { Name = "asd", Expiration = DateTime.UnixEpoch };
        databaseContext.Create(Arg.Any<Entities.Investment>()).Returns(async i =>
        {
            await Task.FromResult(new Entities.Investment
            {
                ID = Guid.NewGuid().ToString(),
                Name = "any",
                Expiration = DateTime.UnixEpoch
            });
        });

        var result = await usecase.Create(investment);

        Assert.IsType<Entities.Investment>(result);
        Assert.NotEqual(string.Empty, result.ID);
    }
    [Fact]
    public async void Given_an_duplicated_investment_creation_it_must_return_the_existent()
    {
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new CreateUseCase(databaseContext);

        var investment = new Entities.Investment { Name = "asd", Expiration = DateTime.UnixEpoch, ID = Guid.NewGuid().ToString() };
        databaseContext.Get(investment.ID).Returns(async i =>
        {
            return await Task.FromResult(new Entities.Investment
            {
                ID = Guid.NewGuid().ToString(),
                Name = "any",
                Expiration = DateTime.UnixEpoch
            });
        });

        var result = await usecase.Create(investment);
        await databaseContext.DidNotReceive().Create(investment);

        Assert.IsType<Entities.Investment>(result);
        Assert.NotEqual(string.Empty, result.ID);
    }

    [Fact]
    public async void Given_an_investment_creation_fail_it_must_return_empty_object()
    {
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new CreateUseCase(databaseContext);

        var investment = new Entities.Investment { Name = "asd", Expiration = DateTime.UnixEpoch };
        databaseContext.When(d => d.Create(Arg.Any<Entities.Investment>())).Throw(new MongoException("errr"));

        var result = await usecase.Create(investment);

        Assert.IsType<Entities.Investment>(result);
        Assert.True(result.IsEmpty());
    }
}