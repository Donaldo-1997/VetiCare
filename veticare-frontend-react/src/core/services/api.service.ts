const BASE_URL = import.meta.env.VITE_API_URL ?? 'https://localhost:7001/api';
const TOKEN_KEY = 'veticare_token';

async function request<T>(
  method: string,
  endpoint: string,
  body?: unknown,
  params?: Record<string, string>
): Promise<T> {
  let url = `${BASE_URL}/${endpoint}`;
  if (params) {
    const qs = Object.entries(params)
      .filter(([, v]) => v !== '' && v !== null && v !== undefined)
      .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(v)}`)
      .join('&');
    if (qs) url += `?${qs}`;
  }

  const token = localStorage.getItem(TOKEN_KEY);
  const headers: Record<string, string> = { 'Content-Type': 'application/json' };
  if (token) headers['Authorization'] = `Bearer ${token}`;

  const res = await fetch(url, {
    method,
    headers,
    body: body !== undefined ? JSON.stringify(body) : undefined,
  });

  if (!res.ok) {
    const text = await res.text().catch(() => '');
    let message = `Error ${res.status}`;
    try {
      const json = JSON.parse(text);
      // Extrae mensajes de validación de ValidationProblemDetails:
      // { errors: { "Field": ["msg1", "msg2"], ... } }
      const validationMessages: string | null = json.errors && typeof json.errors === 'object' && !Array.isArray(json.errors)
        ? (Object.values(json.errors) as string[][]).flat().join('. ')
        : null;
      // Soporta: { message }, ValidationProblemDetails, { detail }, { title }
      message = json.message || validationMessages || json.detail || json.title || message;
    } catch {
      if (text) message = text;
    }
    throw new Error(message);
  }

  if (res.status === 204) return undefined as T;
  return res.json() as Promise<T>;
}

export const api = {
  get:    <T>(endpoint: string, params?: Record<string, string>) =>
    request<T>('GET', endpoint, undefined, params),
  post:   <T>(endpoint: string, body: unknown) => request<T>('POST', endpoint, body),
  put:    <T>(endpoint: string, body: unknown) => request<T>('PUT', endpoint, body),
  patch:  <T>(endpoint: string, body: unknown) => request<T>('PATCH', endpoint, body),
  delete: <T>(endpoint: string) => request<T>('DELETE', endpoint),
};
