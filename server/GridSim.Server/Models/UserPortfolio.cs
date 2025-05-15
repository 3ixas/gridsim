namespace GridSim.Server.Models;

public class UserPortfolio
{
    public decimal Cash { get; set; } = 10000m; // Starting balance
    public int ElectricityUnits { get; set; } = 0;
    public decimal AverageBuyPrice { get; set; } = 0m;
    public List<TradeRecord> TradeHistory { get; set; } = new();
}