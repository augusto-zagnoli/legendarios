import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PoChartType } from '@po-ui/ng-components';
import { AuthService } from 'src/app/services/auth.service';
import { LegendariosPublicoService } from 'src/app/services/legendarios-publico.service';

@Component({
  selector: 'app-home-adm',
  templateUrl: './home-adm.component.html',
  styleUrls: ['./home-adm.component.scss']
})
export class HomeAdmComponent implements OnInit {

  // Estatísticas
  totalCadastros = 0;
  pendentes = 0;
  aprovados = 0;
  reprovados = 0;

  // Gráfico
  readonly tipoGrafico = PoChartType.Donut;
  graficoCategorias: string[] = ['Pendentes', 'Aprovados', 'Reprovados'];
  graficoSeries: { label: string; data: number; color?: string }[] = [
    { label: 'Pendentes',  data: 0, color: '#f0ad4e' },
    { label: 'Aprovados',  data: 0, color: '#5cb85c' },
    { label: 'Reprovados', data: 0, color: '#d9534f' },
  ];

  // Navegação sidebar
  secaoAtiva: 'dashboard' | 'cadastros' | 'anuncios' | 'usuarios' = 'dashboard';
  sidebarCollapsed = false;

  // Tabela
  abaAtiva: 'pendente' | 'aprovado' | 'reprovado' = 'pendente';
  listaExibida: any[] = [];
  carregandoTabela = false;
  mensagemTabela = '';

  // Filtros
  filtrosVisiveis = false;
  filtros = {
    nome: '', email: '', celular: '', cadastro_pessoa: '',
    profissao: '', tipo_sanguineo: '', estado_civil: '',
    cidade: '', estado: '', cep: '', pais: '',
    cnh: '', categoria_cnh: '',
    religiao: '', igreja: '', rede: '',
    tamanho_camiseta: '', emergencia_nome: '',
    e_batizado: '', frequenta_celula: '', e_lider_de_celula: ''
  };

  readonly ufs = [
    'AC','AL','AP','AM','BA','CE','DF','ES','GO','MA',
    'MT','MS','MG','PA','PB','PR','PE','PI','RJ','RN',
    'RS','RO','RR','SC','SP','SE','TO'
  ];

  get listaFiltrada(): any[] {
    return this.listaExibida.filter(item =>
      this.txt(item.nome,             this.filtros.nome) &&
      this.txt(item.email,            this.filtros.email) &&
      this.txt(item.celular,          this.filtros.celular) &&
      this.txt(item.cadastro_pessoa,  this.filtros.cadastro_pessoa) &&
      this.txt(item.profissao,        this.filtros.profissao) &&
      this.txt(item.tipo_sanguineo,   this.filtros.tipo_sanguineo) &&
      this.txt(item.estado_civil,     this.filtros.estado_civil) &&
      this.txt(item.cidade,           this.filtros.cidade) &&
      this.txt(item.estado,           this.filtros.estado) &&
      this.txt(item.cep,              this.filtros.cep) &&
      this.txt(item.pais,             this.filtros.pais) &&
      this.txt(item.cnh,              this.filtros.cnh) &&
      this.txt(item.categoria_cnh,    this.filtros.categoria_cnh) &&
      this.txt(item.religiao,         this.filtros.religiao) &&
      this.txt(item.igreja,           this.filtros.igreja) &&
      this.txt(item.rede,             this.filtros.rede) &&
      this.txt(item.tamanho_camiseta, this.filtros.tamanho_camiseta) &&
      this.txt(item.emergencia_nome,  this.filtros.emergencia_nome) &&
      this.bool(item.e_batizado,        this.filtros.e_batizado) &&
      this.bool(item.frequenta_celula,  this.filtros.frequenta_celula) &&
      this.bool(item.e_lider_de_celula, this.filtros.e_lider_de_celula)
    );
  }

  get totalFiltrosAtivos(): number {
    return Object.values(this.filtros).filter(v => v !== '').length;
  }

  limparFiltros(): void {
    Object.keys(this.filtros).forEach(k => (this.filtros as any)[k] = '');
  }

  private txt(valor: any, filtro: string): boolean {
    if (!filtro) return true;
    return String(valor ?? '').toLowerCase().includes(filtro.toLowerCase());
  }

  private bool(valor: any, filtro: string): boolean {
    if (!filtro) return true;
    return String(valor) === filtro;
  }

  constructor(
    private authService: AuthService,
    private router: Router,
    private service: LegendariosPublicoService
  ) {}

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login-adm']);
      return;
    }
    this.carregarEstatisticas();
    this.carregarAba('pendente');
  }

  carregarEstatisticas(): void {
    this.service.getEstatisticas().subscribe({
      next: (res) => {
        if (res.sucesso) {
          const partes = res.erro.split('|').map(Number);
          this.totalCadastros = partes[0];
          this.pendentes    = partes[1];
          this.aprovados    = partes[2];
          this.reprovados   = partes[3];
          // atualiza gráfico
          this.graficoSeries = [
            { label: 'Pendentes',  data: this.pendentes,  color: '#f0ad4e' },
            { label: 'Aprovados',  data: this.aprovados,  color: '#5cb85c' },
            { label: 'Reprovados', data: this.reprovados, color: '#d9534f' },
          ];
        }
      }
    });
  }

  carregarAba(status: 'pendente' | 'aprovado' | 'reprovado'): void {
    this.abaAtiva = status;
    this.limparFiltros();
    this.carregandoTabela = true;
    this.listaExibida = [];
    this.mensagemTabela = '';

    this.service.getPorStatus(status).subscribe({
      next: (res) => {
        this.carregandoTabela = false;
        if (res.sucesso) {
          this.listaExibida = res.data || [];
          if (this.listaExibida.length === 0) {
            this.mensagemTabela = 'Nenhum registro encontrado.';
          }
        }
      },
      error: () => {
        this.carregandoTabela = false;
        this.mensagemTabela = 'Erro ao carregar dados.';
      }
    });
  }

  irParaUsuarios(): void {
    this.secaoAtiva = 'usuarios';
    this.carregarUsuarios();
  }

  irParaCadastros(): void {
    this.secaoAtiva = 'cadastros';
    if (this.abaAtiva !== 'pendente' && this.abaAtiva !== 'aprovado' && this.abaAtiva !== 'reprovado') {
      this.carregarAba('pendente');
    }
  }

  irParaAnuncios(): void {
    this.secaoAtiva = 'anuncios';
    this.carregarAbaAnuncios();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login-adm']);
  }

  aprovar(id: number): void {
    this.service.atualizarStatus(id, 'aprovado').subscribe({
      next: () => { this.carregarEstatisticas(); this.carregarAba(this.abaAtiva); }
    });
  }

  reprovar(id: number): void {
    this.service.atualizarStatus(id, 'reprovado').subscribe({
      next: () => { this.carregarEstatisticas(); this.carregarAba(this.abaAtiva); }
    });
  }

  reabrir(id: number): void {
    this.service.atualizarStatus(id, 'pendente').subscribe({
      next: () => { this.carregarEstatisticas(); this.carregarAba(this.abaAtiva); }
    });
  }

  get nivelAdmin(): boolean {
    return (this.authService.getUsuario()?.nivelPermissao ?? 0) === 1;
  }

  // ─── Usuários ─────────────────────────────────────────────────────
  listaUsuarios: any[] = [];
  carregandoUsuarios = false;
  modalUsuarioVisivel = false;
  usuarioForm: any = { id_usuario: null, login: '', senha: '', confirmaSenha: '', nivel_permissao: 0 };
  salvandoUsuario = false;
  erroUsuario = '';
  sucessoUsuario = '';

  carregarUsuarios(): void {
    this.carregandoUsuarios = true;
    this.service.getUsuarios().subscribe({
      next: (res) => {
        this.listaUsuarios = res.sucesso ? (res.data ?? []) : [];
        this.carregandoUsuarios = false;
      },
      error: () => { this.carregandoUsuarios = false; }
    });
  }

  abrirModalUsuario(): void {
    this.usuarioForm = { id_usuario: null, login: '', senha: '', confirmaSenha: '', nivel_permissao: 0 };
    this.erroUsuario = '';
    this.sucessoUsuario = '';
    this.modalUsuarioVisivel = true;
  }

  editarUsuario(u: any): void {
    this.usuarioForm = { id_usuario: u.id_usuario, login: u.n_lgnd, senha: '', confirmaSenha: '', nivel_permissao: u.nivel_permissao };
    this.erroUsuario = '';
    this.sucessoUsuario = '';
    this.modalUsuarioVisivel = true;
  }

  fecharModalUsuario(): void {
    this.modalUsuarioVisivel = false;
  }

  salvarNovoUsuario(): void {
    this.erroUsuario = '';
    this.sucessoUsuario = '';

    if (!this.usuarioForm.login.trim()) {
      this.erroUsuario = 'Login é obrigatório.'; return;
    }

    // Criação — senha obrigatória
    if (!this.usuarioForm.id_usuario && this.usuarioForm.senha.length < 6) {
      this.erroUsuario = 'Senha deve ter pelo menos 6 caracteres.'; return;
    }
    // Edição — senha só valida se preenchida
    if (this.usuarioForm.id_usuario && this.usuarioForm.senha && this.usuarioForm.senha.length < 6) {
      this.erroUsuario = 'Nova senha deve ter pelo menos 6 caracteres.'; return;
    }
    if (this.usuarioForm.senha !== this.usuarioForm.confirmaSenha) {
      this.erroUsuario = 'As senhas não coincidem.'; return;
    }

    this.salvandoUsuario = true;

    if (this.usuarioForm.id_usuario) {
      // Editar
      this.service.atualizarUsuario({
        id_usuario: this.usuarioForm.id_usuario,
        login: this.usuarioForm.login,
        nivel_permissao: this.usuarioForm.nivel_permissao,
        nova_senha: this.usuarioForm.senha || undefined
      }).subscribe({
        next: () => {
          this.salvandoUsuario = false;
          this.sucessoUsuario = 'Usuário atualizado com sucesso!';
          this.carregarUsuarios();
          setTimeout(() => this.fecharModalUsuario(), 1200);
        },
        error: (err: any) => {
          this.salvandoUsuario = false;
          this.erroUsuario = err?.error?.mensagem || 'Erro ao atualizar usuário.';
        }
      });
    } else {
      // Criar
      this.service.criarUsuario(
        this.usuarioForm.login,
        this.usuarioForm.senha,
        this.usuarioForm.nivel_permissao
      ).subscribe({
        next: () => {
          this.salvandoUsuario = false;
          this.sucessoUsuario = 'Usuário criado com sucesso!';
          this.carregarUsuarios();
          setTimeout(() => this.fecharModalUsuario(), 1200);
        },
        error: (err: any) => {
          this.salvandoUsuario = false;
          this.erroUsuario = err?.error?.mensagem || 'Erro ao criar usuário.';
        }
      });
    }
  }

  confirmarDeletarUsuario(id: number): void {
    if (!confirm('Tem certeza que deseja remover este usuário?')) return;
    this.service.deletarUsuario(id).subscribe({
      next: () => this.carregarUsuarios()
    });
  }

  // ─── Anúncios ───────────────────────────────────────────────────
  listaAnuncios: any[] = [];
  carregandoAnuncios = false;
  modalAnuncioVisivel = false;
  salvandoAnuncio = false;
  erroAnuncio = '';
  anuncioForm: any = { titulo: '', imagem_url: '', texto: '', link: '', ativo: true, ordem: 0 };

  carregarAbaAnuncios(): void {
    this.carregandoAnuncios = true;
    this.service.getAnunciosAdm().subscribe({
      next: (res) => {
        this.listaAnuncios = res.sucesso ? (res.data ?? []) : [];
        this.carregandoAnuncios = false;
      },
      error: () => { this.carregandoAnuncios = false; }
    });
  }

  novoAnuncio(): void {
    this.anuncioForm = { titulo: '', imagem_url: '', texto: '', link: '', ativo: true, ordem: 0 };
    this.erroAnuncio = '';
    this.modalAnuncioVisivel = true;
  }

  editarAnuncio(a: any): void {
    this.anuncioForm = { ...a };
    this.erroAnuncio = '';
    this.modalAnuncioVisivel = true;
  }

  fecharModalAnuncio(): void {
    this.modalAnuncioVisivel = false;
  }

  salvarAnuncio(): void {
    this.erroAnuncio = '';
    if (!this.anuncioForm.titulo?.trim()) { this.erroAnuncio = 'Título é obrigatório.'; return; }
    if (!this.anuncioForm.imagem_url?.trim()) { this.erroAnuncio = 'URL da imagem é obrigatória.'; return; }
    if (!this.anuncioForm.texto?.trim()) { this.erroAnuncio = 'Texto é obrigatório.'; return; }

    this.salvandoAnuncio = true;
    const obs = this.anuncioForm.id_anuncio
      ? this.service.atualizarAnuncio(this.anuncioForm)
      : this.service.criarAnuncio(this.anuncioForm);

    obs.subscribe({
      next: (res) => {
        this.salvandoAnuncio = false;
        if (res.sucesso) {
          this.fecharModalAnuncio();
          this.carregarAbaAnuncios();
        } else {
          this.erroAnuncio = res.erro || 'Erro ao salvar.';
        }
      },
      error: () => {
        this.salvandoAnuncio = false;
        this.erroAnuncio = 'Erro ao conectar com o servidor.';
      }
    });
  }

  confirmarDeletarAnuncio(id: number): void {
    if (!confirm('Tem certeza que deseja excluir este anúncio?')) return;
    this.service.deletarAnuncio(id).subscribe({
      next: () => this.carregarAbaAnuncios()
    });
  }
}
