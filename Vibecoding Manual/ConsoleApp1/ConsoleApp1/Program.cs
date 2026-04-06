class Program
{
    record Persoon(
        string Voornaam,
        string Achternaam,
        int Leeftijd);

    static void Main()
    {
        var path = "personen.dat";
        var persoon1 = new Persoon(
            Voornaam: "Jan",
            Achternaam: "Jansen",
            Leeftijd: 42);
        var persoon2 = new Persoon(
            Voornaam: "Piet",
            Achternaam: "van der Bos",
            Leeftijd: 39);

        // Wegschrijven naar disk
        using (var fileStream = new FileStream(path, FileMode.Create))
        using (var binaryWriter = new BinaryWriter(fileStream))
        {
            WritePersoon(persoon1, binaryWriter);
            WritePersoon(persoon2, binaryWriter);
        }

        persoon1 = null;
        persoon2 = null;

        Console.WriteLine("Records weggeschreven!");

        // Uitlezen vanaf disk
        using (var fileStream = new FileStream(path, FileMode.Open))
        using (var binaryReader = new BinaryReader(fileStream))
        {
            persoon1 = ReadPersoon(binaryReader);
            persoon2 = ReadPersoon(binaryReader);

            Console.WriteLine($"Uitlezen: {persoon1.Voornaam} {persoon1.Achternaam}, {persoon1.Leeftijd} jaar");
            Console.WriteLine($"Uitlezen: {persoon2.Voornaam} {persoon2.Achternaam}, {persoon2.Leeftijd} jaar");
        }
    }
    private static Persoon ReadPersoon(BinaryReader reader)
    {
        return new Persoon(
            reader.ReadString(), // Leest lengte Voornaam (4 bytes), en dan de Voornaam (x bytes)
            reader.ReadString(), // Leest lengte Achternaam (4 bytes), en dan de Achternaam (x bytes)
            reader.ReadInt32()); // Leest Leeftijd als 4 bytes getal
    }

    private static void WritePersoon(Persoon persoon, BinaryWriter writer)
    {
        writer.Write(persoon.Voornaam); // Schrijft lengte Voornaam (4 bytes) en dan de Voornaam (x bytes)
        writer.Write(persoon.Achternaam); // Schrijft lengte Achternaam (4 bytes) en dan de Achternaam (x bytes)
        writer.Write(persoon.Leeftijd); // Schrijft Leeftijd als 4 bytes getal
    }
}