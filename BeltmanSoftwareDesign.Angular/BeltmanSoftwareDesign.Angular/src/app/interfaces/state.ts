import { User } from "./user";
import { Company } from "./company";

export interface State {
    bearerId: string | null;
    user: User | null;
    currentCompany: Company | null;
}