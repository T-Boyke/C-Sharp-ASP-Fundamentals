using Microsoft.AspNetCore.Mvc;
using _07_Patienten.Domain.Interfaces;

namespace _07_Patienten.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class MedicationsApiController : ControllerBase
{
    private readonly IMedicationLookupService _lookupService;

    public MedicationsApiController(IMedicationLookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet("lookup/{pzn}")]
    public async Task<IActionResult> Lookup(string pzn)
    {
        var result = await _lookupService.SearchByPznAsync(pzn);
        
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return Ok(result);
    }
}
