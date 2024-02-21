import { Project } from "../project";
import { State } from "../state";

export interface ProjectListResponse {
    projects: Project[];
    success: boolean;
    errorAuthentication: boolean;
    errorItemNotFound: boolean;
    state: State | null;
}