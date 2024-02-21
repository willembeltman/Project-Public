import { Workorder } from "../workorder";
import { State } from "../state";

export interface WorkorderCreateResponse {
    workorder: Workorder | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}