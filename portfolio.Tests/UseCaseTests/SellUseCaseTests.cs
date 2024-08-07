using Db;
using Entities;
using NSubstitute;
using UseCases;

namespace portfolio.Tests.UseCaseTests;

public class SellUseCaseTests
{
    [Fact]
    public async void Given_a_successful_sell_operation_it_must_return_the_user_investments_updated()
    {
        var viewUseCase = Substitute.For<IView>();
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new SellUseCase(viewUseCase, databaseContext);

        var investmentID = Guid.NewGuid().ToString();
        const string investmentName = "any";
        var user = new User
        {
            Name = "anyuser",
            Investments = new Dictionary<string, int>{
                {investmentName, 1}
            }
        };
        viewUseCase.View(investmentID).ReturnsForAnyArgs(async i =>
        {
            return await Task.FromResult(new Investment
            {
                Name = "any",
            });
        });

        databaseContext.GetUser(user.Name).Returns(user);

        var result = await usecase.Sell(user.Name, investmentID);

        Assert.IsType<User>(result);
        Assert.Empty(result.Investments);
    }

    [Fact]
    public async void Given_a_sell_operation_it_must_call_view_use_case_to_check_if_product_exists()
    {
        var viewUseCase = Substitute.For<IView>();
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new SellUseCase(viewUseCase, databaseContext);

        var investmentID = Guid.NewGuid().ToString();
        const string investmentName = "any";
        var user = new User
        {
            Name = "any",
            Investments = new Dictionary<string, int>{
                {investmentName, 1}
            }
        };

        viewUseCase.View(investmentID).ReturnsForAnyArgs(async i =>
        {
            return await Task.FromResult(new Investment
            {
                Name = "any",
            });
        });

        databaseContext.GetUser(user.Name).Returns(user);


        var result = await usecase.Sell(user.Name, investmentID);
        await viewUseCase.ReceivedWithAnyArgs().View(Arg.Any<string>());

        Assert.IsType<User>(result);
        Assert.Empty(result.Investments);
    }

    [Fact]
    public async void Given_a_unsuccessful_buy_operation_it_must_return_the_an_empty_user_investments()
    {
        var viewUseCase = Substitute.For<IView>();
        var databaseContext = Substitute.For<IInvestmentsContext>();
        var usecase = new SellUseCase(viewUseCase, databaseContext);

        var user = new User { Name = "any" };
        var investmentID = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, int>{
                {investmentID,0}
            };

        var result = await usecase.Sell(user.Name, investmentID);

        Assert.IsType<User>(result);
        Assert.Empty(result.Investments);
    }
}