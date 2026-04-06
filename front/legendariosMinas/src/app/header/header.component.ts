import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  homeclick() {
    sessionStorage.setItem('ExibeRodape', '1');
  }

  get usuarioLogado(): string | null {
    return this.authService.getUsuario()?.nomeUsuario ?? null;
  }

  get estaLogado(): boolean {
    return this.authService.isAuthenticated();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login-adm']);
  }
}
