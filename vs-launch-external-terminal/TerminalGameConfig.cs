using System.Diagnostics;

namespace vs_launch_external_terminal;

/// <summary>
///     Configuration for terminal game.
/// </summary>
public static class TerminalGameConfig
{
    /// <summary>
    ///     Whether or not to use external terminal on Windows 10.
    /// </summary>
    public static readonly bool Win10_UseExternalTerminal = true;
    
    /// <summary>
    ///     Whether or not to use external terminal on Windows 11.
    /// </summary>
    public static readonly bool Win11_UseExternalTerminal = false;

    /// <summary>
    ///     Property to detect OS and indicate if external process is required.
    /// </summary>
    public static bool UseExternalWindowsTerminal
    {
        get
        {
            // Detect Windows 10
            OperatingSystem os = Environment.OSVersion;
            bool isWindows = os.Platform == PlatformID.Win32NT;
            bool isWin10 = os.Version.Major == 10;
            if (isWindows && isWin10)
                return Win10_UseExternalTerminal;
            // Ensure we are Windows 11
            bool isWin11 = os.Version.Major == 11;
            if (isWindows && isWin11)
                return Win11_UseExternalTerminal;
            // If Windows but not Win10 or Win11, error out
            if (isWindows)
            {
                string msg = $"Unhandled Windows OS version: {os.VersionString}";
                throw new Exception(msg);
            }
            // Any other OS, assume built-in terminal is good
            return false;
        }
    }

    /// <summary>
    ///     How the terminal window is opened IF opening an external window.
    /// </summary>
    public static readonly ProcessWindowStyle WindowStyle = ProcessWindowStyle.Maximized;

}
