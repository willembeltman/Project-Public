import { Customer } from "../customer";

export interface CustomerCreateRequest {
    customer: Customer | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}