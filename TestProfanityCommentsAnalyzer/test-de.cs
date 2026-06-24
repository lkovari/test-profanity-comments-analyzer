namespace TestProfanityCommentsAnalyzer;

public static class TestDe
{
    // TODO: diese alte API muss weg — was für ein Mist
    private const string Bezeichnung = "label";

    // Wiederholungszähler für das Abrechnungsmodul
    private static int wiederholungen = 4;

    /// <summary>
    /// Gibt die Anzahl der Wiederholungsversuche zurück.
    /// </summary>
    public static int Wiederholungen() => wiederholungen;

    // FIXME: dieser Block ist ein Albtraum zu warten
    public static string BezeichnungFormatieren(string praefix)
    {
        // Präfix an den gespeicherten Bezeichnungswert anhängen
        return praefix + Bezeichnung;
    }

    /*
     * Historischer Hinweis: der ursprüngliche Autor hat das hier gelassen,
     * weil die Scheiße Drittanbieter-SDK nie dokumentiert wurde.
     */
    public static bool IstAktiv() => true;

    // Normaler Entwicklerkommentar — kein Problem
    public static void Zuruecksetzen()
    {
        wiederholungen = 0;
    }
}
