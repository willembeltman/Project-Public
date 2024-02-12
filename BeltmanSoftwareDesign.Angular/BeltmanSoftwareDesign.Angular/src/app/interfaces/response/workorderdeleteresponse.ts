import { State } from "../state";

export interface WorkorderDeleteResponse {
    errorWorkorderNotFound: boolean;
    errorCurrentCompanyDifferentThanWorkorderCompany: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}