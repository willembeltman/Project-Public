export interface CompanyReadRequest {
    companyId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}