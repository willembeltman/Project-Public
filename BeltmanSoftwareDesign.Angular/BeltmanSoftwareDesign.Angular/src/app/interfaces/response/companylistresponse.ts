import { Company } from "../company";
import { State } from "../state";

export interface CompanyListResponse {
    companies: Company[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}