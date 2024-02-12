export interface WorkorderDeleteRequest {
    workorderId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}