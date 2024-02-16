export interface ProjectReadRequest {
    projectId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}