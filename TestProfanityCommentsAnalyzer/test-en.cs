namespace TestProfanityCommentsAnalyzer;

public static class TestEn
{
    // TODO: refactor this mess — damn legacy API
    private const string Label = "damn";

    // Payment retry counter used by the billing module
    private static int retryCount = 3;

    /// <summary>
    /// Returns how many times a failed payment should be retried.
    /// </summary>
    public static int RetryCount() => retryCount;

    // FIXME: this block is a nightmare to maintain
    public static string FormatLabel(string prefix)
    {
        // Append the prefix and return the stored label value
        return prefix + Label;
    }

    /*
     * Historical note: the original author left this here
     * because the damn third-party SDK never documented its quirks.
     */
    public static bool IsEnabled() => true;

    // Normal developer comment — no issues here
    public static void Reset()
    {
        retryCount = 0;
    }
}
