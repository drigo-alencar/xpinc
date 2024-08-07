using DTO;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UseCases;

namespace Controllers;

[ApiController]
[Consumes("application/json")]
[Route("/v1/[controller]")]
public class InvestmentsController : ControllerBase
{
    private readonly ICreate _createUseCase;
    private readonly IView _viewUseCase;
    private readonly IUpdate _updateUseCase;
    private readonly IBuy _buyUseCase;
    private readonly ISell _sellUseCase;
    private readonly ILogger<InvestmentsController> _logger;

    public InvestmentsController(ILogger<InvestmentsController> logger,
    ICreate createUseCase,
    IView viewUseCase,
    IUpdate updateUseCase,
    IBuy buyUseCase,
    ISell sellUseCase)
    {
        _createUseCase = createUseCase;
        _viewUseCase = viewUseCase;
        _updateUseCase = updateUseCase;
        _buyUseCase = buyUseCase;
        _sellUseCase = sellUseCase;
        _logger = logger;
    }

    [HttpPost(Name = "PostInvestment")]
    public async Task<ActionResult<Entities.Investment>> Post([FromHeader] DTO.User user, [FromBody] DTO.Investment investment)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            return this.BadRequest(new Entities.Error { Field = nameof(user.Name), Message = "user name must be present" });
        }
        if (user.Type == UserType.None || user.Type == UserType.Customer)
        {
            return this.Unauthorized(new Entities.Error { Field = nameof(user.Type) });
        }

        var newInvestment = new Entities.Investment
        {
            Name = investment.Name,
            Expiration = investment.Expiration,
            Price = investment.Price,
        };

        var created = await _createUseCase.Create(newInvestment);
        return this.Created($"/v1/investments/{created.ID}", created);
    }

    [HttpGet("{investmentID}", Name = "GetInvestment")]
    public async Task<ActionResult<Entities.Investment>> GetInvestment([FromHeader] DTO.User user, string investmentID)
    {
        var investment = await _viewUseCase.View(investmentID);
        if (investment ==null || investment.IsEmpty())
        {
            return this.NotFound(investmentID);
        }
        return this.Ok(investment);
    }

    [HttpPut("{investmentID}", Name = "PutInvestment")]
    public async Task<ActionResult<Entities.Investment>> PutInvestment([FromHeader] DTO.User user, string investmentID, [FromBody] DTO.Investment investment)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            return this.BadRequest(new Entities.Error { Field = nameof(user.Name), Message = "user name must be present" });
        }
        if (user.Type == UserType.None || user.Type == UserType.Customer)
        {
            return this.Unauthorized(new Entities.Error { Field = nameof(user.Type) });
        }


        var newInvestment = new Entities.Investment
        {
            Name = investment.Name,
            Expiration = investment.Expiration,
            Price = investment.Price,
        };
        var investmentUpdated = await _updateUseCase.Update(investmentID, newInvestment);

        return this.Ok(investmentUpdated);
    }

    [HttpPost("{investmentID}/sell", Name = "SellInvestment")]
    public async Task<ActionResult<Entities.Investment>> Sell([FromHeader] DTO.User user, string investmentID)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            return this.BadRequest(new Entities.Error { Field = nameof(user.Name), Message = "user name must be present" });
        }
        if (user.Type == UserType.None || user.Type == UserType.Operator)
        {
            return this.Unauthorized(new Entities.Error { Field = nameof(user.Type) });
        }

        var investment = await _sellUseCase.Sell(user.Name, investmentID);
        return this.Ok(investment);
    }

    [HttpPost("{investmentID}/buy", Name = "BuyInvestment")]
    public async Task<ActionResult<Entities.Investment>> Buy([FromHeader] DTO.User user, string investmentID)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            return this.BadRequest(new Entities.Error { Field = nameof(user.Name), Message = "user name must be present" });
        }
        if (user.Type == UserType.None || user.Type == UserType.Operator)
        {
            return this.Unauthorized(new Entities.Error { Field = nameof(user.Type) });
        }

        var investment = await _buyUseCase.Buy(user.Name, investmentID);
        return this.Ok(investment);
    }
}
