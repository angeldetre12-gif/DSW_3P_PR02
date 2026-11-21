'use strict';

const DEMO_MISSIONS = [
  { id: 1, nombre: 'Rescatar a Pocho', dificultad: 'Difícil', recompensaXP: 2000, completada: false },
  { id: 2, nombre: 'Vencer a Barba', dificultad: 'Media', recompensaXP: 400, completada: true },
  { id: 3, nombre: 'Cruzar el bosque lunar', dificultad: 'Fácil', recompensaXP: 750, completada: false }
];

const state = {
  baseUrl: localStorage.getItem('animission_api_url') || '',
  token: '',
  missions: [...DEMO_MISSIONS],
  demo: true,
  endpoint: '/api/misiones'
};

const $ = (id) => document.getElementById(id);
const pick = (obj, ...keys) => keys.map((key) => obj?.[key]).find((value) => value !== undefined);
const cleanUrl = (url) => url.trim().replace(/\/+$/, '');

function show(id) { $(id).classList.add('show'); }
function hide(id) { $(id).classList.remove('show'); }
function notify(message, error = false) {
  const toast = $('toast');
  toast.textContent = message;
  toast.className = 'toast show' + (error ? ' error' : '');
  clearTimeout(notify.timer);
  notify.timer = setTimeout(() => toast.classList.remove('show'), 4200);
}

async function api(path, options = {}) {
  const headers = {
    Accept: 'application/json',
    ...(options.body ? { 'Content-Type': 'application/json' } : {}),
    ...(state.token ? { Authorization: 'Bearer ' + state.token } : {})
  };
  let response;
  try {
    response = await fetch(cleanUrl(state.baseUrl) + path, { ...options, headers });
  } catch (error) {
    throw new Error('No se pudo conectar con la API: ' + error.message);
  }
  if (response.status === 401) {
    state.token = '';
    throw new Error('Sesión expirada. Inicia sesión nuevamente.');
  }
  if (!response.ok) throw new Error('HTTP ' + response.status + ': ' + (await response.text()).slice(0, 180));
  if (response.status === 204) return null;
  const text = await response.text();
  return text ? JSON.parse(text) : null;
}

function render() {
  const doneCount = state.missions.filter((item) => Boolean(pick(item, 'completada', 'Completada'))).length;
  const xp = state.missions
    .filter((item) => Boolean(pick(item, 'completada', 'Completada')))
    .reduce((sum, item) => sum + Number(pick(item, 'recompensaXP', 'RecompensaXP') || 0), 0);

  $('total').textContent = state.missions.length;
  $('done').textContent = doneCount;
  $('pending').textContent = state.missions.length - doneCount;
  $('xp').textContent = xp >= 1000 ? (xp / 1000).toFixed(1).replace('.0', '') + 'K' : xp;
  $('heroDone').textContent = state.missions.length ? Math.round(doneCount / state.missions.length * 100) + '%' : '0%';

  const query = $('search').value.trim().toLowerCase();
  const filter = $('filter').value;
  const filtered = state.missions.filter((item) => {
    const done = Boolean(pick(item, 'completada', 'Completada'));
    const name = String(pick(item, 'nombre', 'Nombre') || '').toLowerCase();
    return name.includes(query) && (filter === 'all' || (filter === 'done' && done) || (filter === 'pending' && !done));
  });

  const root = $('missions');
  root.replaceChildren();
  if (!filtered.length) {
    root.innerHTML = '<div class="empty"><strong>Sin misiones disponibles</strong><br><small>Cambia el filtro o agrega una nueva misión.</small></div>';
    return;
  }

  filtered.forEach((item) => {
    const id = pick(item, 'id', 'Id');
    const name = String(pick(item, 'nombre', 'Nombre') || 'Sin nombre');
    const difficulty = String(pick(item, 'dificultad', 'Dificultad') || 'Media');
    const reward = Number(pick(item, 'recompensaXP', 'RecompensaXP') || 0);
    const done = Boolean(pick(item, 'completada', 'Completada'));
    const difficultyKey = difficulty.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase();
    const card = document.createElement('article');
    card.className = 'mission-card' + (done ? ' done' : '');
    card.innerHTML = `<div class="mission-top"><span class="tag ${difficultyKey}">${difficulty}</span><span>${done ? '✓ COMPLETA' : '◌ PENDIENTE'}</span></div>
      <h3></h3><p>${done ? 'Objetivo cumplido por el escuadrón.' : 'Reúne a tu equipo y acepta el desafío.'}</p>
      <div class="mission-footer"><span class="xp">◆ ${reward.toLocaleString('es-MX')} XP</span><div class="card-actions"><button class="edit">Editar</button><button class="delete">Eliminar</button></div></div>`;
    card.querySelector('h3').textContent = name;
    card.querySelector('.edit').addEventListener('click', () => openMission(item));
    card.querySelector('.delete').addEventListener('click', () => deleteMission(id, name));
    root.append(card);
  });
}

async function loadMissions() {
  if (!state.baseUrl) {
    state.demo = true;
    state.missions = [...DEMO_MISSIONS];
    $('apiStatus').textContent = 'Modo demostración';
    $('statusDot').className = '';
    render();
    return;
  }
  try {
    let data;
    try {
      data = await api('/api/misiones');
      state.endpoint = '/api/misiones';
    } catch {
      data = await api('/api/eventos');
      state.endpoint = '/api/eventos';
    }
    if (!Array.isArray(data)) throw new Error('La API no devolvió una lista.');
    state.demo = false;
    state.missions = state.endpoint === '/api/eventos'
      ? data.map((item) => ({
          id: item.id ?? item.Id,
          nombre: item.nombre ?? item.Nombre,
          dificultad: item.categoria ?? item.Categoria ?? 'Media',
          recompensaXP: item.cupoMaximo ?? item.CupoMaximo ?? 0,
          completada: item.activo ?? item.Activo ?? false
        }))
      : data;
    $('apiStatus').textContent = 'API conectada';
    $('statusDot').className = 'online';
    render();
  } catch (error) {
    notify(error.message, true);
  }
}

function openMission(item = null) {
  $('missionForm').reset();
  $('missionId').value = item ? String(pick(item, 'id', 'Id')) : '';
  $('missionTitle').textContent = item ? 'Editar misión' : 'Nueva misión';
  $('missionName').value = item ? String(pick(item, 'nombre', 'Nombre') || '') : '';
  $('missionDifficulty').value = item ? String(pick(item, 'dificultad', 'Dificultad') || 'Media') : 'Fácil';
  $('missionXp').value = item ? String(pick(item, 'recompensaXP', 'RecompensaXP') || 0) : '100';
  $('missionDone').checked = item ? Boolean(pick(item, 'completada', 'Completada')) : false;
  show('missionModal');
}

async function deleteMission(id, name) {
  if (!confirm(`¿Eliminar la misión "${name}"?`)) return;
  if (state.demo) {
    state.missions = state.missions.filter((item) => pick(item, 'id', 'Id') !== id);
    render();
    notify('Misión eliminada en modo demostración.');
    return;
  }
  try {
    await api(state.endpoint + '/' + encodeURIComponent(id), { method: 'DELETE' });
    await loadMissions();
    notify('Misión eliminada.');
  } catch (error) { notify(error.message, true); }
}

$('missionForm').addEventListener('submit', async (event) => {
  event.preventDefault();
  const id = $('missionId').value;
  const mission = {
    nombre: $('missionName').value.trim(),
    dificultad: $('missionDifficulty').value,
    recompensaXP: Number($('missionXp').value),
    completada: $('missionDone').checked
  };

  if (state.demo) {
    if (id) {
      state.missions = state.missions.map((item) =>
        String(pick(item, 'id', 'Id')) === id ? { ...item, ...mission } : item
      );
      notify('Misión actualizada en modo demostración.');
    } else {
      state.missions.unshift({ id: Date.now(), ...mission });
      notify('Misión creada en modo demostración.');
    }
    hide('missionModal');
    render();
    return;
  }

  try {
    const body = state.endpoint === '/api/eventos'
      ? {
          id: id ? Number(id) : 0,
          nombre: mission.nombre,
          categoria: mission.dificultad,
          fecha: new Date().toISOString().slice(0, 10),
          lugar: 'Por definir',
          cupoMaximo: mission.recompensaXP || 100,
          activo: mission.completada
        }
      : mission;
    await api(state.endpoint + (id ? '/' + encodeURIComponent(id) : ''), {
      method: id ? 'PUT' : 'POST',
      body: JSON.stringify(body)
    });
    hide('missionModal');
    await loadMissions();
    notify(id ? 'Misión actualizada.' : 'Misión creada.');
  } catch (error) {
    notify(error.message, true);
  }
});

$('loginForm').addEventListener('submit', async (event) => {
  event.preventDefault();
  if (!state.baseUrl) {
    hide('loginModal');
    show('configModal');
    notify('Primero configura la URL de tu API.', true);
    return;
  }
  try {
    const result = await api('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify({
        usuario: $('username').value,
        password: $('password').value
      })
    });
    const token = pick(result, 'access_token', 'accessToken', 'token');
    if (!token) throw new Error('La respuesta no contiene un token.');
    state.token = token;
    hide('loginModal');
    notify('Sesión iniciada.');
    await loadMissions();
  } catch (error) {
    notify(error.message, true);
  }
});

$('configForm').addEventListener('submit', (event) => {
  event.preventDefault();
  try {
    const url = new URL(cleanUrl($('baseUrl').value));
    if (!['http:', 'https:'].includes(url.protocol) || url.search || url.hash) {
      throw new Error();
    }
    state.baseUrl = url.origin;
    state.token = '';
    localStorage.setItem('animission_api_url', state.baseUrl);
    hide('configModal');
    notify('API guardada. Ahora inicia sesión.');
    loadMissions();
  } catch {
    notify('Introduce una URL válida, por ejemplo https://tu-api.com', true);
  }
});

$('configBtn').addEventListener('click', () => {
  $('baseUrl').value = state.baseUrl;
  show('configModal');
});

$('authBtn').addEventListener('click', () => {
  if (state.token) {
    state.token = '';
    state.demo = true;
    state.missions = [...DEMO_MISSIONS];
    $('apiStatus').textContent = 'Modo demostración';
    $('statusDot').className = '';
    render();
    notify('Sesión cerrada.');
  } else {
    show('loginModal');
  }
});

$('newBtn').addEventListener('click', () => openMission());
$('search').addEventListener('input', render);
$('filter').addEventListener('change', render);

document.querySelectorAll('[data-close]').forEach((button) => {
  button.addEventListener('click', () => hide(button.dataset.close));
});
document.querySelectorAll('.overlay').forEach((overlay) => {
  overlay.addEventListener('click', (event) => {
    if (event.target === overlay) hide(overlay.id);
  });
});
document.addEventListener('keydown', (event) => {
  if (event.key === 'Escape') {
    document.querySelectorAll('.overlay.show').forEach((overlay) => hide(overlay.id));
  }
});

if (state.baseUrl) $('baseUrl').value = state.baseUrl;
render();
