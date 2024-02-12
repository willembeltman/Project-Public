import { Company } from "../company";

export interface CompanyCreateRequest {
    company: Company | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}