import { useEffect, useState } from 'react';
import { fetchCurrentPrice, fetchPortfolio, placeTrade } from './api';

type Trade = {
  timestamp: string;
  price: number;
  quantity: number;
  actor: string;
  actionType: 'buy' | 'sell';
};

type Portfolio = {
  cash: number;
  electricityUnits: number;
  averageBuyPrice: number;
  tradeHistory: Trade[];
};

const App = () => {
  const [price, setPrice] = useState<number | null>(null);
  const [portfolio, setPortfolio] = useState<Portfolio | null>(null);
  const [tradeQuantity, setTradeQuantity] = useState(10);

  useEffect(() => {
    const loadPrice = async () => {
      try {
        const latest = await fetchCurrentPrice();
        setPrice(latest);
      } catch (err) {
        console.error('Price fetch failed', err);
      }
    };
    loadPrice();
    const interval = setInterval(loadPrice, 1000);
    return () => clearInterval(interval);
  }, []);

  const loadPortfolio = async () => {
    try {
      const data = await fetchPortfolio();
      setPortfolio(data);
    } catch (err) {
      console.error('Portfolio fetch failed', err);
    }
  };

  useEffect(() => {
    loadPortfolio();
  }, []);

  const handleTrade = async (type: 'buy' | 'sell') => {
    try {
      await placeTrade(type, tradeQuantity);
      await loadPortfolio();
    } catch (err) {
      alert(`Trade failed: ${type.toUpperCase()}`);
      console.error(err);
    }
  };

  return (
    <main style={{ padding: '2rem', fontFamily: 'sans-serif' }}>
      <h1>GridSim</h1>
      <p>
        Live Electricity Price:{' '}
        <strong>{price !== null ? `£${price.toFixed(2)}` : 'Loading...'}</strong>
      </p>

      <section style={{ marginTop: '2rem' }}>
        <h2>Trade</h2>
        <input
          type="number"
          value={tradeQuantity}
          onChange={(e) => setTradeQuantity(Number(e.target.value))}
          style={{ marginRight: '1rem' }}
        />
        <button onClick={() => handleTrade('buy')} style={{ marginRight: '0.5rem' }}>
          Buy
        </button>
        <button onClick={() => handleTrade('sell')}>Sell</button>
      </section>

      <section style={{ marginTop: '2rem' }}>
        <h2>Portfolio</h2>
        {portfolio ? (
          <>
            <p>Cash: £{portfolio.cash.toFixed(2)}</p>
            <p>Electricity Units: {portfolio.electricityUnits}</p>
            <p>Average Buy Price: £{portfolio.averageBuyPrice.toFixed(2)}</p>

            <h3>Trade History</h3>
            <ul>
              {portfolio.tradeHistory.slice().reverse().map((t, idx) => (
                <li key={idx}>
                  [{new Date(t.timestamp).toLocaleTimeString()}] {t.actionType.toUpperCase()} {t.quantity} units @ £{t.price.toFixed(2)}
                </li>
              ))}
            </ul>
          </>
        ) : (
          <p>Loading portfolio...</p>
        )}
      </section>
    </main>
  );
};

export default App;