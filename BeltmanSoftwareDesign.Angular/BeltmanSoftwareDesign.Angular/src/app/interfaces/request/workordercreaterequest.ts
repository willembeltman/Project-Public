import { Workorder } from "../workorder";

export interface WorkorderCreateRequest {
    workorder: Workorder | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}