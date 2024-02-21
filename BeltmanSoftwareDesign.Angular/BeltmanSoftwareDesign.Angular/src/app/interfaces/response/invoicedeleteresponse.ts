import { State } from "../state";

export interface InvoiceDeleteResponse {
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}