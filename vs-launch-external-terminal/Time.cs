using System.Diagnostics;

namespace vs_launch_external_terminal;

/// <summary>
///     Time related information.
/// </summary>
public static class Time
{
    private static readonly Stopwatch stopwatch = new();

    public static bool AutoStart { get; set; } = true;
    public static string TimeFormat { get; set; } = "mm:ss.fff";

    public static string DisplayText => new DateTime().AddSeconds(stopwatch.ElapsedMilliseconds / 1000.0).ToString(TimeFormat);
    public static float ElapsedSeconds => stopwatch.ElapsedMilliseconds / 1000f;
    public static int ElapsedSecondsWhole => (int)(stopwatch.ElapsedMilliseconds / 1000);
    public static int ElapsedMilliseconds => (int)(stopwatch.ElapsedMilliseconds);

    public static void Start() => stopwatch.Start();
    public static void Stop() => stopwatch.Stop();
    public static void Reset() => stopwatch.Reset();

}
