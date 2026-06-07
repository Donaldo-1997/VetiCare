import { api } from './api.service';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
}

export const authService = {
  login:    (body: LoginRequest)    => api.post<AuthResponse>('Auth/login', body),
  register: (body: RegisterRequest) => api.post<{ message: string }>('Auth/register', body),
};
