using System.Globalization;
using DateGrpc;
using Grpc.Core;

namespace ServerGRPC;

public class DateCalculatorService : DateCalculator.DateCalculatorBase
{
    public override Task<DateResponse> GetNextReportDate(DateRequest request, ServerCallContext context)
    {
        DateTime newDate;
        var baseDate = DateTime.ParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture); 
        try
        {
            newDate = new DateTime(baseDate.Year, baseDate.Month+1, request.DayOfMonth);
            return Task.FromResult(new DateResponse { NextDate = newDate.ToString("yyyy-MM-dd") });
        }
        catch (ArgumentOutOfRangeException)
        {
            if (request.Adjust)
            {
                var theLastMonthDay = DateTime.DaysInMonth(baseDate.Year, baseDate.Month+1);
                var resultDate = new DateTime(baseDate.Year, baseDate.Month+1, theLastMonthDay);
                return Task.FromResult(new DateResponse { NextDate = resultDate.ToString("yyyy-MM-dd") });
            }
            else
            {
                var resultDate = new DateTime(baseDate.Year, baseDate.Month + 1, 1);
                return Task.FromResult(new DateResponse { NextDate = resultDate.ToString("yyyy-MM-dd") });
            }
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument, 
                $"Invalid date format. Expected YYYY-MM-DD. Error: {e.Message}"));
        }
    }
}