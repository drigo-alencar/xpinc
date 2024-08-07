
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using UseCases;

namespace portfolio.Tests.ControllersTests
{
    public class StatementsTests
    {
        [Fact]
        public async void Given_a_customer_user_it_must_return_its_investments()
        {
            var statementUseCase = Substitute.For<IStatement>();
            var api = new Controllers.StatementsController(statementUseCase);

            var user = new DTO.User
            {
                Name = "any",
                Type = Entities.UserType.Customer,
            };
            statementUseCase.Find(user.Name).Returns(async x =>
            {
                return await Task.FromResult<IDictionary<string, int>>(new Dictionary<string, int> { { "investment a", 1 } });
            });

            var result = await api.Get(user);

            Assert.NotNull(result.Result);
        }


        [Fact]
        public async void Given_a_operator_user_it_must_return_unauthorized()
        {
            var statementUseCase = Substitute.For<IStatement>();
            var api = new Controllers.StatementsController(statementUseCase);

            var user = new DTO.User
            {
                Name = "any",
                Type = Entities.UserType.Operator,
            };

            var result = await api.Get(user);

            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }
    }
}