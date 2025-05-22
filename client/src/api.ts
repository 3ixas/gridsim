import axios from 'axios';

const API_BASE = 'http://localhost:5189';

export const fetchCurrentPrice = async (): Promise<number> => {
  const res = await axios.get(`${API_BASE}/price/current`);
  return res.data.price;
};

export const fetchPortfolio = async () => {
  const res = await axios.get(`${API_BASE}/api/trade/portfolio`);
  return res.data;
};

export const placeTrade = async (type: 'buy' | 'sell', quantity: number) => {
  const res = await axios.post(`${API_BASE}/api/trade/${type}?quantity=${quantity}`);
  return res.data;
};