import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface LoginResponse {
  token: string;
  idUsuario: number;
  nomeUsuario: string;
  nivelPermissao: number;
}

export interface AuthResponse {
  sucesso: boolean;
  data: {
    token: string;
    refreshToken: string;
    idUsuario: number;
    nomeUsuario: string;
    nivelPermissao: number;
    role: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly REFRESH_KEY = 'refresh_token';
  private readonly USER_KEY = 'auth_user';
  private readonly baseUrl = environment.baseURL;
  private readonly apiUrl = environment.apiURL;

  private usuarioSubject = new BehaviorSubject<any>(this.getUsuario());
  usuario$ = this.usuarioSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(login: string, password: string, rememberUser: boolean): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}adm-login`, { login, password, rememberUser }).pipe(
      tap(response => this.salvarSessao(response, rememberUser))
    );
  }

  loginV2(login: string, password: string, rememberUser: boolean): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}auth/login`, { login, password, rememberUser }).pipe(
      tap(response => {
        if (response.sucesso && response.data) {
          this.salvarSessaoV2(response.data, rememberUser);
        }
      })
    );
  }

  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    const token = this.getToken();
    return this.http.post<AuthResponse>(`${this.apiUrl}auth/refresh-token`, {
      Token: token,
      RefreshToken: refreshToken
    }).pipe(
      tap(response => {
        if (response.sucesso && response.data) {
          this.salvarSessaoV2(response.data, !!localStorage.getItem(this.TOKEN_KEY));
        }
      })
    );
  }

  private salvarSessao(response: any, rememberUser: boolean): void {
    const storage = rememberUser ? localStorage : sessionStorage;
    const token = response.token || response.Token;
    const idUsuario = response.idUsuario ?? response.IdUsuario;
    const nomeUsuario = response.nomeUsuario || response.NomeUsuario;
    const nivelPermissao = response.nivelPermissao ?? response.NivelPermissao ?? 0;
    const refreshToken = response.refreshToken || response.RefreshToken;
    const role = response.role || response.Role || 'Participante';

    if (token) {
      storage.setItem(this.TOKEN_KEY, token);
    }
    if (refreshToken) {
      storage.setItem(this.REFRESH_KEY, refreshToken);
    }

    const user = { token, idUsuario, nomeUsuario, nivelPermissao, role };
    storage.setItem(this.USER_KEY, JSON.stringify(user));
    sessionStorage.setItem('PO_USER_LOGIN', JSON.stringify({ id_usuario: idUsuario }));
    this.usuarioSubject.next(user);
  }

  private salvarSessaoV2(data: AuthResponse['data'], rememberUser: boolean): void {
    const storage = rememberUser ? localStorage : sessionStorage;
    storage.setItem(this.TOKEN_KEY, data.token);
    storage.setItem(this.REFRESH_KEY, data.refreshToken);
    const user = {
      token: data.token,
      idUsuario: data.idUsuario,
      nomeUsuario: data.nomeUsuario,
      nivelPermissao: data.nivelPermissao,
      role: data.role
    };
    storage.setItem(this.USER_KEY, JSON.stringify(user));
    sessionStorage.setItem('PO_USER_LOGIN', JSON.stringify({ id_usuario: data.idUsuario }));
    this.usuarioSubject.next(user);
  }

  logout(): void {
    const refreshToken = this.getRefreshToken();
    if (refreshToken) {
      this.http.post(`${this.apiUrl}auth/revoke-token`, { RefreshToken: refreshToken }).subscribe();
    }
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_KEY);
    localStorage.removeItem(this.USER_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.REFRESH_KEY);
    sessionStorage.removeItem(this.USER_KEY);
    sessionStorage.removeItem('PO_USER_LOGIN');
    this.usuarioSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY) || sessionStorage.getItem(this.TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_KEY) || sessionStorage.getItem(this.REFRESH_KEY);
  }

  getUsuario(): any {
    const data = localStorage.getItem(this.USER_KEY) || sessionStorage.getItem(this.USER_KEY);
    return data ? JSON.parse(data) : null;
  }

  getRole(): string {
    const user = this.getUsuario();
    return user?.role || user?.Role || 'Participante';
  }

  getNivelPermissao(): number {
    const user = this.getUsuario();
    return user?.nivelPermissao ?? user?.NivelPermissao ?? 0;
  }

  isAdmin(): boolean {
    return this.getNivelPermissao() === 1;
  }

  isLider(): boolean {
    return this.getNivelPermissao() === 2;
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
