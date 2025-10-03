using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Timers;

namespace vs_launch_external_terminal;

internal class Program
{
    const int charsWidth = 128 / 1;
    const int charsHeight = 64 / 1;

    private static readonly System.Timers.Timer gameLoopTimer = new();
    private static TerminalGame? game;
    private static bool CanGameExecuteTick = true;
    private static int targetFPS = 20;
    private static TerminalExecuteMode terminalMode = TerminalExecuteMode.ExecuteOnce;

    public static int TargetFPS
    {
        get
        {
            return targetFPS;
        }
        set
        {
            // Set target as reference
            targetFPS = value;
            // Timer is in milliseconds!
            gameLoopTimer.Interval = 1000.0 / targetFPS;
        }
    }
    public static TerminalExecuteMode TerminalExecuteMode
    {
        get
        {
            return terminalMode;
        }
        set
        {
            // Enable / disable timer as needed
            gameLoopTimer.Enabled = value == TerminalExecuteMode.ExecuteTime;
            // Set as usual
            terminalMode = value;
        }
    }


    static void Main(string[] args)
    {
        Console.OutputEncoding = Console.OutputEncoding = Encoding.Unicode;
        Console.Clear();

        // BOOTSTRAP into another terminal
        if (args.Length == 0 && TerminalGame.BootstapIntoWindowsTerminal)
        {
            BoostrapIntoWindowsTerminal();
        }
        // Set up core loop
        else
        {
            Input.InitInputThread();
            game = new();
            game.Setup();
            // Set up timer
            if (Time.AutoStart)
                Time.Start();

            bool doLoop = true;
            while (doLoop && !Input.IsKeyPressed(ConsoleKey.Escape))
            {
                Input.PreparePollNextInput();
                switch (TerminalExecuteMode)
                {
                    case TerminalExecuteMode.ExecuteOnce:
                        game.Execute();
                        // If still in once mode, kill loop
                        if (TerminalExecuteMode == TerminalExecuteMode.ExecuteOnce)
                            doLoop = false;
                        break;
                    case TerminalExecuteMode.ExecuteLoop:
                        game.Execute();
                        break;
                    case TerminalExecuteMode.ExecuteTime:
                        TargetFPS = targetFPS; // Force update interval
                        gameLoopTimer.Elapsed += GameLoopTimerEvents;
                        gameLoopTimer.Start();
                        while (TerminalExecuteMode == TerminalExecuteMode.ExecuteTime &&
                               !Input.IsKeyPressed(ConsoleKey.Escape))
                        {
                            if (CanGameExecuteTick)
                            {
                                CanGameExecuteTick = false;
                                game.Execute();
                            }
                        }
                        gameLoopTimer.Stop();
                        gameLoopTimer.Elapsed -= GameLoopTimerEvents;
                        break;
                    default:
                        string msg = $"{nameof(vs_launch_external_terminal.TerminalExecuteMode)}{TerminalExecuteMode}";
                        throw new NotImplementedException(msg);
                }
            }
        }

        // Force exit due to threads in background
        Environment.Exit(0);
    }

    private static void BoostrapIntoWindowsTerminal()
    {
        // Get path to this executable
        string path = Directory.GetCurrentDirectory() + @"\" + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
        //Console.WriteLine(path);

        // Configure new process with executable
        string appDataDir = Environment.GetEnvironmentVariable("AppData") ?? throw new Exception("No environment variable AppData.");
        Process process = new();
        process.StartInfo.FileName = appDataDir + @"\..\Local\Microsoft\WindowsApps\wt.exe";
        process.StartInfo.Arguments = $"\"{path}\" \"IsBootstrapped\"";
        process.StartInfo.WindowStyle = TerminalGame.WindowStyle;
        process.StartInfo.UseShellExecute = false; // Don't use original window, open new window
        process.Start();
        // Elevate priority to get better performance
        process.PriorityBoostEnabled = true;
        process.PriorityClass = ProcessPriorityClass.RealTime;

        Console.WriteLine($"Boostrapping completed.");
        Console.WriteLine($"Program : {path}");
        Console.WriteLine($"Terminal: {process.StartInfo.FileName}");
    }

    private static void Test()
    {
        //Console.SetWindowPosition(0, 0);
        //Console.SetWindowSize(100, 100);

        //Console.SetWindowSize(charsWidth, charsHeight);
        //int width = Console.WindowWidth;// / 2; // because emoji are double width
        //int height = Console.WindowHeight;// - 1; // leave 1 line for typing

        //TerminalFrameBuffer fb = new(width, height, "❤ "); // red heart is half width for whatever reason
        //fb.SetCharCol("💚", 4); // green
        //fb.SetCharRow("💛", 4); // yellow
        //fb.SetCircle("😂", fb.Width / 2, fb.Height / 2, 1); // 
        //fb.SetCircle("😂", -3, -3, 16); // 
        ////fb.SetCharRow(" ", Console.WindowHeight - 1); // clear bottom line for typing
        //Console.Clear();

        Console.SetWindowSize(charsWidth * 2, charsHeight);
        TerminalFrameBuffer fb = new(charsWidth, charsHeight, "❤ "); // red heart is half width for whatever reason
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);
        fb.Write();
        string a = "😂";
        string b = "❤ ";
        bool state = true;
        int i = 0;
        int width = charsWidth;
        int height = charsHeight;
        int x = 0;
        int y = 0;
        while (true)
        {
            Input.PreparePollNextInput();
            ////fb.Reset();
            ////fb.SetCircle("😂", fb.Width / 2, fb.Height / 2, ++i);
            ////fb.SetRectangle("😂", 0, 0, i/2, i++);
            ////fb[i % width, i / width] = state ? a : b;

            //int x = i % width; // double width emoji
            //int y = i / width;
            //Console.SetCursorPosition(x * 2, y);
            //Console.Write(state ? a : b);

            //i++;
            //if (i == width * height)
            //{
            //    i = 0;
            //    state = !state;
            //}
            ////Console.ReadLine();
            ////Console.Clear();
            //// Clear the scrollback
            ////Console.WriteLine("\x1b[3J");

            if (Input.IsKeyPressed(ConsoleKey.RightArrow))
            {
                x++;
                fb[x, y] = "😂";
            }
        }
    }


    private static void GameLoopTimerEvents(object? o, ElapsedEventArgs sender)
    {
        CanGameExecuteTick = true;
    }
}
