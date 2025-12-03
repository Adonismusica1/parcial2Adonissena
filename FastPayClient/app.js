const API = "http://localhost:5000";

async function postJson(url, data) {
  const res = await fetch(API + url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  });
  return res.json().catch(() => ({}));
}

async function getJson(url){
  const res = await fetch(API + url);
  return res.json().catch(()=>({}));
}

document.getElementById('btnRegister').onclick = async () => {
  const dto = {
    Name: document.getElementById('regName').value,
    Phone: document.getElementById('regPhone').value,
    Email: document.getElementById('regEmail').value,
    Pin: document.getElementById('regPin').value
  };
  const r = await postJson('/api/users', dto);
  document.getElementById('regResult').innerText = r.Error ? ('Error: '+r.Error) : ('Registrado: '+JSON.stringify(r));
};

document.getElementById('btnQuery').onclick = async () => {
  const phone = document.getElementById('qPhone').value;
  const r = await getJson(`/api/users/${encodeURIComponent(phone)}`);
  document.getElementById('userResult').innerText = JSON.stringify(r, null, 2);
};

document.getElementById('btnSend').onclick = async () => {
  const dto = {
    FromPhone: document.getElementById('fromPhone').value,
    ToPhone: document.getElementById('toPhone').value,
    Amount: parseFloat(document.getElementById('amount').value || '0'),
    Note: document.getElementById('note').value,
    Pin: document.getElementById('fromPin').value
  };
  const r = await postJson('/api/payments/send', dto);
  document.getElementById('sendResult').innerText = r.Error ? ('Error: '+r.Error) : ('OK: '+JSON.stringify(r));
};

document.getElementById('btnHistory').onclick = async () => {
  const phone = document.getElementById('histPhone').value;
  const r = await getJson(`/api/transactions/${encodeURIComponent(phone)}`);
  document.getElementById('histResult').innerText = JSON.stringify(r, null, 2);
};
