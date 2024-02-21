import { Project } from "../project";
import { State } from "../state";

export interface ProjectReadResponse {
    project: Project | null;
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}