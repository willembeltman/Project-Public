import { Workorder } from "../workorder";

export interface WorkorderUpdateRequest {
    workorder: Workorder | null;
    bearerId: string | null;
    currentCompanyId: number | null;
}