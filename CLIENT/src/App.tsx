import { useEffect, useMemo, useState } from 'react';
import bgImage from '../review/wcup.jpg';

type MatchDto = {
  home: string;
  away: string;
  homeCode: string;
  awayCode: string;
  date: string;
  homeScore: number;
  awayScore: number;
};

type GroupDto = {
  title: string;
  matches: MatchDto[];
};

type ApiMatchDto = {
  home: string;
  away: string;
  homeCode: string;
  awayCode: string;
  date: string;
};

type PredictionDto = {
  group: string;
  homeTeam: string;
  awayTeam: string;
  homeScore: number;
  awayScore: number;
};

const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5132';

function App() {
  const [username, setUsername] = useState('');
  const [currentUser, setCurrentUser] = useState<string | null>(null);
  const [groups, setGroups] = useState<GroupDto[]>([]);
  const [message, setMessage] = useState('');
  const [showSavedPopup, setShowSavedPopup] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const stored = localStorage.getItem('wc_user');
    if (stored) {
      setCurrentUser(stored);
    }
  }, []);

  useEffect(() => {
    if (!currentUser) {
      return;
    }

    setIsLoading(true);
    Promise.all([
      fetch(`${API_URL}/api/matches`).then(res => res.json()),
      fetch(`${API_URL}/api/predictions?username=${encodeURIComponent(currentUser)}`).then(res => {
        if (!res.ok) {
          return [] as PredictionDto[];
        }
        return res.json() as Promise<PredictionDto[]>;
      })
    ])
      .then(([matchGroups, predictions]) => {
        const normalizedGroups = (matchGroups as any[]).map(group => ({
          title: group.title,
          matches: (group.matches as ApiMatchDto[]).map(match => ({
            home: match.home,
            away: match.away,
            homeCode: match.homeCode,
            awayCode: match.awayCode,
            date: match.date,
            homeScore: 0,
            awayScore: 0
          }))
        })) as GroupDto[];

        predictions.forEach(prediction => {
          const group = normalizedGroups.find(g => g.title === prediction.group);
          group?.matches.forEach(match => {
            if (match.home === prediction.homeTeam && match.away === prediction.awayTeam) {
              match.homeScore = prediction.homeScore;
              match.awayScore = prediction.awayScore;
            }
          });
        });

        setGroups(normalizedGroups);
      })
      .catch(() => {
        setMessage('Sunucuya bağlanırken bir hata oluştu. Lütfen backend çalıştığından emin olun.');
      })
      .finally(() => setIsLoading(false));
  }, [currentUser]);

  const hasPredictions = useMemo(() => groups.some(group => group.matches.length > 0), [groups]);

  const handleLogin = async () => {
    if (!username.trim()) {
      setMessage('Lütfen bir kullanıcı adı girin.');
      return;
    }

    const response = await fetch(`${API_URL}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username: username.trim() })
    });

    if (!response.ok) {
      setMessage('Giriş işlemi başarısız oldu.');
      return;
    }

    const data = await response.json();
    localStorage.setItem('wc_user', data.username);
    setCurrentUser(data.username);
    setUsername('');
    setMessage('');
  };

  const savePredictions = async () => {
    if (!currentUser) {
      setMessage('Önce giriş yapmanız gerekiyor.');
      return;
    }

    const predictions = groups.flatMap(group =>
      group.matches.map(match => ({
        group: group.title,
        homeTeam: match.home,
        awayTeam: match.away,
        homeScore: match.homeScore,
        awayScore: match.awayScore
      }))
    );

    setIsLoading(true);
    const response = await fetch(`${API_URL}/api/predictions`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username: currentUser, predictions })
    });

    if (!response.ok) {
      setMessage('Tahminler kaydedilirken bir hata oluştu.');
      setIsLoading(false);
      return;
    }

    setMessage('Tahminlerin başarıyla kaydedildi!');
    setShowSavedPopup(true);
    window.setTimeout(() => setShowSavedPopup(false), 2500);
    setIsLoading(false);
  };

  const handleScoreChange = (groupIndex: number, matchIndex: number, field: 'homeScore' | 'awayScore', value: number) => {
    setGroups(prev => {
      const next = [...prev];
      next[groupIndex] = {
        ...next[groupIndex],
        matches: next[groupIndex].matches.map((match, index) =>
          index === matchIndex ? { ...match, [field]: value } : match
        )
      };
      return next;
    });
  };

  const renderLogin = () => (
    <div className="login-page">
      <div
        className="bg-image"
        style={{
          backgroundImage: `url(${bgImage})`,
          height: '100vh',
          width: '100vw',
          backgroundPosition: 'center center',
          backgroundRepeat: 'no-repeat',
          backgroundSize: '100% 100%',
          position: 'fixed'
        }}
      />
      <div className="bg-overlay">
        <div className="login-card">
          <h1>
            <span className="w">D</span>ünya <span className="c">K</span>upası <span className="u">2</span><span className="p">026</span>
          </h1>
          <p className="subtitle">Tahminlerine başlamak için kullanıcı adını gir.</p>
          <input
            value={username}
            onChange={e => setUsername(e.target.value)}
            placeholder="⚽ Adınızı buraya yazın..."
          />
          <button onClick={handleLogin}>Karnavala Katıl!</button>
          {message && <div className="message">{message}</div>}
        </div>
      </div>
    </div>
  );

  const renderPredictionPage = () => (
    <div className="app-container">
      <header className="header">
        <div>
          <h1>2026 Grup Maçları Tahminleri</h1>
          <div className="user-info">Tahminci: {currentUser}</div>
        </div>
        <button className="logout-button" onClick={() => { localStorage.removeItem('wc_user'); setCurrentUser(null); setGroups([]); setMessage(''); }}>
          Çıkış Yap
        </button>
      </header>
      <main className="container">
        {isLoading && <div className="status-banner">Yükleniyor...</div>}
        {message && <div className="status-banner">{message}</div>}

        {hasPredictions ? groups.map((group, groupIndex) => (
          <section key={group.title}>
            <div className="group-header">{group.title}</div>
            {group.matches.map((match, matchIndex) => (
              <div className="match-card" key={`${group.title}-${match.home}-${match.away}`}>
                <div className="match-date">{new Date(match.date).toLocaleDateString('tr-TR', { weekday: 'short', day: 'numeric', month: 'long' })} {new Date(match.date).toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' })}</div>
                <div className="team-box home">
                  <span className="team-name">{match.home}</span>
                  <img
                    src={`https://flagcdn.com/w80/${match.homeCode}.png`}
                    alt={match.home}
                    className="flag"
                    onError={e => ((e.target as HTMLImageElement).style.visibility = 'hidden')}
                  />
                </div>
                <div className="score-container">
                  <input
                    type="number"
                    className="score-input"
                    min={0}
                    value={match.homeScore}
                    onChange={e => handleScoreChange(groupIndex, matchIndex, 'homeScore', Number(e.target.value))}
                  />
                  <span className="separator">-</span>
                  <input
                    type="number"
                    className="score-input"
                    min={0}
                    value={match.awayScore}
                    onChange={e => handleScoreChange(groupIndex, matchIndex, 'awayScore', Number(e.target.value))}
                  />
                </div>
                <div className="team-box away">
                  <img
                    src={`https://flagcdn.com/w80/${match.awayCode}.png`}
                    alt={match.away}
                    className="flag"
                    onError={e => ((e.target as HTMLImageElement).style.visibility = 'hidden')}
                  />
                  <span className="team-name">{match.away}</span>
                </div>
              </div>
            ))}
          </section>
        )) : (
          <div className="status-banner">Maç verisi bulunamadı.</div>
        )}
        <div className="save-section">
          <button className="btn-save" disabled={isLoading} onClick={savePredictions}>
            Tahminlerimi Kaydet
          </button>
        </div>

        {showSavedPopup && (
          <div className="popup-overlay">
            <div className="popup-card">
              <h2>Tahminleriniz kaydedildi!</h2>
              <p>Verileriniz güvenli bir şekilde saklandı.</p>
            </div>
          </div>
        )}
      </main>
    </div>
  );

  return currentUser ? renderPredictionPage() : renderLogin();
}

export default App;
