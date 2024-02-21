import { State } from "../state";

export interface CustomerDeleteResponse {
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}