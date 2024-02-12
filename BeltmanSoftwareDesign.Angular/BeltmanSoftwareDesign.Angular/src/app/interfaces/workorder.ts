import { InvoiceWorkorder } from "./invoiceworkorder";
import { WorkorderAttachment } from "./workorderattachment";

export interface Workorder {
    id: number;
    companyId: number;
    start: string;
    stop: string | null;
    description: string | null;
    projectId: number | null;
    projectName: string | null;
    customerId: number | null;
    customerName: string | null;
    invoiceWorkorders: InvoiceWorkorder[];
    workorderAttachments: WorkorderAttachment[];
    amountUur: number | null;
}