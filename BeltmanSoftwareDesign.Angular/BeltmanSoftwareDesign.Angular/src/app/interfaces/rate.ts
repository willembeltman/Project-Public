export interface Rate {
    id: number;
    taxRateId: number | null;
    taxRateName: string | null;
    name: string | null;
    description: string | null;
    price: number;
}