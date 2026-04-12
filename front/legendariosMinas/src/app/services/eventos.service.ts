import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface EventoRequest {
  titulo: string;
  descricao?: string;
  dataInicio: string;
  dataFim?: string;
  localEvento?: string;
  maxVagas: number;
  status?: string;
  idLider?: number;
  imagemUrl?: string;
  requerAprovacao?: boolean;
}

export interface EventoResponse {
  idEvento: number;
  titulo: string;
  descricao: string;
  dataInicio: string;
  dataFim: string;
  localEvento: string;
  maxVagas: number;
  vagasOcupadas: number;
  vagasDisponiveis: number;
  status: string;
  idLider: number;
  nomeLider: string;
  imagemUrl: string;
  requerAprovacao: boolean;
  criadoEm: string;
}

export interface PaginatedResponse<T> {
  sucesso: boolean;
  data: T[];
  pagina: number;
  tamanhoPagina: number;
  totalRegistros: number;
  totalPaginas: number;
}

@Injectable({ providedIn: 'root' })
export class EventosService {
  private base = `${environment.apiURL}eventos`;

  constructor(private http: HttpClient) {}

  listar(pagina = 1, tamanhoPagina = 20, busca = ''): Observable<PaginatedResponse<EventoResponse>> {
    let params = new HttpParams()
      .set('pagina', pagina)
      .set('tamanhoPagina', tamanhoPagina);
    if (busca) params = params.set('busca', busca);
    return this.http.get<PaginatedResponse<EventoResponse>>(`${this.base}/paginado`, { params });
  }

  obter(id: number): Observable<any> {
    return this.http.get(`${this.base}/${id}`);
  }

  criar(dto: EventoRequest): Observable<any> {
    return this.http.post(this.base, dto);
  }

  atualizar(id: number, dto: EventoRequest): Observable<any> {
    return this.http.put(this.base, { ...dto, IdEvento: id });
  }

  deletar(id: number): Observable<any> {
    return this.http.delete(`${this.base}/${id}`);
  }
}
