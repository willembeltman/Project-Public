import { InvoiceWorkorder } from "./invoiceworkorder";

export interface Invoice {
    id: number;
    invoiceTypeId: number | null;
    invoiceTypeName: string | null;
    projectId: number | null;
    projectName: string | null;
    customerId: number | null;
    customerName: string | null;
    taxRateId: number | null;
    taxRateName: string | null;
    taxRatePercentage: number | null;
    date: Date;
    invoiceNumber: string | null;
    description: string | null;
    isPayedInCash: boolean;
    isPayed: boolean;
    datePayed: Date | null;
    paymentCode: string | null;
    invoiceWorkorders: InvoiceWorkorder[];
    quarter: number;
}