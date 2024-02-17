import { Invoice } from "../invoice";

export interface InvoiceCreateRequest {
    invoice: Invoice | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}