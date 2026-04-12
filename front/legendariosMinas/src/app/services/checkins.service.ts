import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface CheckinRequest {
  idEvento: number;
  idLegendario: number;
  observacoes?: string;
}

export interface CheckinResponse {
  idCheckin: number;
  idInscricao: number;
  idEvento: number;
  tituloEvento: string;
  idLegendario: number;
  nomeLegendario: string;
  dataCheckin: string;
  dataCheckout: string | null;
  observacoes: string;
}

@Injectable({ providedIn: 'root' })
export class CheckinsService {
  private base = `${environment.apiURL}checkins`;

  constructor(private http: HttpClient) {}

  porEvento(idEvento: number): Observable<any> {
    return this.http.get(`${this.base}/evento/${idEvento}`);
  }

  registrarCheckin(dto: CheckinRequest): Observable<any> {
    return this.http.post(this.base, dto);
  }

  registrarCheckout(id: number): Observable<any> {
    return this.http.patch(`${this.base}/${id}/checkout`, {});
  }
}
