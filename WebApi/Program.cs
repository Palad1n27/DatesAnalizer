using DateGrpc;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddSwaggerGen();
services.AddGrpc();
var dateCalcAddress = builder.Configuration["DateCalcAddress"];
builder.Services.AddGrpcClient<DateCalculator.DateCalculatorClient>(o =>
{
    o.Address = new Uri(dateCalcAddress ?? throw new Exception("Connection string is null"));
});

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddlewareV1>();
app.MapControllers();
app.Run();