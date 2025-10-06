namespace MohawkTerminalGame;

public class TerminalGame
{
    // Place your variables here


    /// Run once before Execute begins
    public void Setup()
    {
        Program.TerminalExecuteMode = TerminalExecuteMode.ExecuteTime;
        Program.TargetFPS = 60;

        Terminal.SetTitle("Title");
        Terminal.WriteWithWordBreaks = true; // prevent word breaks when wrapping text
        Terminal.WordBreakCharacter = ' '; // break on space
        Terminal.UseRoboType = false; // write out text one character at a time
        Terminal.RoboTypeIntervalMilliseconds = 1; // timer per character
        Terminal.CursorVisible = false; // if the cursor is visible
    }

    // Execute() runs based on Program.TerminalExecuteMode (assign to it in Setup).
    //  ExecuteOnce: runs only once. Once Execute() is done, program closes.
    //  ExecuteLoop: runs in infinite loop. Next iteration starts at the top of Execute().
    //  ExecuteTime: runs at timed intervals (eg. "FPS"). Code tries to run at Program.TargetFPS.
    //               Code must finish within the alloted time frame for this to work well.
    public void Execute()
    {

    }

}
