import { Project } from "../project";

export interface ProjectUpdateRequest {
    project: Project | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}