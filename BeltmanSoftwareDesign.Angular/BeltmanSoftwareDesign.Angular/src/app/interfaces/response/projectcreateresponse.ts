import { Project } from "../project";
import { State } from "../state";

export interface ProjectCreateResponse {
    project: Project | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    errorWrongCompany: boolean;
    state: State | null;
}