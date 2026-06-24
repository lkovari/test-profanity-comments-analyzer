namespace TestProfanityCommentsAnalyzer;

public static class TestHu
{
    // TODO: ezt a szar régi API-t meg kellene szüntetni
    private const string Cimke = "label";

    // Számlázási modul újrapróbálkozási számlálója
    private static int ujraprobaSzam = 2;

    /// <summary>
    /// Visszaadja az újrapróbálkozások számát.
    /// </summary>
    public static int UjraprobaSzam() => ujraprobaSzam;

    // FIXME: ez a blokk borzalmasan olvashatatlan
    public static string CimkeFormatum(string elotag)
    {
        // Az előtag hozzáfűzése a tárolt címke értékhez
        return elotag + Cimke;
    }

    /*
     * Történelmi megjegyzés: az eredeti fejlesztő
     * azért hagyta itt, mert kurva jól tudja miért nem működött.
     */
    public static bool Engedelyezve() => true;

    // Rendes fejlesztői megjegyzés — nincs probléma
    public static void Visszaallitas()
    {
        ujraprobaSzam = 0;
    }
}
