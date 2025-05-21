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
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
// app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/price/current", (PriceSimulationService sim) =>
{
    return Results.Ok(new { price = sim.CurrentPrice });
});

app.Run();