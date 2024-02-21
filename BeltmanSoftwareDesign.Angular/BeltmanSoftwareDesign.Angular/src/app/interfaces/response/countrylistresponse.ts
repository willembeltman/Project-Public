import { Country } from "../country";
import { State } from "../state";

export interface CountryListResponse {
    countries: Country[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}