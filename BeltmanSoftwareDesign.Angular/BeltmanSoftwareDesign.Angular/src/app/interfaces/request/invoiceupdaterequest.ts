import { Invoice } from "../invoice";

export interface InvoiceUpdateRequest {
    invoice: Invoice | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}