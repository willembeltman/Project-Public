import { State } from "../state";

export interface SetCurrentCompanyResponse {
    companyNotFound: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}