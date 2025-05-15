namespace GridSim.Server.Models;

public class TradeRecord
{
    public DateTime Timestamp { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Actor { get; set; } = string.Empty; // e.g. "user" or bot name
    public string ActionType { get; set; } = string.Empty; // "buy" or "sell"
}