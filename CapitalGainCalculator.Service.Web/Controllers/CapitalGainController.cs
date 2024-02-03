using CapitalGainCalculator.Service.Web.DTOs;
using CapitalGainCalculator.Service.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CapitalGainCalculator.Service.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CapitalGainController : ControllerBase
{
    private readonly ICapitalGainService _capitalGainService;
    public CapitalGainController(ICapitalGainService capitalGainService) => 
        _capitalGainService = capitalGainService;

    [HttpPost("CalculateChargeableGain")]
    public IActionResult GetCapitalGain([FromBody] TransactionSet transactions)
    {
        var result = _capitalGainService.GetTotalCapitalGain(transactions);
        return Ok(result);
    }
}
