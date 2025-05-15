using GridSim.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register background simulation service
builder.Services.AddSingleton<PriceSimulationService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<PriceSimulationService>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// TEMP endpoint to return live price
app.MapGet("/price/current", (PriceSimulationService sim) =>
{
    return Results.Ok(new { price = sim.CurrentPrice });
});

app.Run();