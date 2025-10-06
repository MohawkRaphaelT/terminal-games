namespace MohawkTerminalGame;

public enum TerminalInputMode
{
    /// <summary>
    ///     Allows for ReadLine as usual
    /// </summary>
    KeyboardReadAndReadLine,

    /// <summary>
    ///     Allows there to be background polling, however it
    ///     means you cannot rely on ReadLine.
    /// </summary>
    BackgroundPollingNoReadLine,
}
