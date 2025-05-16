using GridSim.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register background simulation service
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

// TEMP endpoint to return live price
app.MapGet("/price/current", (PriceSimulationService sim) =>
{
    return Results.Ok(new { price = sim.CurrentPrice });
});

app.MapPost("/trade/buy", (TradeService tradeService, int quantity) =>
{
    if (tradeService.Buy(quantity, out var trade))
        return Results.Ok(trade);
    return Results.BadRequest(new { error = "Insufficient funds" });
});

app.MapPost("/trade/sell", (TradeService tradeService, int quantity) =>
{
    if (tradeService.Sell(quantity, out var trade))
        return Results.Ok(trade);
    return Results.BadRequest(new { error = "Insufficient holdings" });
});

app.MapGet("/portfolio", (TradeService tradeService) =>
{
    return Results.Ok(tradeService.GetPortfolio());
});

app.Run();