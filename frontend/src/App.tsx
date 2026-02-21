import { useState, useEffect } from 'react'
import './App.css'

interface Champion {
  id: number;
  name: string;
  role: string;
  imageUrl: string;
}

interface ChampionFormData {
  name: string;
  role: string;
  imageUrl: string;
}

const initialFormData: ChampionFormData = {
  name: '',
  role: '',
  imageUrl: ''
};

function App() {
  const [champions, setChampions] = useState<Champion[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<ChampionFormData>(initialFormData);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [isFormVisible, setIsFormVisible] = useState(false);

  const apiUrl = import.meta.env.VITE_API_URL || '/api';

  useEffect(() => {
    fetchChampions();
  }, [apiUrl]);

  const fetchChampions = async () => {
    try {
      const response = await fetch(`${apiUrl}/champions`);
      if (!response.ok) {
        throw new Error('Failed to fetch champions');
      }
      const data = await response.json();
      setChampions(data);
      setError(null);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An unknown error occurred');
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const url = editingId ? `${apiUrl}/champions/${editingId}` : `${apiUrl}/champions`;
      const method = editingId ? 'PUT' : 'POST';
      
      const payload = editingId ? { ...formData, id: editingId } : formData;
      
      // Use default image if none provided
      if (!payload.imageUrl || payload.imageUrl.trim() === '') {
        payload.imageUrl = 'https://ddragon.leagueoflegends.com/cdn/img/champion/splash/Aatrox_0.jpg'; // Default placeholder
      }

      const response = await fetch(url, {
        method,
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        throw new Error(`Failed to ${editingId ? 'update' : 'create'} champion`);
      }

      await fetchChampions();
      resetForm();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An error occurred during save');
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this champion?')) return;
    
    try {
      const response = await fetch(`${apiUrl}/champions/${id}`, {
        method: 'DELETE',
      });

      if (!response.ok) {
        throw new Error('Failed to delete champion');
      }

      await fetchChampions();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An error occurred during delete');
    }
  };

  const startEdit = (champion: Champion) => {
    setFormData({
      name: champion.name,
      role: champion.role,
      imageUrl: champion.imageUrl
    });
    setEditingId(champion.id);
    setIsFormVisible(true);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const resetForm = () => {
    setFormData(initialFormData);
    setEditingId(null);
    setIsFormVisible(false);
  };

  return (
    <div className="container">
      <h1>League of Legends Champions</h1>
      
      <button 
        className="add-button"
        onClick={() => {
          resetForm();
          setIsFormVisible(!isFormVisible);
        }}
      >
        {isFormVisible ? 'Cancel' : 'Add New Champion'}
      </button>

      {isFormVisible && (
        <form onSubmit={handleSubmit} className="champion-form">
          <h2>{editingId ? 'Edit Champion' : 'Add New Champion'}</h2>
          <div className="form-group">
            <label htmlFor="name">Name:</label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="role">Role:</label>
            <select
              id="role"
              name="role"
              value={formData.role}
              onChange={handleInputChange}
              required
            >
              <option value="">Select Role</option>
              <option value="Assassin">Assassin</option>
              <option value="Fighter">Fighter</option>
              <option value="Mage">Mage</option>
              <option value="Marksman">Marksman</option>
              <option value="Support">Support</option>
              <option value="Tank">Tank</option>
            </select>
          </div>
          <div className="form-group">
            <label htmlFor="imageUrl">Image URL (Optional):</label>
            <input
              type="text"
              id="imageUrl"
              name="imageUrl"
              value={formData.imageUrl}
              onChange={handleInputChange}
              placeholder="https://example.com/image.jpg (or leave empty for default)"
            />
          </div>
          <div className="form-actions">
            <button type="submit" className="save-button">
              {editingId ? 'Update' : 'Create'}
            </button>
            <button type="button" onClick={resetForm} className="cancel-button">
              Cancel
            </button>
          </div>
        </form>
      )}
      
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
                <div className="card-actions">
                  <button onClick={() => startEdit(champion)} className="edit-button">Edit</button>
                  <button onClick={() => handleDelete(champion.id)} className="delete-button">Delete</button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
      
      {!loading && !error && champions.length === 0 && !isFormVisible && (
        <p>No champions found. Click "Add New Champion" to get started!</p>
      )}
    </div>
  )
}

export default App
