using Db;
using Microsoft.AspNetCore.Mvc;
using UseCases;

namespace Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class StatementsController : ControllerBase
{
    private readonly IStatement _statementUseCase;

    public StatementsController(IStatement statementUseCase)
    {
        _statementUseCase = statementUseCase;
    }

    [HttpGet(Name = "GetStatement")]
    public async Task<ActionResult<IDictionary<string, int>>> Get([FromHeader] DTO.User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            return this.BadRequest(new Entities.Error { Field = nameof(user.Name), Message = "user name must be present" });
        }
        if (user.Type == Entities.UserType.None || user.Type == Entities.UserType.Operator)
        {
            return this.Unauthorized(new Entities.Error { Field = nameof(user.Type), Message = "operators aren't allowed" });
        }

        var statement = await _statementUseCase.Find(user.Name);
        return this.Ok(statement);
    }
}