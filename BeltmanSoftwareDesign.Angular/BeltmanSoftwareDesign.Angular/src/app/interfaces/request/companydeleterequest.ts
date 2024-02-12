export interface CompanyDeleteRequest {
    companyId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}