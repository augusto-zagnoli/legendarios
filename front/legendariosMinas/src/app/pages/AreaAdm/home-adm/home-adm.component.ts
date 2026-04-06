import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import { legendarios } from './Model/legendariosModel';
import { HomeAdmService } from './service/home-adm.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-home-adm',
  templateUrl: './home-adm.component.html',
  styleUrls: ['./home-adm.component.scss']
})
export class HomeAdmComponent implements OnInit {
  pagina: number = 10;
  paginaatual: number | undefined;
  ultimapagina: number | undefined;
  totaldeorcamentospararetornar: number | undefined = 10;
  listaDadosLegendarios: legendarios[] = [];



  categories: string[] = ['teste3', 'teste2'];

  series = [
    { label: 'Legendários', data: 100 },
    { label: 'Senderistas', data: 10 },
  ];


  constructor(private serviceHomeAdm: HomeAdmService,
    private router: Router,
    private authService: AuthService) { }

  ngOnInit(): void {
    // A rota já é protegida pelo AuthGuard;
    // esta verificação adicional garante consistência
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login-adm']);
    }
  }

  onClickGridItenDadosTecnicos(event: any, element: any, testo: string) {

  }
}
