export interface TaxRate {
    id: number;
    countryId: number | null;
    countryName: string | null;
    name: string | null;
    description: string | null;
    percentage: number;
}