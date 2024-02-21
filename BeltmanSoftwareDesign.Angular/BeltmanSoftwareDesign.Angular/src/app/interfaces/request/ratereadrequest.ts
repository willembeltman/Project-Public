export interface RateReadRequest {
    rateId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}