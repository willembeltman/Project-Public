import { User } from "../user";
import { State } from "../state";

export interface UserListResponse {
    users: User[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}