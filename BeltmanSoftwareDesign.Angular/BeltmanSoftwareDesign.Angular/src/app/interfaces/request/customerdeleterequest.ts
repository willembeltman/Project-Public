export interface CustomerDeleteRequest {
    customerId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}