using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_launch_external_terminal;

public class TerminalGame
{
    // DO NOT DELETE. Required for setup
    public static readonly bool BootstapIntoWindowsTerminal = true;
    public static readonly ProcessWindowStyle WindowStyle = ProcessWindowStyle.Maximized;
    
    // Variables


    /// Run once before Execute begins
    public void Setup()
    {
        Program.TerminalExecuteMode = TerminalExecuteMode.ExecuteTime;
    }

    // Runs based on mode set in Setup.
    //  ExecuteOnce: runs only once. Once Execute() is done, program closes.
    //  ExecuteLoop: runs in infinite loop. Next iteration starts at the top of Execute().
    //  ExecuteTime: runs at timed intervals (eg. "FPS"). Code tries to run at Program.TargetFPS.
    //               Code must finish within the frame time for this to work.
    public void Execute()
    {
        Terminal.WriteLine(Time.DisplayText);
        //Terminal.WriteLine("Hello, World!");
    }
}
