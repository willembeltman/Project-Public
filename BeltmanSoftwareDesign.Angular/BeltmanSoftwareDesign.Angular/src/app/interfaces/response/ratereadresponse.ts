import { Rate } from "../rate";
import { State } from "../state";

export interface RateReadResponse {
    rate: Rate | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}