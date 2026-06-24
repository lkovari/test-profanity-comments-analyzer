namespace TestProfanityCommentsAnalyzer;

public static class TestIt
{
    // TODO: rifattorizzare questo casino — che cavolo fa questo codice
    private const string Etichetta = "label";

    // Contatore di tentativi per il modulo di fatturazione
    private static int tentativi = 5;

    /// <summary>
    /// Restituisce il numero di tentativi di pagamento.
    /// </summary>
    public static int Tentativi() => tentativi;

    // FIXME: questo blocco è un incubo da mantenere
    public static string FormattaEtichetta(string prefisso)
    {
        // Aggiunge il prefisso al valore dell'etichetta memorizzato
        return prefisso + Etichetta;
    }

    /*
     * Nota storica: l'autore originale ha lasciato questo commento
     * perché il maledetto SDK di terze parti non era documentato.
     */
    public static bool Abilitato() => true;

    // Commento normale dello sviluppatore — nessun problema
    public static void Reimposta()
    {
        tentativi = 0;
    }
}
