export interface InvoiceDeleteRequest {
    invoiceId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}