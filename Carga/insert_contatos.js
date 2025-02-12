import http from 'k6/http';
import { check, sleep } from 'k6';
import { uuidv4 } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';

export let options = {
  vus: 100, // 50 usuários simultâneos
  iterations: 10000, // Total de 1000 requisições
};

export default function () {
  let url = 'http://localhost:31146/api/Contato/inserir-contato';

  let payload = JSON.stringify({
    id: uuidv4(), // ID único para cada requisição
    dataCriacao: new Date().toISOString(), // Data dinâmica
    nome: `usuario_${Math.floor(Math.random() * 10000)}`, // Nome aleatório
    idade: Math.floor(Math.random() * 60) + 18, // Idade entre 18 e 78
    email: `usuario${Math.floor(Math.random() * 10000)}@teste.com`, // Email único
    telefone: (Math.floor(Math.random() * 900000000) + 100000000).toString(), // Telefone aleatório
    codigoDdd: 12 // Código DDD entre 10 e 99
  });

  console.log(payload);

  let params = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  let res = http.post(url, payload, params);

  check(res, {
    'status is 200': (r) => r.status === 200,
    'response time is < 500ms': (r) => r.timings.duration < 500,
  });

  sleep(0.1); 
}
