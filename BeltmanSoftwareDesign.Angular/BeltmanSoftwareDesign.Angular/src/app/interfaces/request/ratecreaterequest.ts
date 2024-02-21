import { Rate } from "../rate";

export interface RateCreateRequest {
    rate: Rate | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}