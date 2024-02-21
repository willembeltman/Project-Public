import { Rate } from "../rate";
import { State } from "../state";

export interface RateListResponse {
    rates: Rate[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}