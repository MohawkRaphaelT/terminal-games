using System;
using System.Collections.Generic;
using System.Text;

namespace vs_launch_external_terminal;

public class TerminalGame
{   
    // Place your variables here


    /// Run once before Execute begins
    public void Setup()
    {
        Program.TerminalExecuteMode = TerminalExecuteMode.ExecuteTime;
        Terminal.SetTitle("Title");
        Terminal.WriteWithWordBreaks = true; // prevent word breaks when wrapping text
        Terminal.WordBreakCharacter = ' '; // break on space
        Terminal.UseRoboType = true; // write out text one character at a time
        Terminal.RoboTypeIntervalMilliseconds = 50; // timer per character

        Terminal.SetWindowPosition(0, 0);
        Terminal.SetWindowSize(Terminal.LargestWindowWidth, Terminal.LargestWindowHeight);
    }

    // Execute() runs based on Program.TerminalExecuteMode.
    //  ExecuteOnce: runs only once. Once Execute() is done, program closes.
    //  ExecuteLoop: runs in infinite loop. Next iteration starts at the top of Execute().
    //  ExecuteTime: runs at timed intervals (eg. "FPS"). Code tries to run at Program.TargetFPS.
    //               Code must finish within the frame time for this to work well.
    public void Execute()
    {
        Terminal.ForegroundColor = Random.ColorExcept(ConsoleColor.Black);
        Terminal.Write(Time.DisplayText);
        Terminal.ClearLine();
        //Terminal.WriteLine("asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh ");
        //Terminal.Beep();
        //Terminal.Clear();
        Terminal.ReadAndClearLine();

    }
}
