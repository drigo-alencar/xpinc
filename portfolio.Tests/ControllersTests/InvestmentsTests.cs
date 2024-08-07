using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace portfolio.Tests.ControllersTests;

public class InvestmentsTests
{
    [Theory]
    [InlineData(UserType.Customer)]
    [InlineData(UserType.Operator)]
    public async void Given_is_an_user_but_without_name_it_must_return_bad_request(UserType type)
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var createUseCase = Substitute.For<UseCases.ICreate>();
        var api = new Controllers.InvestmentsController(logger,
        createUseCase,
        Substitute.For<UseCases.IView>(),
        Substitute.For<UseCases.IUpdate>(),
        Substitute.For<UseCases.IBuy>(),
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Type = type };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };

        var response = await api.Post(user, investment);

        Assert.IsType<BadRequestObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_operator_but_without_type_it_must_return_access_denied()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var createUseCase = Substitute.For<UseCases.ICreate>();
        var api = new Controllers.InvestmentsController(logger,
        createUseCase, Substitute.For<UseCases.IView>(),
        Substitute.For<UseCases.IUpdate>(),
        Substitute.For<UseCases.IBuy>(),
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody" };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };

        var response = await api.Post(user, investment);

        Assert.IsType<UnauthorizedObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_customer_creating_an_investment_it_must_return_access_denied()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var createUseCase = Substitute.For<UseCases.ICreate>();
        var api = new Controllers.InvestmentsController(logger,
        createUseCase,
         Substitute.For<UseCases.IView>(),
         Substitute.For<UseCases.IUpdate>(),
         Substitute.For<UseCases.IBuy>(),
         Substitute.For<UseCases.ISell>()
         );

        var user = new DTO.User { Name = "anybody", Type = UserType.Customer };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };

        var response = await api.Post(user, investment);

        Assert.IsType<UnauthorizedObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_operator_creating_it_must_return_created()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var createUseCase = Substitute.For<UseCases.ICreate>();
        var api = new Controllers.InvestmentsController(logger,
        createUseCase,
        Substitute.For<UseCases.IView>(),
        Substitute.For<UseCases.IUpdate>(),
        Substitute.For<UseCases.IBuy>(),
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody", Type = UserType.Operator };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };
        createUseCase.Create(Arg.Any<Entities.Investment>()).ReturnsForAnyArgs(async x =>
        {
            return await Task.FromResult(new Entities.Investment
            {
                ID = Guid.NewGuid().ToString(),
                Name = investment.Name,
                Expiration = investment.Expiration,
                Price = investment.Price
            });
        });

        var response = await api.Post(user, investment);
        await createUseCase.Received().Create(Arg.Any<Entities.Investment>());

        Assert.IsType<CreatedResult>(response.Result);
    }

    [Theory]
    [InlineData(UserType.Customer)]
    [InlineData(UserType.Operator)]
    public async void Given_is_an_user_it_must_return_the_investment(UserType type)
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var viewUseCase = Substitute.For<UseCases.IView>();

        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        viewUseCase,
        Substitute.For<UseCases.IUpdate>(),
        Substitute.For<UseCases.IBuy>(),
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody", Type = type };
        var investment = new Entities.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch, ID = Guid.NewGuid().ToString() };
        viewUseCase.View(investment.ID).ReturnsForAnyArgs(async x =>
        {
            return await Task.FromResult(investment);
        });

        var response = await api.GetInvestment(user, investment.ID);
        await viewUseCase.Received().View(investment.ID);

        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Theory]
    [InlineData(UserType.Customer)]
    [InlineData(UserType.Operator)]
    public async void Given_is_an_user_is_viewing_an_investment_if_is_empty_must_return_not_found(UserType type)
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var viewUseCase = Substitute.For<UseCases.IView>();

        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        viewUseCase,
        Substitute.For<UseCases.IUpdate>(),
        Substitute.For<UseCases.IBuy>(),
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody", Type = type };
        var investmentID = Guid.NewGuid().ToString();
        var investment = new Entities.Investment();
        viewUseCase.View(investmentID).ReturnsForAnyArgs(async x =>
        {
            return await Task.FromResult(investment);
        });

        var response = await api.GetInvestment(user, investmentID);
        await viewUseCase.Received().View(investmentID);

        Assert.IsType<NotFoundObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_customer_updating_an_investment_it_must_return_access_denied()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var updateUseCase = Substitute.For<UseCases.IUpdate>();
        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        Substitute.For<UseCases.IView>(),
        updateUseCase,
        Substitute.For<UseCases.IBuy>(),
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody", Type = UserType.Customer };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };
        var investmentID = Guid.NewGuid().ToString();
        updateUseCase.Update(investmentID, Arg.Any<Entities.Investment>()).ReturnsForAnyArgs(async x =>
        {
            await Task.FromResult(new Entities.Investment
            {
                ID = investmentID,
                Name = investment.Name,
                Price = investment.Price,
                Expiration = investment.Expiration
            });
        });

        var response = await api.PutInvestment(user, investmentID, investment);

        Assert.IsType<UnauthorizedObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_operator_updating_an_investment_it_must_return_ok()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var updateUseCase = Substitute.For<UseCases.IUpdate>();
        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        Substitute.For<UseCases.IView>(),
        updateUseCase,
        Substitute.For<UseCases.IBuy>(),
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody", Type = UserType.Operator };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };
        var investmentID = Guid.NewGuid().ToString();
        updateUseCase.Update(investmentID, Arg.Any<Entities.Investment>()).ReturnsForAnyArgs(async x =>
        {
            return await Task.FromResult(new Entities.Investment
            {
                ID = investmentID,
                Name = investment.Name,
                Price = investment.Price,
                Expiration = investment.Expiration
            });
        });

        var response = await api.PutInvestment(user, investmentID, investment);
        await updateUseCase.ReceivedWithAnyArgs().Update(Arg.Any<string>(), Arg.Any<Entities.Investment>());

        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_operator_buying_an_investment_it_must_return_unauthorized()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var buyUseCase = Substitute.For<UseCases.IBuy>();
        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        Substitute.For<UseCases.IView>(),
        Substitute.For<UseCases.IUpdate>(),
        buyUseCase,
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody", Type = UserType.Operator };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };
        var investmentID = Guid.NewGuid().ToString();

        var response = await api.Buy(user, investmentID);
        await buyUseCase.DidNotReceiveWithAnyArgs().Buy(Arg.Any<string>(), Arg.Any<string>());

        Assert.IsType<UnauthorizedObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_customer_buying_an_investment_it_must_return_ok()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var buyUseCase = Substitute.For<UseCases.IBuy>();
        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        Substitute.For<UseCases.IView>(),
        Substitute.For<UseCases.IUpdate>(),
        buyUseCase,
        Substitute.For<UseCases.ISell>()
        );

        var user = new DTO.User { Name = "anybody", Type = UserType.Customer };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };
        var investmentID = Guid.NewGuid().ToString();
        buyUseCase.Buy(user.Name, investmentID).
        ReturnsForAnyArgs(await Task.FromResult(new Entities.User
        {
            Investments = { { investmentID, 0 } }
        }));


        var response = await api.Buy(user, investmentID);
        await buyUseCase.ReceivedWithAnyArgs().Buy(Arg.Any<string>(), Arg.Any<string>());

        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_operator_selling_an_investment_it_must_return_unauthorized()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var sellUseCase = Substitute.For<UseCases.ISell>();
        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        Substitute.For<UseCases.IView>(),
        Substitute.For<UseCases.IUpdate>(),
        Substitute.For<UseCases.IBuy>(),
        sellUseCase
        );

        var user = new DTO.User { Name = "anybody", Type = UserType.Operator };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };
        var investmentID = Guid.NewGuid().ToString();

        var response = await api.Sell(user, investmentID);
        await sellUseCase.DidNotReceiveWithAnyArgs().Sell(Arg.Any<string>(), Arg.Any<string>());

        Assert.IsType<UnauthorizedObjectResult>(response.Result);
    }

    [Fact]
    public async void Given_is_an_customer_selling_an_investment_it_must_return_ok()
    {
        var logger = Substitute.For<ILogger<Controllers.InvestmentsController>>();
        var sellUseCase = Substitute.For<UseCases.ISell>();
        var api = new Controllers.InvestmentsController(logger,
        Substitute.For<UseCases.ICreate>(),
        Substitute.For<UseCases.IView>(),
        Substitute.For<UseCases.IUpdate>(),
        Substitute.For<UseCases.IBuy>(),
        sellUseCase
        );

        var user = new DTO.User { Name = "anybody", Type = UserType.Customer };
        var investment = new DTO.Investment { Name = "", Price = 0.0, Expiration = DateTime.UnixEpoch };
        var investmentID = Guid.NewGuid().ToString();
        sellUseCase.Sell(user.Name, investmentID).ReturnsForAnyArgs(
            await Task.FromResult(new Entities.User
            {
                Investments = { { investmentID, 0 } }
            }));

        var response = await api.Sell(user, investmentID);
        await sellUseCase.ReceivedWithAnyArgs().Sell(Arg.Any<string>(), Arg.Any<string>());

        Assert.IsType<OkObjectResult>(response.Result);
    }
}