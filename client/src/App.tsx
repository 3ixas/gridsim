import { useEffect, useState } from 'react';
import { fetchCurrentPrice } from './api';

const App = () => {
  const [price, setPrice] = useState<number | null>(null);

  useEffect(() => {
    const loadPrice = async () => {
      try {
        const latest = await fetchCurrentPrice();
        setPrice(latest);
      } catch (error) {
        console.error("Failed to fetch price:", error);
      }
    };

    loadPrice();

    const interval = setInterval(loadPrice, 1000);
    return () => clearInterval(interval);
  }, []);

  return (
    <main style={{ padding: "2rem", fontFamily: "sans-serif" }}>
      <h1>GridSim</h1>
      <p>Live Electricity Price: <strong>{price !== null ? `£${price.toFixed(2)}` : 'Loading...'}</strong></p>
    </main>
  );
};

export default App;