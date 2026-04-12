import { Component, OnInit } from '@angular/core';
import { VoluntariosService, VoluntarioResponse, VoluntarioRequest } from 'src/app/services/voluntarios.service';

@Component({
  selector: 'app-voluntarios-adm',
  templateUrl: './voluntarios-adm.component.html',
  styleUrls: ['./voluntarios-adm.component.css']
})
export class VoluntariosAdmComponent implements OnInit {
  voluntarios: VoluntarioResponse[] = [];
  carregando = false;
  busca = '';
  pagina = 1;
  tamanhoPagina = 20;
  totalRegistros = 0;
  totalPaginas = 0;

  // Modal
  modalVisivel = false;
  salvando = false;
  erro = '';
  form: VoluntarioRequest & { idVoluntario?: number } = this.novoForm();

  constructor(private service: VoluntariosService) {}

  ngOnInit(): void {
    this.carregar();
  }

  carregar(): void {
    this.carregando = true;
    this.service.listar(this.pagina, this.tamanhoPagina, this.busca).subscribe({
      next: (res: any) => {
        this.voluntarios = (res.data ?? res.Data ?? []).map((v: any) => ({
          idVoluntario: v.idVoluntario ?? v.IdVoluntario,
          idLegendario: v.idLegendario ?? v.IdLegendario,
          nomeLegendario: v.nomeLegendario ?? v.NomeLegendario,
          habilidades: v.habilidades ?? v.Habilidades,
          disponibilidade: v.disponibilidade ?? v.Disponibilidade,
          areaAtuacao: v.areaAtuacao ?? v.AreaAtuacao,
          dataCadastro: v.dataCadastro ?? v.DataCadastro
        }));
        this.totalRegistros = res.totalRegistros ?? res.TotalRegistros ?? 0;
        this.totalPaginas = res.totalPaginas ?? res.TotalPaginas ?? 0;
        this.carregando = false;
      },
      error: () => { this.carregando = false; }
    });
  }

  pesquisar(): void {
    this.pagina = 1;
    this.carregar();
  }

  irPagina(p: number): void {
    if (p < 1 || p > this.totalPaginas) return;
    this.pagina = p;
    this.carregar();
  }

  novoForm(): VoluntarioRequest & { idVoluntario?: number } {
    return { idLegendario: 0, habilidades: '', disponibilidade: '', areaAtuacao: '' };
  }

  abrirNovo(): void {
    this.form = this.novoForm();
    this.erro = '';
    this.modalVisivel = true;
  }

  editar(v: VoluntarioResponse): void {
    this.form = {
      idVoluntario: v.idVoluntario,
      idLegendario: v.idLegendario,
      habilidades: v.habilidades,
      disponibilidade: v.disponibilidade,
      areaAtuacao: v.areaAtuacao
    };
    this.erro = '';
    this.modalVisivel = true;
  }

  fecharModal(): void {
    this.modalVisivel = false;
  }

  salvar(): void {
    this.erro = '';
    if (!this.form.idLegendario) { this.erro = 'ID do Legendário é obrigatório.'; return; }

    this.salvando = true;
    const obs = this.form.idVoluntario
      ? this.service.atualizar(this.form.idVoluntario, this.form)
      : this.service.criar(this.form);

    obs.subscribe({
      next: () => {
        this.salvando = false;
        this.fecharModal();
        this.carregar();
      },
      error: (err: any) => {
        this.salvando = false;
        this.erro = err?.error?.mensagem || 'Erro ao salvar.';
      }
    });
  }

  confirmarDeletar(id: number): void {
    if (!confirm('Tem certeza que deseja remover este voluntário?')) return;
    this.service.deletar(id).subscribe({ next: () => this.carregar() });
  }
}
