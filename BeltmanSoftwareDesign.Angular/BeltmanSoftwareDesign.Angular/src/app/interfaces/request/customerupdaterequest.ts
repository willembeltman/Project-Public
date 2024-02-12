import { Customer } from "../customer";

export interface CustomerUpdateRequest {
    customer: Customer | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}