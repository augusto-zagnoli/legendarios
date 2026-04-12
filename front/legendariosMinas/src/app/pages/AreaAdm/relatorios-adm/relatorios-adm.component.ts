import { Component, OnInit } from '@angular/core';
import { RelatoriosService } from 'src/app/services/relatorios.service';

@Component({
  selector: 'app-relatorios-adm',
  templateUrl: './relatorios-adm.component.html',
  styleUrls: ['./relatorios-adm.component.css']
})
export class RelatoriosAdmComponent implements OnInit {
  // Auditoria
  auditorias: any[] = [];
  carregandoAudit = false;
  paginaAudit = 1;
  totalPaginasAudit = 0;
  totalAudit = 0;

  // Exportação
  exportando = false;
  msgExport = '';

  constructor(private service: RelatoriosService) {}

  ngOnInit(): void {
    this.carregarAuditoria();
  }

  carregarAuditoria(): void {
    this.carregandoAudit = true;
    this.service.getAuditoria(this.paginaAudit).subscribe({
      next: (res: any) => {
        this.auditorias = (res.data ?? res.Data ?? []).map((a: any) => ({
          id: a.id ?? a.Id,
          tabela: a.tabela ?? a.Tabela,
          id_registro: a.id_registro ?? a.Id_registro,
          acao: a.acao ?? a.Acao,
          dados_anteriores: a.dados_anteriores ?? a.Dados_anteriores,
          dados_novos: a.dados_novos ?? a.Dados_novos,
          nome_usuario: a.nome_usuario ?? a.Nome_usuario,
          data_acao: a.data_acao ?? a.Data_acao
        }));
        this.totalAudit = res.totalRegistros ?? res.TotalRegistros ?? 0;
        this.totalPaginasAudit = res.totalPaginas ?? res.TotalPaginas ?? 0;
        this.carregandoAudit = false;
      },
      error: () => { this.carregandoAudit = false; }
    });
  }

  irPaginaAudit(p: number): void {
    if (p < 1 || p > this.totalPaginasAudit) return;
    this.paginaAudit = p;
    this.carregarAuditoria();
  }

  exportarLegendarios(): void {
    this.exportando = true;
    this.msgExport = '';
    this.service.exportarLegendarios().subscribe({
      next: (blob) => {
        this.service.downloadBlob(blob, 'legendarios.csv');
        this.exportando = false;
        this.msgExport = 'Exportação concluída!';
      },
      error: () => {
        this.exportando = false;
        this.msgExport = 'Erro ao exportar.';
      }
    });
  }
}
