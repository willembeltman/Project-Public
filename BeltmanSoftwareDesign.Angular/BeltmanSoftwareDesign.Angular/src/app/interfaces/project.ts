export interface Project {
    id: number;
    customerId: number | null;
    customerName: string | null;
    name: string | null;
    publiekelijk: boolean;
}