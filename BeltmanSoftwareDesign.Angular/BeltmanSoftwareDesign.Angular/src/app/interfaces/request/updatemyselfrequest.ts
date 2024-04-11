import { User } from "../user";

export interface UpdateMyselfRequest {
    user: User | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}