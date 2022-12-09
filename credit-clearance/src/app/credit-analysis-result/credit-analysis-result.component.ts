import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CreditAnalysisResponse } from '../shared/models/response/creditAnalysisResponse.model';
import { CreditService } from '../shared/services/credit.service';

@Component({
  selector: 'app-credit-analysis-result',
  templateUrl: './credit-analysis-result.component.html',
  styleUrls: ['./credit-analysis-result.component.scss']
})
export class CreditAnalysisResultComponent {
  data!: CreditAnalysisResponse;

  constructor(private creditService: CreditService, private router: Router) {
    if(!this.creditService.creditAnalysisResult) {
      this.returnToAnalysisForm();
      return;
    }
    this.data = this.creditService.creditAnalysisResult;
  }

  returnToAnalysisForm(): void {
    this.router.navigateByUrl('');
  }
}
