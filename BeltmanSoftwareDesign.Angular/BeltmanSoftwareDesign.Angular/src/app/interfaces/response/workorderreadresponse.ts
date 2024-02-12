import { Workorder } from "../workorder";
import { State } from "../state";

export interface WorkorderReadResponse {
    workorder: Workorder | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}