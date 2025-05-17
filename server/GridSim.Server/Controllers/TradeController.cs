using Microsoft.AspNetCore.Mvc;
using GridSim.Server.Services;
using GridSim.Server.Models;

namespace GridSim.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TradeController : ControllerBase
{
    private readonly TradeService _tradeService;

    public TradeController(TradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpPost("buy")]
    public IActionResult Buy([FromQuery] int quantity)
    {
        if (_tradeService.Buy(quantity, out var trade))
            return Ok(trade);
        return BadRequest(new { error = "Insufficient funds" });
    }

    [HttpPost("sell")]
    public IActionResult Sell([FromQuery] int quantity)
    {
        if (_tradeService.Sell(quantity, out var trade))
            return Ok(trade);
        return BadRequest(new { error = "Insufficient holdings" });
    }

    [HttpGet("portfolio")]
    public IActionResult GetPortfolio()
    {
        return Ok(_tradeService.GetPortfolio());
    }
}