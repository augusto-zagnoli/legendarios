import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface VoluntarioRequest {
  idLegendario: number;
  habilidades?: string;
  disponibilidade?: string;
  areaAtuacao?: string;
}

export interface VoluntarioResponse {
  idVoluntario: number;
  idLegendario: number;
  nomeLegendario: string;
  emailLegendario: string;
  celularLegendario: string;
  habilidades: string;
  disponibilidade: string;
  areaAtuacao: string;
  dataCadastro: string;
  ativo: boolean;
}

@Injectable({ providedIn: 'root' })
export class VoluntariosService {
  private base = `${environment.apiURL}voluntarios`;

  constructor(private http: HttpClient) {}

  listar(pagina = 1, tamanhoPagina = 20, busca = ''): Observable<any> {
    let params = new HttpParams()
      .set('pagina', pagina)
      .set('tamanhoPagina', tamanhoPagina);
    if (busca) params = params.set('busca', busca);
    return this.http.get(this.base, { params });
  }

  obter(id: number): Observable<any> {
    return this.http.get(`${this.base}/${id}`);
  }

  criar(dto: VoluntarioRequest): Observable<any> {
    return this.http.post(this.base, dto);
  }

  atualizar(id: number, dto: VoluntarioRequest): Observable<any> {
    return this.http.put(`${this.base}/${id}`, dto);
  }

  deletar(id: number): Observable<any> {
    return this.http.delete(`${this.base}/${id}`);
  }
}
