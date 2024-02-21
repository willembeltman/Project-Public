import { InvoiceWorkorder } from "./invoiceworkorder";
import { WorkorderAttachment } from "./workorderattachment";

export interface Workorder {
    id: number;
    name: string | null;
    description: string | null;
    start: Date;
    stop: Date;
    projectId: number | null;
    projectName: string | null;
    customerId: number | null;
    customerName: string | null;
    invoiceWorkorders: InvoiceWorkorder[];
    workorderAttachments: WorkorderAttachment[];
    amountUur: number;
}