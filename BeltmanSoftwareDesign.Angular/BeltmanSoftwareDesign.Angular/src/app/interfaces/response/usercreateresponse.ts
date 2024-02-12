import { User } from "../user";
import { State } from "../state";

export interface UserCreateResponse {
    user: User | null;
    errorUserNameAlreadyUsed: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}