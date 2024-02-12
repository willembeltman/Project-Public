import { User } from "../user";
import { State } from "../state";

export interface UserUpdateResponse {
    user: User | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}