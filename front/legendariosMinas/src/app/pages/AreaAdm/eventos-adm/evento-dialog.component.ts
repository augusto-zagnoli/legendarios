import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EventoRequest } from 'src/app/services/eventos.service';

export interface EventoDialogData {
  form: EventoRequest & { idEvento?: number };
}

@Component({
  selector: 'app-evento-dialog',
  template: `
    <h2 mat-dialog-title>{{ data.form.idEvento ? 'Editar' : 'Novo' }} Evento</h2>

    <mat-dialog-content class="dialog-content">
      <div class="alert alert-danger py-2" *ngIf="erro">{{ erro }}</div>

      <div class="row g-3">
        <div class="col-12">
          <mat-form-field appearance="outline" class="w-100">
            <mat-label>Título</mat-label>
            <input matInput [(ngModel)]="data.form.titulo" placeholder="Título do evento" required>
          </mat-form-field>
        </div>

        <div class="col-12">
          <mat-form-field appearance="outline" class="w-100">
            <mat-label>Descrição</mat-label>
            <textarea matInput [(ngModel)]="data.form.descricao" placeholder="Descrição..." rows="3"></textarea>
          </mat-form-field>
        </div>

        <div class="col-md-6">
          <mat-form-field appearance="outline" class="w-100">
            <mat-label>Data Início</mat-label>
            <input matInput type="datetime-local" [(ngModel)]="data.form.dataInicio" required>
          </mat-form-field>
        </div>

        <div class="col-md-6">
          <mat-form-field appearance="outline" class="w-100">
            <mat-label>Data Fim</mat-label>
            <input matInput type="datetime-local" [(ngModel)]="data.form.dataFim">
          </mat-form-field>
        </div>

        <div class="col-md-6">
          <mat-form-field appearance="outline" class="w-100">
            <mat-label>Local</mat-label>
            <input matInput [(ngModel)]="data.form.localEvento" placeholder="Local do evento">
          </mat-form-field>
        </div>

        <div class="col-md-3">
          <mat-form-field appearance="outline" class="w-100">
            <mat-label>Vagas</mat-label>
            <input matInput type="number" [(ngModel)]="data.form.maxVagas" min="1" required>
          </mat-form-field>
        </div>

        <div class="col-md-3">
          <mat-form-field appearance="outline" class="w-100">
            <mat-label>Status</mat-label>
            <mat-select [(ngModel)]="data.form.status">
              <mat-option value="rascunho">Rascunho</mat-option>
              <mat-option value="aberto">Aberto</mat-option>
              <mat-option value="encerrado">Encerrado</mat-option>
              <mat-option value="cancelado">Cancelado</mat-option>
            </mat-select>
          </mat-form-field>
        </div>
      </div>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close [disabled]="salvando">Cancelar</button>
      <button mat-raised-button color="primary" (click)="salvar()" [disabled]="salvando">
        <mat-spinner *ngIf="salvando" diameter="18" class="d-inline-block me-1"></mat-spinner>
        {{ salvando ? 'Salvando...' : 'Salvar' }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    .dialog-content {
      min-width: 500px;
      max-width: 700px;
    }
    @media (max-width: 768px) {
      .dialog-content { min-width: unset; }
    }
    mat-spinner { vertical-align: middle; }
  `]
})
export class EventoDialogComponent {
  erro = '';
  salvando = false;

  constructor(
    public dialogRef: MatDialogRef<EventoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EventoDialogData
  ) {}

  salvar(): void {
    this.erro = '';
    if (!this.data.form.titulo?.trim()) { this.erro = 'Título é obrigatório.'; return; }
    if (!this.data.form.dataInicio) { this.erro = 'Data de início é obrigatória.'; return; }
    if (this.data.form.maxVagas < 1) { this.erro = 'Vagas deve ser ao menos 1.'; return; }

    this.dialogRef.close(this.data.form);
  }
}
