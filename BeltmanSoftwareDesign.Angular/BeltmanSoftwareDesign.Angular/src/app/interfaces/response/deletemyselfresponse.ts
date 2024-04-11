import { State } from "../state";

export interface DeleteMyselfResponse {
    errorOnlyDeletesToYourselfAreAllowed: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}