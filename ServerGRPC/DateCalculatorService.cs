using System.Globalization;
using DateGrpc;
using Grpc.Core;

namespace ServerGRPC;

public class DateCalculatorService : DateCalculator.DateCalculatorBase
{
    public override Task<DateResponse> GetNextReportDate(DateRequest request, ServerCallContext context)
    {
        try
        {
            var baseDate = DateTime.ParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var targetMonth = baseDate.AddMonths(1);

            DateTime resultDate;
            int daysInTargetMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);

            if (request.DayOfMonth <= daysInTargetMonth)
            {
                resultDate = new DateTime(targetMonth.Year, targetMonth.Month, request.DayOfMonth);
            }
            else if (request.Adjust)
            {
                resultDate = new DateTime(targetMonth.Year, targetMonth.Month, daysInTargetMonth);
            }
            else
            {
                var nextMonth = baseDate.AddMonths(2);
                resultDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            }

            return Task.FromResult(new DateResponse
            {
                NextDate = resultDate.ToString("yyyy-MM-dd")
            });
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                $"Invalid request. Error: {e.Message}"
            ));
        }
    }
}