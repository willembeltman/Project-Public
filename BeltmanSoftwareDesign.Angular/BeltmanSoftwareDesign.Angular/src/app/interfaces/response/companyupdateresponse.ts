import { Company } from "../company";
import { State } from "../state";

export interface CompanyUpdateResponse {
    company: Company | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}