import { useState, useEffect } from 'react'
import './App.css'

interface Champion {
  id: number;
  name: string;
  role: string;
  imageUrl: string;
}

function App() {
  const [champions, setChampions] = useState<Champion[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const apiUrl = import.meta.env.VITE_API_URL || '/api';

  useEffect(() => {
    const fetchChampions = async () => {
      try {
        const response = await fetch(`${apiUrl}/champions`);
        if (!response.ok) {
          throw new Error('Failed to fetch champions');
        }
        const data = await response.json();
        setChampions(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'An unknown error occurred');
      } finally {
        setLoading(false);
      }
    };

    fetchChampions();
  }, [apiUrl]);

  return (
    <div className="container">
      <h1>League of Legends Champions</h1>
      
      {loading && <p>Loading champions...</p>}
      
      {error && <p className="error">Error: {error}</p>}
      
      {!loading && !error && (
        <div className="champion-grid">
          {champions.map((champion) => (
            <div key={champion.id} className="champion-card">
              <img 
                src={champion.imageUrl} 
                alt={champion.name} 
                className="champion-image"
                onError={(e) => {
                  (e.target as HTMLImageElement).src = 'https://via.placeholder.com/150?text=No+Image';
                }}
              />
              <div className="champion-info">
                <h3>{champion.name}</h3>
                <p className="champion-role">{champion.role}</p>
              </div>
            </div>
          ))}
        </div>
      )}
      
      {!loading && !error && champions.length === 0 && (
        <p>No champions found.</p>
      )}
    </div>
  )
}

export default App
