import { User } from "../user";
import { State } from "../state";

export interface UserReadResponse {
    user: User | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}