import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface CadastroPublicoDTO {
  nome: string;
  email: string;
  celular: string;
  cadastro_pessoa: string;
  data_de_nascimento: string;
  estado_civil: string;
  profissao: string;
  tipo_sanguineo: string;
  cnh?: string;
  categoria_cnh?: string;
  endereco?: string;
  cidade?: string;
  estado?: string;
  cep?: string;
  pais?: string;
  emergencia_nome?: string;
  emergencia_telefone?: string;
  tamanho_camiseta?: string;
  observacoes?: string;
}

export interface EstatisticasDashboard {
  total: number;
  pendentes: number;
  aprovados: number;
  reprovados: number;
}

@Injectable({ providedIn: 'root' })
export class LegendariosPublicoService {
  private base = `${environment.baseURL}legendarios/`;

  constructor(private http: HttpClient) {}

  cadastrar(dto: CadastroPublicoDTO): Observable<any> {
    return this.http.post(`${this.base}cadastro-publico`, dto);
  }

  getEstatisticas(): Observable<any> {
    return this.http.get(`${this.base}dashboard/estatisticas`);
  }

  getPorStatus(status: string): Observable<any> {
    return this.http.get(`${this.base}dashboard/por-status/${status}`);
  }

  atualizarStatus(idLegendario: number, status: string): Observable<any> {
    return this.http.patch(`${this.base}dashboard/status/${idLegendario}`, { status });
  }

  criarUsuario(login: string, senha: string, nivelPermissao: number): Observable<any> {
    return this.http.post(`${environment.baseURL}adm-usuarios`, { login, senha, nivel_permissao: nivelPermissao });
  }

  getUsuarios(): Observable<any> {
    return this.http.get(`${environment.baseURL}adm-usuarios`);
  }

  atualizarUsuario(dto: { id_usuario: number; login: string; nivel_permissao: number; nova_senha?: string }): Observable<any> {
    return this.http.put(`${environment.baseURL}adm-usuarios`, dto);
  }

  deletarUsuario(id: number): Observable<any> {
    return this.http.delete(`${environment.baseURL}adm-usuarios/${id}`);
  }

  // ---- Anúncios ----
  getAnuncios(): Observable<any> {
    return this.http.get(`${environment.baseURL}anuncios`);
  }

  getAnunciosAdm(): Observable<any> {
    return this.http.get(`${environment.baseURL}anuncios/adm`);
  }

  criarAnuncio(dto: any): Observable<any> {
    return this.http.post(`${environment.baseURL}anuncios`, dto);
  }

  atualizarAnuncio(dto: any): Observable<any> {
    return this.http.put(`${environment.baseURL}anuncios`, dto);
  }

  deletarAnuncio(id: number): Observable<any> {
    return this.http.delete(`${environment.baseURL}anuncios/${id}`);
  }
}
