import { State } from "../state";

export interface RegisterResponse {
    errorEmailNotValid: boolean;
    errorEmailInUse: boolean;
    errorUsernameInUse: boolean;
    errorUsernameEmpty: boolean;
    errorPasswordEmpty: boolean;
    errorPhoneNumberEmpty: boolean;
    errorCouldNotCreateUser: boolean;
    errorCouldNotGetDevice: boolean;
    errorCouldNotCreateBearer: boolean;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}