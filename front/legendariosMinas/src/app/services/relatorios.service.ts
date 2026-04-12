import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class RelatoriosService {
  private base = `${environment.apiURL}relatorios`;

  constructor(private http: HttpClient) {}

  exportarLegendarios(): Observable<Blob> {
    return this.http.get(`${this.base}/legendarios/csv`, {
      responseType: 'blob'
    });
  }

  exportarInscritosEvento(idEvento: number): Observable<Blob> {
    return this.http.get(`${this.base}/eventos/${idEvento}/inscritos/csv`, {
      responseType: 'blob'
    });
  }

  exportarPresencaEvento(idEvento: number): Observable<Blob> {
    return this.http.get(`${this.base}/eventos/${idEvento}/presenca/csv`, {
      responseType: 'blob'
    });
  }

  getAuditoria(pagina = 1, tamanhoPagina = 50): Observable<any> {
    const params = new HttpParams()
      .set('pagina', pagina)
      .set('tamanhoPagina', tamanhoPagina);
    return this.http.get(`${this.base}/auditoria`, { params });
  }

  downloadBlob(blob: Blob, fileName: string): void {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
  }
}
