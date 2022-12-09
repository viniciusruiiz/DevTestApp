import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreditAnalysisFormComponent } from './credit-analysis-form/credit-analysis-form.component';
import { CreditAnalysisResultComponent } from './credit-analysis-result/credit-analysis-result.component';

const routes: Routes = [
  {
    path: '',
    title: 'Análise de crédito - formulário',
    component: CreditAnalysisFormComponent,
    pathMatch: 'full'
  },
  {
    path: 'result',
    title: 'Análise de crédito - resultado',
    component: CreditAnalysisResultComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
