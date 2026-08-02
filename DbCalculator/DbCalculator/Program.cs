while (true)
{

    Console.WriteLine("--- dB @ 1 Watt Calculator ---");

    // Input gemeten dB
    Console.Write("Voer de gemeten dB in (bijv. 135,3): ");
    if (!double.TryParse(Console.ReadLine()?.Replace('.', ','), out double dbGemeten))
    {
        Console.WriteLine("Ongeldige invoer voor dB.");
        return;
    }

    // Input vermogen in Watt
    Console.Write("Voer het vermogen in Watt in (bijv. 609): ");
    if (!double.TryParse(Console.ReadLine()?.Replace('.', ','), out double watt) || watt <= 0)
    {
        Console.WriteLine("Ongeldige invoer voor Watt (moet groter zijn dan 0).");
        return;
    }

    // Berekening
    double dbWinst = 10 * Math.Log10(watt);
    double dbBijEenWatt = dbGemeten - dbWinst;

    // Resultaat tonen
    Console.WriteLine("\n--- Resultaat ---");
    Console.WriteLine($"Vermogensverhouding: +{dbWinst:F2} dB");
    Console.WriteLine($"{dbGemeten} dB @ {watt}W = {dbBijEenWatt:F2} dB @ 1W");

    //Console.ReadLine();
}
