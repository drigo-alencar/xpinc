using Db;
using Entities;
using NSubstitute;
using UseCases;

namespace portfolio.Tests.UseCaseTests;

public class BuyUseCaseTests{
    [Fact]
    public async void Given_a_successful_buy_operation_it_must_return_the_user_investments_updated()
    {
        var viewUseCase = Substitute.For<IView>();
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new BuyUseCase(viewUseCase, databaseContext);

        var user = new User{Name = "any"};
        databaseContext.GetUser(user.Name).Returns(user);

        var investmentID = Guid.NewGuid().ToString();
        viewUseCase.View(Arg.Any<string>()).ReturnsForAnyArgs(async i => {
            return await Task.FromResult(new Investment{
                ID = investmentID,
                Name = "any"
            });
        });
        
        var result = await usecase.Buy(user.Name,investmentID);
        

        Assert.IsType<User>(result);
        Assert.NotEmpty(result.Investments);
    }

    [Fact]
    public async void Givcen_a_buy_operation_if_investment_not_exists_must_return_null()
    {
        var viewUseCase = Substitute.For<IView>();
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new BuyUseCase(viewUseCase, databaseContext);

        var user = new User{Name = "any"};
        databaseContext.GetUser(user.Name).Returns(user);

        var investmentID = Guid.NewGuid().ToString();
        viewUseCase.View(Arg.Any<string>()).ReturnsForAnyArgs(async i => {
            return await Task.FromResult<Investment>(null);
        });
        
        var result = await usecase.Buy(user.Name,investmentID);
        

        Assert.Null(result);
    }

     [Fact]
    public async void Given_a_buy_operation_it_must_call_view_use_case_to_determine_if_investment_exists()
    {
        var viewUseCase = Substitute.For<IView>();
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new BuyUseCase(viewUseCase, databaseContext);

        var user = new User{Name = "any"};
        databaseContext.GetUser(user.Name).Returns(user);

        var investmentID = Guid.NewGuid().ToString();
           viewUseCase.View(Arg.Any<string>()).ReturnsForAnyArgs(async i => {
            return await Task.FromResult(new Investment{
                ID = investmentID,
                Name = "any"
            });
        });
        databaseContext.When( u=> u.Update(user)).Do(x =>
        {
            user.Investments["investment"] = 1;
        });
        
        var result = await usecase.Buy(user.Name,investmentID);
        await viewUseCase.ReceivedWithAnyArgs().View(Arg.Any<string>());

        Assert.IsType<User>(result);
        Assert.NotEmpty(result.Investments);
    }

      [Fact]
    public async void Given_a_buy_operation_for_a_not_existent_user_it_must_create_it_and_conclude_operation_with_success()
    {
        var viewUseCase = Substitute.For<IView>();
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new BuyUseCase(viewUseCase, databaseContext);

        var user = new User{Name = "any"};
        var investmentID = Guid.NewGuid().ToString();
          viewUseCase.View(Arg.Any<string>()).ReturnsForAnyArgs(async i => {
            return await Task.FromResult(new Investment{
                ID = investmentID,
                Name = "any"
            });
        });
        databaseContext.When( u=> u.Update(user)).Do(x =>
        {
            user.Investments["investment"] = 1;
        });
        
        var result = await usecase.Buy(user.Name,investmentID);

        Assert.IsType<User>(result);
        Assert.NotEmpty(result.Investments);
    }
}