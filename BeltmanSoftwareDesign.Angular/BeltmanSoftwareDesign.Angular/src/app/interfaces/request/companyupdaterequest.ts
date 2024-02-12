import { Company } from "../company";

export interface CompanyUpdateRequest {
    company: Company | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}