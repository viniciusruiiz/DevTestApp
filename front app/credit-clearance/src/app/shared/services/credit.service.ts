import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { CreditAnalysisRequest } from '../models/request/creditAnalysisRequest.model';
import { CreditAnalysisResponse } from '../models/response/creditAnalysisResponse.model';
import { CreditTypeListResponse } from '../models/response/creditTypeListResponse.model';

@Injectable({
  providedIn: 'root'
})
export class CreditService {
  baseUrl: string;
  creditAnalysisResult?: CreditAnalysisResponse;

  constructor(private http: HttpClient) { 
    this.baseUrl = `${environment.apiBaseUrl}/main`
  }

  getAllCreditTypes() : Observable<CreditTypeListResponse[]> {
    return this.http.get<CreditTypeListResponse[]>(`${this.baseUrl}`);
  }

  creditAnalysis(creditInput: CreditAnalysisRequest) : Observable<CreditAnalysisResponse> {
    return this.http.post<CreditAnalysisResponse>(`${this.baseUrl}/credit-analysis`, creditInput);
  }
}
