import { InvoiceWorkorder } from "./invoiceworkorder";
import { WorkorderAttachment } from "./workorderattachment";

export interface Workorder {
    id: number;
    start: Date;
    stop: Date;
    description: string | null;
    projectId: number | null;
    projectName: string | null;
    customerId: number | null;
    customerName: string | null;
    invoiceWorkorders: InvoiceWorkorder[];
    workorderAttachments: WorkorderAttachment[];
    amountUur: number;
}