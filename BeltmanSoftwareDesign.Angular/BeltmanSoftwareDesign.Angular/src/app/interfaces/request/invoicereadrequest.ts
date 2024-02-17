export interface InvoiceReadRequest {
    invoiceId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}