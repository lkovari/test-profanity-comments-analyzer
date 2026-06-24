namespace TestProfanityCommentsAnalyzer;

public static class TestRo
{
    // TODO: refactorizare mizeria asta — la naiba cu API-ul vechi
    private const string Eticheta = "label";

    // Contor de reîncercări pentru modulul de facturare
    private static int reincercari = 1;

    /// <summary>
    /// Returnează numărul de reîncercări de plată.
    /// </summary>
    public static int Reincercari() => reincercari;

    // FIXME: acest bloc e un coșmar de întreținut
    public static string FormateazaEticheta(string prefix)
    {
        // Adaugă prefixul la valoarea etichetei stocate
        return prefix + Eticheta;
    }

    /*
     * Notă istorică: autorul original a lăsat asta aici
     * pentru că dracu știe de ce SDK-ul terț nu era documentat.
     */
    public static bool Activat() => true;

    // Comentariu normal al dezvoltatorului — fără probleme
    public static void Resetare()
    {
        reincercari = 0;
    }
}
