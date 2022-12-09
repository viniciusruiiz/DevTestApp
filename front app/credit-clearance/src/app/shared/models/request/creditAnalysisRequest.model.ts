import { CreditType } from "../../enums/CreditType.enum";

export interface CreditAnalysisRequest {
    amount: number;
    creditType: CreditType;
    numberOfInstallments: number;
    firstDueDate: string
} 