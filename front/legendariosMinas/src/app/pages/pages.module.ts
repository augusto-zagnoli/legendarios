import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { PoModule } from '@po-ui/ng-components';
import { RouterModule } from '@angular/router';
import { PoTemplatesModule } from '@po-ui/ng-templates';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatStepper, MatStepperModule } from "@angular/material/stepper";
import { ConsultarLegendarioComponent } from './AreaAdm/consultar-legendario/consultar-legendario.component';
import { HomeAdmComponent } from './AreaAdm/home-adm/home-adm.component';
import { LoginAdmComponent } from './AreaAdm/login-adm/login-adm.component';
import { CadastroSenderistasComponent } from './cadastro-senderistas/cadastro-senderistas.component';
import { TableHomeComponent } from './AreaAdm/home-adm/tablehome/table-home/table-home.component';
import { EditarLegendarioComponent } from './AreaAdm/home-adm/editar-legendario/editar-legendario.component';
import { AppRoutingModule } from '../app-routing.module';
import {PortalModule} from '@angular/cdk/portal';
import {ScrollingModule} from '@angular/cdk/scrolling';
import {OverlayModule} from '@angular/cdk/overlay';
import {DialogModule} from '@angular/cdk/dialog';
import { FirstSteepComponent } from "./cadastro-senderistas/firstSteep/first-page/first-steep.component";
import { SecondSteepComponent } from "./cadastro-senderistas/SecondSteep/second-page/second-steep.component";
import { ThirthSteepComponent } from "./cadastro-senderistas/ThirthSteep/ThirthPage/thirth-steep/thirth-steep.component";
import { CdkStepper } from "@angular/cdk/stepper";
import { PagamentoCadastroSenderistaComponent } from './cadastro-senderistas/pagamento-cadastro-senderista/pagamento-cadastro-senderista.component';
import { DescricaoEventoCadastroSenderistaComponent } from './cadastro-senderistas/descricao-evento-cadastro-senderista/descricao-evento-cadastro-senderista.component';
import { StatusPagamentoComponent } from './cadastro-senderistas/pagamento-cadastro-senderista/status-pagamento/status-pagamento.component';
import { PreCadastroComponent } from './pre-cadastro/pre-cadastro.component';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    declarations: [
        ConsultarLegendarioComponent,
        HomeAdmComponent,
        LoginAdmComponent,
        CadastroSenderistasComponent,
        TableHomeComponent,
        EditarLegendarioComponent,
        FirstSteepComponent,
        SecondSteepComponent,
        ThirthSteepComponent,
        PagamentoCadastroSenderistaComponent,
        DescricaoEventoCadastroSenderistaComponent,
        StatusPagamentoComponent,
        PreCadastroComponent
    ],
    providers: [ MatStepper,CdkStepper,],
    imports: [
        BrowserModule,
        CommonModule,
        AppRoutingModule,
        PoModule,
        RouterModule.forRoot([]),
        PoTemplatesModule,
        MatTableModule,
        MatPaginatorModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        NoopAnimationsModule
    ]
})
export class PagesModule { }
