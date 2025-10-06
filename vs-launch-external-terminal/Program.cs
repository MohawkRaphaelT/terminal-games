using System.Text;
using System.Timers;

namespace MohawkTerminalGame;

/// <summary>
///     
/// </summary>
internal class Program
{
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
        // Set IO aznd clear window.
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;
        Console.Clear();

        // Prep
        // Make sure cursor state is consistent for all OSs.
        Terminal.CursorVisible = true;
        // Spin up new thread for input
        Input.InitInputThread();
        // Create and setup game
        game = new();
        game.Setup();
        // Set up Time helper
        if (Time.AutoStart)
            Time.Start();

        // Core "loop"
        bool doLoop = true;
        while (doLoop && !Input.IsKeyPressed(ConsoleKey.Escape))
        {
            // Refresh inputs
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
                    // Run loop while in this mode
                    while (TerminalExecuteMode == TerminalExecuteMode.ExecuteTime &&
                           !Input.IsKeyPressed(ConsoleKey.Escape))
                    {
                        // Refresh once enough time has passed, unblocking
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
                    string msg = $"{nameof(MohawkTerminalGame.TerminalExecuteMode)}{TerminalExecuteMode}";
                    throw new NotImplementedException(msg);
            }
        }

        // Clear colors before exiting
        Console.ResetColor();
        // Force exit due to threads in background
        Environment.Exit(0);
    }

    private static void GameLoopTimerEvents(object? o, ElapsedEventArgs sender)
    {
        CanGameExecuteTick = true;
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

        const int charsWidth = 128 / 1;
        const int charsHeight = 64 / 1;

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

}
