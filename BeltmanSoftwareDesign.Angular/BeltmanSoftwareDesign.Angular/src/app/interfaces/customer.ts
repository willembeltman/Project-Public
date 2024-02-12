export interface Customer {
    id: number;
    countryId: number | null;
    countryName: string | null;
    name: string | null;
    description: string | null;
    address: string | null;
    postalcode: string | null;
    place: string | null;
    phoneNumber: string | null;
    invoiceEmail: string | null;
    publiekelijk: boolean;
}