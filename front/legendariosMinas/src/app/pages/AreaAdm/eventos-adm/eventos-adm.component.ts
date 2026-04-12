import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { EventosService, EventoResponse, EventoRequest } from 'src/app/services/eventos.service';
import { InscricoesService, InscricaoResponse } from 'src/app/services/inscricoes.service';
import { CheckinsService, CheckinResponse } from 'src/app/services/checkins.service';
import { RelatoriosService } from 'src/app/services/relatorios.service';
import { AuthService } from 'src/app/services/auth.service';
import { EventoDialogComponent } from './evento-dialog.component';

@Component({
  selector: 'app-eventos-adm',
  templateUrl: './eventos-adm.component.html',
  styleUrls: ['./eventos-adm.component.css']
})
export class EventosAdmComponent implements OnInit {
  // Lista de eventos
  eventos: EventoResponse[] = [];
  eventosDataSource = new MatTableDataSource<EventoResponse>();
  eventosDisplayedColumns = ['titulo', 'dataInicio', 'localEvento', 'vagas', 'status', 'acoes'];
  inscritosDisplayedColumns = ['nomeLegendario', 'status', 'dataInscricao', 'observacoes', 'acoes'];
  checkinsDisplayedColumns = ['nomeLegendario', 'dataCheckin', 'dataCheckout', 'observacoes', 'acoes'];
  carregando = false;
  busca = '';
  pagina = 1;
  tamanhoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  @ViewChild(MatSort) sort!: MatSort;

  // Detalhe do evento
  eventoSelecionado: EventoResponse | null = null;
  abaDetalhe: 'inscritos' | 'checkins' = 'inscritos';
  inscritos: InscricaoResponse[] = [];
  checkins: CheckinResponse[] = [];
  carregandoDetalhe = false;

  // Checkin rápido
  checkinIdLegendario = 0;
  checkinObs = '';
  realizandoCheckin = false;
  msgCheckin = '';

  constructor(
    private eventosService: EventosService,
    private inscricoesService: InscricoesService,
    private checkinsService: CheckinsService,
    private relatoriosService: RelatoriosService,
    public authService: AuthService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.carregarEventos();
  }

  carregarEventos(): void {
    this.carregando = true;
    this.eventosService.listar(this.pagina, this.tamanhoPagina, this.busca).subscribe({
      next: (res: any) => {
        this.eventos = (res.data ?? res.Data ?? []).map((e: any) => ({
          idEvento: e.idEvento ?? e.IdEvento,
          titulo: e.titulo ?? e.Titulo,
          descricao: e.descricao ?? e.Descricao,
          dataInicio: e.dataInicio ?? e.DataInicio,
          dataFim: e.dataFim ?? e.DataFim,
          localEvento: e.localEvento ?? e.LocalEvento,
          maxVagas: e.maxVagas ?? e.MaxVagas,
          vagasOcupadas: e.vagasOcupadas ?? e.VagasOcupadas ?? 0,
          vagasDisponiveis: e.vagasDisponiveis ?? e.VagasDisponiveis ?? e.maxVagas ?? e.MaxVagas,
          status: e.status ?? e.Status,
          idLider: e.idLider ?? e.IdLider,
          nomeLider: e.nomeLider ?? e.NomeLider,
          imagemUrl: e.imagemUrl ?? e.ImagemUrl,
          requerAprovacao: e.requerAprovacao ?? e.RequerAprovacao,
          criadoEm: e.criadoEm ?? e.CriadoEm
        }));
        this.totalRegistros = res.totalRegistros ?? res.TotalRegistros ?? 0;
        this.totalPaginas = res.totalPaginas ?? res.TotalPaginas ?? 0;
        this.eventosDataSource.data = this.eventos;
        this.eventosDataSource.sort = this.sort;
        this.carregando = false;
      },
      error: () => { this.carregando = false; }
    });
  }

  pesquisar(): void {
    this.pagina = 1;
    this.carregarEventos();
  }

  onPaginaChange(event: PageEvent): void {
    this.pagina = event.pageIndex + 1;
    this.tamanhoPagina = event.pageSize;
    this.carregarEventos();
  }

  irPagina(p: number): void {
    if (p < 1 || p > this.totalPaginas) return;
    this.pagina = p;
    this.carregarEventos();
  }

  // ─── Dialog CRUD ──────────────────────────
  abrirNovoEvento(): void {
    this.abrirDialog({ titulo: '', descricao: '', dataInicio: '', dataFim: '', localEvento: '', maxVagas: 50, status: 'aberto' });
  }

  editarEvento(ev: EventoResponse): void {
    this.abrirDialog({
      idEvento: ev.idEvento,
      titulo: ev.titulo,
      descricao: ev.descricao,
      dataInicio: ev.dataInicio?.substring(0, 16),
      dataFim: ev.dataFim?.substring(0, 16),
      localEvento: ev.localEvento,
      maxVagas: ev.maxVagas,
      status: ev.status
    });
  }

  private abrirDialog(form: EventoRequest & { idEvento?: number }): void {
    const dialogRef = this.dialog.open(EventoDialogComponent, {
      width: '700px',
      data: { form: { ...form } },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: EventoRequest & { idEvento?: number } | undefined) => {
      if (!result) return;
      const obs = result.idEvento
        ? this.eventosService.atualizar(result.idEvento, result)
        : this.eventosService.criar(result);

      obs.subscribe({
        next: () => this.carregarEventos(),
        error: (err) => {
          const msg = err?.error?.mensagem || 'Erro ao salvar evento.';
          this.abrirDialog({ ...result });
        }
      });
    });
  }

  confirmarDeletar(id: number): void {
    if (!confirm('Tem certeza que deseja excluir este evento?')) return;
    this.eventosService.deletar(id).subscribe({
      next: () => this.carregarEventos()
    });
  }

  // ─── Detalhe do evento ───────────────────
  abrirDetalhe(ev: EventoResponse): void {
    this.eventoSelecionado = ev;
    this.abaDetalhe = 'inscritos';
    this.carregarInscritos(ev.idEvento);
  }

  fecharDetalhe(): void {
    this.eventoSelecionado = null;
    this.inscritos = [];
    this.checkins = [];
  }

  carregarInscritos(idEvento: number): void {
    this.carregandoDetalhe = true;
    this.inscricoesService.porEvento(idEvento).subscribe({
      next: (res: any) => {
        this.inscritos = (res.data ?? res.Data ?? []).map((i: any) => ({
          idInscricao: i.idInscricao ?? i.IdInscricao,
          idEvento: i.idEvento ?? i.IdEvento,
          tituloEvento: i.tituloEvento ?? i.TituloEvento,
          idLegendario: i.idLegendario ?? i.IdLegendario,
          nomeLegendario: i.nomeLegendario ?? i.NomeLegendario,
          status: i.status ?? i.Status,
          dataInscricao: i.dataInscricao ?? i.DataInscricao,
          dataConfirmacao: i.dataConfirmacao ?? i.DataConfirmacao
        }));
        this.carregandoDetalhe = false;
      },
      error: () => { this.carregandoDetalhe = false; }
    });
  }

  carregarCheckins(idEvento: number): void {
    this.carregandoDetalhe = true;
    this.checkinsService.porEvento(idEvento).subscribe({
      next: (res: any) => {
        this.checkins = (res.data ?? res.Data ?? []).map((c: any) => ({
          idCheckin: c.idCheckin ?? c.IdCheckin,
          idEvento: c.idEvento ?? c.IdEvento,
          idLegendario: c.idLegendario ?? c.IdLegendario,
          nomeLegendario: c.nomeLegendario ?? c.NomeLegendario,
          dataCheckin: c.dataCheckin ?? c.DataCheckin,
          dataCheckout: c.dataCheckout ?? c.DataCheckout,
          observacoes: c.observacoes ?? c.Observacoes
        }));
        this.carregandoDetalhe = false;
      },
      error: () => { this.carregandoDetalhe = false; }
    });
  }

  trocarAbaDetalhe(aba: 'inscritos' | 'checkins'): void {
    this.abaDetalhe = aba;
    if (!this.eventoSelecionado) return;
    if (aba === 'inscritos') this.carregarInscritos(this.eventoSelecionado.idEvento);
    else this.carregarCheckins(this.eventoSelecionado.idEvento);
  }

  cancelarInscricao(id: number): void {
    this.inscricoesService.cancelar(id).subscribe({
      next: () => {
        if (this.eventoSelecionado) this.carregarInscritos(this.eventoSelecionado.idEvento);
        this.carregarEventos();
      }
    });
  }

  confirmarInscricao(id: number): void {
    this.inscricoesService.confirmar(id).subscribe({
      next: () => {
        if (this.eventoSelecionado) this.carregarInscritos(this.eventoSelecionado.idEvento);
      }
    });
  }

  // ─── Check-in rápido ────────────────────
  realizarCheckin(): void {
    if (!this.eventoSelecionado || !this.checkinIdLegendario) return;
    this.realizandoCheckin = true;
    this.msgCheckin = '';
    this.checkinsService.registrarCheckin({
      idEvento: this.eventoSelecionado.idEvento,
      idLegendario: this.checkinIdLegendario,
      observacoes: this.checkinObs
    }).subscribe({
      next: () => {
        this.realizandoCheckin = false;
        this.msgCheckin = 'Check-in realizado!';
        this.checkinIdLegendario = 0;
        this.checkinObs = '';
        this.carregarCheckins(this.eventoSelecionado!.idEvento);
      },
      error: (err) => {
        this.realizandoCheckin = false;
        this.msgCheckin = err?.error?.mensagem || 'Erro ao realizar check-in.';
      }
    });
  }

  realizarCheckout(id: number): void {
    this.checkinsService.registrarCheckout(id).subscribe({
      next: () => {
        if (this.eventoSelecionado) this.carregarCheckins(this.eventoSelecionado.idEvento);
      }
    });
  }

  // ─── Exportação CSV ──────────────────────
  exportarInscritos(): void {
    if (!this.eventoSelecionado) return;
    this.relatoriosService.exportarInscritosEvento(this.eventoSelecionado.idEvento).subscribe({
      next: (blob) => this.relatoriosService.downloadBlob(blob, `inscritos_evento_${this.eventoSelecionado!.idEvento}.csv`)
    });
  }

  exportarPresenca(): void {
    if (!this.eventoSelecionado) return;
    this.relatoriosService.exportarPresencaEvento(this.eventoSelecionado.idEvento).subscribe({
      next: (blob) => this.relatoriosService.downloadBlob(blob, `presenca_evento_${this.eventoSelecionado!.idEvento}.csv`)
    });
  }

  getStatusBadgeClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'aberto': return 'bg-success';
      case 'rascunho': return 'bg-info';
      case 'encerrado': return 'bg-secondary';
      case 'cancelado': return 'bg-danger';
      default: return 'bg-info';
    }
  }
}
