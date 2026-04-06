import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LegendariosPublicoService } from 'src/app/services/legendarios-publico.service';

@Component({
  selector: 'app-pre-cadastro',
  templateUrl: './pre-cadastro.component.html',
  styleUrls: ['./pre-cadastro.component.scss']
})
export class PreCadastroComponent implements OnInit {
  form: FormGroup;
  enviando = false;
  sucesso = false;
  erro = '';

  estadosCivis = ['Solteiro(a)', 'Casado(a)', 'Divorciado(a)', 'Viúvo(a)', 'União Estável'];
  tiposSanguineos = ['A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-'];
  tamanhosCamiseta = ['PP', 'P', 'M', 'G', 'GG', 'XGG'];
  categoriasCnh = ['A', 'B', 'AB', 'C', 'D', 'E', 'ACC'];

  constructor(private fb: FormBuilder, private service: LegendariosPublicoService) {
    this.form = this.fb.group({
      // Dados pessoais
      nome: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      celular: ['', Validators.required],
      cadastro_pessoa: ['', Validators.required],
      data_de_nascimento: ['', Validators.required],
      estado_civil: ['', Validators.required],
      tipo_sanguineo: ['', Validators.required],
      tamanho_camiseta: ['', Validators.required],
      // Profissão e CNH
      profissao: [''],
      cnh: [''],
      categoria_cnh: [''],
      // Endereço
      cep: [''],
      endereco: [''],
      cidade: [''],
      estado: [''],
      pais: ['Brasil'],
      // Contato de emergência
      emergencia_nome: ['', Validators.required],
      emergencia_telefone: ['', Validators.required],
      // Observações
      observacoes: ['']
    });
  }

  ngOnInit(): void {}

  buscarCep(): void {
    const cep = this.form.get('cep')?.value?.replace(/\D/g, '');
    if (cep?.length === 8) {
      fetch(`https://viacep.com.br/ws/${cep}/json/`)
        .then(r => r.json())
        .then(data => {
          if (!data.erro) {
            this.form.patchValue({
              endereco: data.logradouro,
              cidade: data.localidade,
              estado: data.uf
            });
          }
        });
    }
  }

  enviar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.enviando = true;
    this.erro = '';
    this.service.cadastrar(this.form.value).subscribe({
      next: (res) => {
        this.enviando = false;
        if (res.sucesso) {
          this.sucesso = true;
          this.form.reset();
        } else {
          this.erro = res.erro || 'Erro ao enviar cadastro.';
        }
      },
      error: () => {
        this.enviando = false;
        this.erro = 'Erro ao conectar com o servidor.';
      }
    });
  }

  campo(name: string) {
    return this.form.get(name);
  }

  invalido(name: string): boolean {
    const c = this.campo(name);
    return !!(c?.invalid && c?.touched);
  }
}
