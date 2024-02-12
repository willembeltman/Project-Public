import { State } from "../state";

export interface LoginResponse {
    errorEmailNotValid: boolean;
    authenticationError: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}