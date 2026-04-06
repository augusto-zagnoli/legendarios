import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface LoginResponse {
  token: string;
  idUsuario: number;
  nomeUsuario: string;
  nivelPermissao: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USER_KEY = 'auth_user';
  private readonly baseUrl = environment.baseURL;

  constructor(private http: HttpClient) {}

  login(login: string, password: string, rememberUser: boolean): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}adm-login`, { login, password, rememberUser }).pipe(
      tap(response => this.salvarSessao(response, rememberUser))
    );
  }

  private salvarSessao(response: LoginResponse, rememberUser: boolean): void {
    const storage = rememberUser ? localStorage : sessionStorage;
    storage.setItem(this.TOKEN_KEY, response.token);
    storage.setItem(this.USER_KEY, JSON.stringify(response));
    // Compatibilidade com código legado que usa PO_USER_LOGIN
    sessionStorage.setItem('PO_USER_LOGIN', JSON.stringify({ id_usuario: response.idUsuario }));
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.USER_KEY);
    sessionStorage.removeItem('PO_USER_LOGIN');
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY) || sessionStorage.getItem(this.TOKEN_KEY);
  }

  getUsuario(): LoginResponse | null {
    const data = localStorage.getItem(this.USER_KEY) || sessionStorage.getItem(this.USER_KEY);
    return data ? JSON.parse(data) : null;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp > Math.floor(Date.now() / 1000);
    } catch {
      return false;
    }
  }
}
