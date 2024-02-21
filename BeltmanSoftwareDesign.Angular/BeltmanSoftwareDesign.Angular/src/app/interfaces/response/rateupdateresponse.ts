import { Rate } from "../rate";
import { State } from "../state";

export interface RateUpdateResponse {
    rate: Rate | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}