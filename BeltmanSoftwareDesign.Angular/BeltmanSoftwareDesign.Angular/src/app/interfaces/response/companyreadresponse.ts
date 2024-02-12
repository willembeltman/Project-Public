import { Company } from "../company";
import { State } from "../state";

export interface CompanyReadResponse {
    company: Company | null;
    companyNotFound: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}