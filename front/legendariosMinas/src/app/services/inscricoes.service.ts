import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface InscricaoRequest {
  idEvento: number;
  idLegendario: number;
  observacoes?: string;
}

export interface InscricaoResponse {
  idInscricao: number;
  idEvento: number;
  idLegendario: number;
  nomeLegendario: string;
  tituloEvento: string;
  status: string;
  dataInscricao: string;
  dataConfirmacao: string;
  observacoes: string;
}

@Injectable({ providedIn: 'root' })
export class InscricoesService {
  private base = `${environment.apiURL}inscricoes`;

  constructor(private http: HttpClient) {}

  porEvento(idEvento: number): Observable<any> {
    return this.http.get(`${this.base}/evento/${idEvento}`);
  }

  porLegendario(idLegendario: number): Observable<any> {
    return this.http.get(`${this.base}/legendario/${idLegendario}`);
  }

  inscrever(dto: InscricaoRequest): Observable<any> {
    return this.http.post(this.base, dto);
  }

  cancelar(id: number): Observable<any> {
    return this.http.delete(`${this.base}/${id}`);
  }

  confirmar(id: number): Observable<any> {
    return this.http.patch(`${this.base}/${id}/status`, { status: 'confirmado' });
  }
}
