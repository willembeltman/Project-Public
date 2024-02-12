export interface CustomerReadRequest {
    customerId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}