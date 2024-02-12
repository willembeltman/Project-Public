import { Workorder } from "../workorder";
import { State } from "../state";

export interface WorkorderListResponse {
    workorders: Workorder[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}