using System.ComponentModel.DataAnnotations;
using DateGrpc;
using Microsoft.AspNetCore.Mvc;
using DateRequest = WebApi.Requests.DateRequest;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DateController : Controller
{
    private readonly DateCalculator.DateCalculatorClient _client;

    public DateController(DateCalculator.DateCalculatorClient client)
    {
        _client = client;
    }

    [HttpPost]
    public async Task<ActionResult> CalculateDate([FromBody] DateRequest request)
    {
        if(request.DayOfMonth < 1 || request.DayOfMonth > 31)
            throw new ValidationException("Invalid day of month");
        
        string dateStr = request.Date?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");
        bool adjust = request.Adjust ?? false;

        var grpcRequest = new DateGrpc.DateRequest
        {
            Date = dateStr,
            DayOfMonth = request.DayOfMonth,
            Adjust = adjust
        };
        return Ok(await _client.GetNextReportDateAsync(grpcRequest));
    }
}