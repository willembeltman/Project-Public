import { State } from "../state";

export interface UserDeleteResponse {
    errorOnlyDeletesToYourselfAreAllowed: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}