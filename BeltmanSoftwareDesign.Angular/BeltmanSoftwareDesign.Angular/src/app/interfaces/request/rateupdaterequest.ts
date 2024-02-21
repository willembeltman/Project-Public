import { Rate } from "../rate";

export interface RateUpdateRequest {
    rate: Rate | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}