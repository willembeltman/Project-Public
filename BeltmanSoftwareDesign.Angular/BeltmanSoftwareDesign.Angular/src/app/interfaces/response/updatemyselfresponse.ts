import { User } from "../user";
import { State } from "../state";

export interface UpdateMyselfResponse {
    user: User | null;
    errorOnlyUpdatesToYourselfAreAllowed: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}