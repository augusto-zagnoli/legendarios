import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface DashboardStats {
  totalLegendarios: number;
  legendariosPendentes: number;
  legendariosAprovados: number;
  legendariosReprovados: number;
  eventosAtivos: number;
  totalEventos: number;
  totalInscricoes: number;
  inscricoesConfirmadas: number;
  checkinsHoje: number;
  totalVoluntarios: number;
  voluntariosAtivos: number;
  totalAnuncios: number;
}

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private base = `${environment.apiURL}dashboard`;

  constructor(private http: HttpClient) {}

  getStats(): Observable<any> {
    return this.http.get(`${this.base}/stats`);
  }
}
