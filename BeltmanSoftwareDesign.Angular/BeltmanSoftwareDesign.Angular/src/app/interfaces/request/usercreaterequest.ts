import { User } from "../user";

export interface UserCreateRequest {
    user: User | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}