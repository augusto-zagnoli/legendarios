import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditarLegendarioComponent } from './pages/AreaAdm/home-adm/editar-legendario/editar-legendario.component';
import { HomeAdmComponent } from './pages/AreaAdm/home-adm/home-adm.component';
import { LoginAdmComponent } from './pages/AreaAdm/login-adm/login-adm.component';
import { CadastroSenderistasComponent } from './pages/cadastro-senderistas/cadastro-senderistas.component';
import { DescricaoEventoCadastroSenderistaComponent } from './pages/cadastro-senderistas/descricao-evento-cadastro-senderista/descricao-evento-cadastro-senderista.component';
import { PagamentoCadastroSenderistaComponent } from './pages/cadastro-senderistas/pagamento-cadastro-senderista/pagamento-cadastro-senderista.component';
import { StatusPagamentoComponent } from './pages/cadastro-senderistas/pagamento-cadastro-senderista/status-pagamento/status-pagamento.component';
import { HomeComponent } from './pages/home/home.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'login-adm',
    component: LoginAdmComponent
  },
  {
    path: 'cadastrar-senderista',
    component: CadastroSenderistasComponent
  },
  {
    path: 'home-adm',
    component: HomeAdmComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'editar-legendario',
    component: EditarLegendarioComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'status-pagamento',
    component: StatusPagamentoComponent
  },
  {
    path: 'descricao-evento-senderista',
    component: DescricaoEventoCadastroSenderistaComponent
  }
  ,
  {
    path: 'pagamento-senderista',
    component: PagamentoCadastroSenderistaComponent
  }


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
