import { Invoice } from "../invoice";
import { State } from "../state";

export interface InvoiceListResponse {
    invoices: Invoice[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}