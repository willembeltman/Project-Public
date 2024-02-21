import { User } from "../user";
import { State } from "../state";

export interface UserUpdateResponse {
    user: User | null;
    errorOnlyUpdatesToYourselfAreAllowed: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}