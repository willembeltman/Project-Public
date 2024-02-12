import { Company } from "../company";
import { State } from "../state";

export interface CompanyCreateResponse {
    company: Company | null;
    errorCompanyNameAlreadyUsed: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}