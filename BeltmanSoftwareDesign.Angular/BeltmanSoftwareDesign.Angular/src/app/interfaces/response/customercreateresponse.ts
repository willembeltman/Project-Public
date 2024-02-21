import { Customer } from "../customer";
import { State } from "../state";

export interface CustomerCreateResponse {
    customer: Customer | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}