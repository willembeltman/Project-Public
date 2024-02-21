import { State } from "../state";

export interface ProjectDeleteResponse {
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}