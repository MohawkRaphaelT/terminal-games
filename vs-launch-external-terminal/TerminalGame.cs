using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace vs_launch_external_terminal;

public class TerminalGame
{
    // DO NOT DELETE. Required for program configuration.
    public static readonly bool BootstapIntoWindowsTerminal = true;
    public static readonly ProcessWindowStyle WindowStyle = ProcessWindowStyle.Maximized;
    
    // Variables


    /// Run once before Execute begins
    public void Setup()
    {
        Program.TerminalExecuteMode = TerminalExecuteMode.ExecuteTime;
    }

    // Execute() runs based on Program.TerminalExecuteMode.
    //  ExecuteOnce: runs only once. Once Execute() is done, program closes.
    //  ExecuteLoop: runs in infinite loop. Next iteration starts at the top of Execute().
    //  ExecuteTime: runs at timed intervals (eg. "FPS"). Code tries to run at Program.TargetFPS.
    //               Code must finish within the frame time for this to work well.
    public void Execute()
    {
        Console.ForegroundColor = Random.ColorExcept(ConsoleColor.Black);
        Terminal.WriteLine(Time.DisplayText);
    }
}
