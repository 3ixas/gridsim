using GridSim.Server.Models;

namespace GridSim.Server.Services;

public class TradeService
{
    private readonly PriceSimulationService _priceService;
    private readonly UserPortfolio _portfolio = new();

    public TradeService(PriceSimulationService priceService)
    {
        _priceService = priceService;
    }

    public UserPortfolio GetPortfolio()
    {
        return _portfolio;
    }

    public bool Buy(int quantity, out TradeRecord? trade)
    {
        var price = _priceService.CurrentPrice;
        var totalCost = quantity * price;

        if (_portfolio.Cash < totalCost)
        {
            trade = null;
            return false;
        }

        _portfolio.Cash -= totalCost;
        _portfolio.ElectricityUnits += quantity;
        _portfolio.AverageBuyPrice = RecalculateAverageBuy(price, quantity);

        trade = new TradeRecord
        {
            Timestamp = DateTime.UtcNow,
            Price = price,
            Quantity = quantity,
            Actor = "user",
            ActionType = "buy"
        };

        _portfolio.TradeHistory.Add(trade);
        return true;
    }

    public bool Sell(int quantity, out TradeRecord? trade)
    {
        if (_portfolio.ElectricityUnits < quantity)
        {
            trade = null;
            return false;
        }

        var price = _priceService.CurrentPrice;
        var totalRevenue = quantity * price;

        _portfolio.Cash += totalRevenue;
        _portfolio.ElectricityUnits -= quantity;

        if (_portfolio.ElectricityUnits == 0)
            _portfolio.AverageBuyPrice = 0;

        trade = new TradeRecord
        {
            Timestamp = DateTime.UtcNow,
            Price = price,
            Quantity = quantity,
            Actor = "user",
            ActionType = "sell"
        };

        _portfolio.TradeHistory.Add(trade);
        return true;
    }

    private decimal RecalculateAverageBuy(decimal newPrice, int newQty)
    {
        var totalUnits = _portfolio.ElectricityUnits;
        var oldValue = _portfolio.AverageBuyPrice * (totalUnits - newQty);
        var newValue = newPrice * newQty;
        return Math.Round((oldValue + newValue) / totalUnits, 2);
    }
}