export interface WorkorderReadRequest {
    workorderId: number;
    bearerId: string | null;
    currentCompanyId: number | null;
}