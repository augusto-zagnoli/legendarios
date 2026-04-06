import { Component, OnInit } from '@angular/core';
import { LegendariosPublicoService } from 'src/app/services/legendarios-publico.service';

@Component({
  selector: 'app-acoes-sociais',
  templateUrl: './acoes-sociais.component.html',
  styleUrls: ['./acoes-sociais.component.css']
})
export class AcoesSociaisComponent implements OnInit {
  anuncios: any[] = [];
  carregando = true;
  erro = '';

  constructor(private service: LegendariosPublicoService) {}

  ngOnInit(): void {
    this.service.getAnuncios().subscribe({
      next: (res) => {
        if (res.sucesso) {
          this.anuncios = res.data ?? [];
        } else {
          this.erro = 'Não foi possível carregar os anúncios.';
        }
        this.carregando = false;
      },
      error: () => {
        this.erro = 'Erro ao conectar com o servidor.';
        this.carregando = false;
      }
    });
  }
}
