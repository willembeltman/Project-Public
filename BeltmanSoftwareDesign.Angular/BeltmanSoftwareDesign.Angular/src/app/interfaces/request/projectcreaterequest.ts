import { Project } from "../project";

export interface ProjectCreateRequest {
    project: Project | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}