import { User } from "../user";

export interface UserUpdateRequest {
    user: User | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}