using System.Diagnostics;

namespace vs_launch_external_terminal;

public static class Time
{
    private static readonly Stopwatch gameTime = new();

    public static bool AutoStart { get; set; } = true;
    public static string TimeFormat { get; set; } = "mm:ss.fff";

    public static string DisplayText => new DateTime().AddSeconds(gameTime.ElapsedMilliseconds / 1000.0).ToString(TimeFormat);
    public static float ElapsedSeconds => gameTime.ElapsedMilliseconds / 1000f;
    public static int ElapsedSecondsWhole => (int)(gameTime.ElapsedMilliseconds / 1000);
    public static int ElapsedMilliseconds => (int)(gameTime.ElapsedMilliseconds);

    public static void Start() => gameTime.Start();
    public static void Stop() => gameTime.Stop();
    public static void Reset() => gameTime.Reset();

}
