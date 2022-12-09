import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, delay, Subject, takeUntil } from 'rxjs';
import { CreditTypeListResponse } from '../shared/models/response/creditTypeListResponse.model';
import { CreditService } from '../shared/services/credit.service';

@Component({
  selector: 'app-credit-analysis-form',
  templateUrl: './credit-analysis-form.component.html',
  styleUrls: ['./credit-analysis-form.component.scss']
})
export class CreditAnalysisFormComponent implements OnInit, OnDestroy {
  loadingCreditType$: BehaviorSubject<boolean> = new BehaviorSubject(true);
  creditTypes$: BehaviorSubject<CreditTypeListResponse[]> = new BehaviorSubject([] as CreditTypeListResponse[]);

  loadingSubmit$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  unsubscribe$: Subject<void> = new Subject();

  formGroup = new FormGroup({
    creditType: new FormControl<number | null>(0, [Validators.required, Validators.min(1)]),
    amount: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    numberOfInstallments: new FormControl<number | null>(null, [Validators.required, Validators.min(5), Validators.max(72)]),
    firstDueDate: new FormControl('', [Validators.required])
  })

  minDate: Date;
  maxDate: Date;

  get showForm(): boolean {
    let creditType = this.formGroup.controls.creditType.value ?? 0
    return creditType > 0;
  }

  constructor(private creditService: CreditService, private router: Router, private snackBar: MatSnackBar) {
    this.creditService.getAllCreditTypes().pipe(takeUntil(this.unsubscribe$), delay(500)).subscribe({
      next: creditTypes => {
        this.creditTypes$.next(creditTypes);
        this.loadingCreditType$.next(false);
      },
      error: (err) => {
        this.loadingCreditType$.next(false);
        console.error('ERROR: ', err);
        this.snackBar.open("Ocorreu um erro ao carregar os tipos de crédito. Tente novamente mais tarde.", undefined, { duration: 5000 })
      }
    });

    this.minDate = new Date();
    this.maxDate = new Date();
  }

  ngOnInit(): void {
    this.updateMinAndMaxDate();
  }

  submit(): void {
    const { amount, creditType, firstDueDate, numberOfInstallments } = this.formGroup.getRawValue();

    if (amount && creditType && firstDueDate && numberOfInstallments) {
      this.loadingSubmit$.next(true);

      this.creditService.creditAnalysis({
        amount,
        creditType,
        firstDueDate,
        numberOfInstallments
      }).pipe(takeUntil(this.unsubscribe$)).subscribe({
        next: (response) => {
          this.loadingSubmit$.next(false);
          this.creditService.creditAnalysisResult = response;
          this.router.navigateByUrl('result')
        },
        error: (err) => {
          this.loadingSubmit$.next(false);
          console.error('ERROR: ', err);
          this.snackBar.open("Ocorreu um erro ao analisar o crédito.", undefined, { duration: 5000 })
        }
      });
    }
  }

  private updateMinAndMaxDate(): void {
    const dateNow = new Date();

    this.minDate.setDate(dateNow.getDate() + 15);
    this.maxDate.setDate(dateNow.getDate() + 40);
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
