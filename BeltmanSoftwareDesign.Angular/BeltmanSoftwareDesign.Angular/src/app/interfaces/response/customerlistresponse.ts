import { Customer } from "../customer";
import { State } from "../state";

export interface CustomerListResponse {
    customers: Customer[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}