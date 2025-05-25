using DateGrpc;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddSwaggerGen();
services.AddGrpc();

builder.Services.AddGrpcClient<DateCalculator.DateCalculatorClient>(o =>
{
    o.Address = new Uri("http://localhost:5000"); // gRPC Server URL
});

// Для работы с консольным приложением (не HTTP/2)
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();