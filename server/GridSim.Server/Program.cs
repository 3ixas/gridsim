using GridSim.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Register services
builder.Services.AddSingleton<PriceSimulationService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<PriceSimulationService>());
builder.Services.AddSingleton<TradeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();