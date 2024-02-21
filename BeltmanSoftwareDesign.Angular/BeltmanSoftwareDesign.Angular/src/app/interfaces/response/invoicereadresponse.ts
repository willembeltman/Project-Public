import { Invoice } from "../invoice";
import { State } from "../state";

export interface InvoiceReadResponse {
    invoice: Invoice | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}