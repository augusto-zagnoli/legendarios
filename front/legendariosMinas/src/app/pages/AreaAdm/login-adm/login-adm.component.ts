import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login-adm',
  templateUrl: './login-adm.component.html',
  styleUrls: ['./login-adm.component.css'],
})
export class LoginAdmComponent implements OnInit {
  form: FormGroup;
  erro: string = '';
  carregando: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      login: ['', Validators.required],
      password: ['', Validators.required],
      rememberUser: [false]
    });
  }

  ngOnInit(): void {
    sessionStorage.setItem('ExibeRodape', '0');
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/home-adm']);
    }
  }

  entrar(): void {
    if (this.form.invalid) return;
    this.carregando = true;
    this.erro = '';
    const { login, password, rememberUser } = this.form.value;
    this.authService.login(login, password, rememberUser).subscribe({
      next: () => {
        this.carregando = false;
        this.router.navigate(['/home-adm']);
      },
      error: (err) => {
        this.carregando = false;
        this.erro = err.status === 401
          ? 'Usuário ou senha inválidos.'
          : 'Erro ao realizar login. Tente novamente.';
      }
    });
  }
}
