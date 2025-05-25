namespace WebApi.Requests;

public class DateRequest
{
    public required int DayOfMonth { get; set; }
    public DateTime? Date { get; set; } = DateTime.Today;
    public bool? Adjust { get; set; }
}