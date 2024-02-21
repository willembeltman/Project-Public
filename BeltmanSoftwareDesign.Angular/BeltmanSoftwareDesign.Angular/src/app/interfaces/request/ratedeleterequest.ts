export interface RateDeleteRequest {
    rateId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}