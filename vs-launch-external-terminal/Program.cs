using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Timers;

namespace vs_launch_external_terminal;

internal class Program
{
    static int i = 0;
    static bool runNatively = false;
    const TerminalMode mode = TerminalMode.Standard;
    const ProcessWindowStyle WindowStyle = ProcessWindowStyle.Maximized;
    const int charsWidth = 32;
    const int charsHeight = 32;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Console.OutputEncoding = Encoding.Unicode;
        Console.Clear();
        //Console.text
        Console.WriteLine("❤️😭✨✅\U0001f979🔥⭐😂❤️‍\U0001fa79");

        // BOOTSRAP into another terminal
        if (args.Length == 0 && !runNatively)
        {
            // Run itself
            Process process = new();
            string path = Directory.GetCurrentDirectory() + @"\" + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            Console.WriteLine(path);

            // Config
            string appDataDir = Environment.GetEnvironmentVariable("AppData") ?? "(ERROR: no environment variable AppData)";
            process.StartInfo.FileName = $"{appDataDir}\\..\\Local\\Microsoft\\WindowsApps\\wt.exe";
            process.StartInfo.Arguments = $"\"{path}\" \"IsBootstrapped\"";
            process.StartInfo.WindowStyle = WindowStyle;     // Typically maximized
            process.StartInfo.UseShellExecute = false;       // Don't use original window, open new window
            //process.StartInfo.RedirectStandardInput = true;  // Redirect IO
            //process.StartInfo.RedirectStandardOutput = true; // Redirect IO
            // Open in new window
            process.Start();
            //Console.SetIn(process.StandardOutput); // Redirect IO
            //Console.SetOut(process.StandardInput); // Redirect IO
            //process.WaitForExit();
        }
        // Set up core loop
        else
        {
            switch (mode)
            {
                case TerminalMode.Standard:
                    ModeStandard();
                    break;

                case TerminalMode.InputWithCharBuffer:
                    ModeInput();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }

    private static void ModeInput()
    {
        // Game loop timer
        var timer = new System.Timers.Timer();
        timer.Interval = 1 / 30.0;
        timer.Elapsed += FrameUpdate;
        timer.Start();

        // Exit loop with ESC check
        while (Console.ReadKey().Key != ConsoleKey.Escape)
        {
        }
    }
    private static void FrameUpdate(object? sender, ElapsedEventArgs e)
    {
        Console.Clear();
        Console.WriteLine(i++);
    }


    private static void CheckForExit()
    {
        // Exit loop with ESC check
        while (Console.ReadKey().Key != ConsoleKey.Escape)
        {
        }
    }

    private static void ModeStandard()
    {
        // Set up thread tasks, loop some
        //Task[] tasks = [
        //            new(Loop),
        //            new(CheckForExit),
        //        ];
        //foreach (var task in tasks)
        //    task.Start();
        //Task.WaitAny(tasks);
        //Console.WriteLine("thing done...");

        //Task task = new(Loop);
        //task.Start();

        //// Exit loop with ESC check
        //while (Console.ReadKey().Key != ConsoleKey.Escape)
        //{
        //}

        // Delay because... Windows terminal sucks
        Thread.Sleep(100);
        Loop();
    }
    private static void Loop()
    {
        //Console.SetWindowPosition(0, 0);
        //Console.SetWindowSize(100, 100);

        Console.SetWindowSize(charsWidth, charsHeight);
        int width = Console.WindowWidth;// / 2; // because emoji are double width
        int height = Console.WindowHeight;// - 1; // leave 1 line for typing

        TerminalFrameBuffer fb = new(width, height, "❤ "); // red heart is half width for whatever reason
        fb.SetCharCol("💚", 4); // green
        fb.SetCharRow("💛", 4); // yellow
        fb.SetCircle("😂", fb.Width / 2, fb.Height / 2, 1); // 
        fb.SetCircle("😂", -3, -3, 16); // 
        //fb.SetCharRow(" ", Console.WindowHeight - 1); // clear bottom line for typing
        Console.Clear();

        int i = 0;
        while (true)
        {
            fb.Reset();
            //fb.SetCircle("😂", fb.Width / 2, fb.Height / 2, ++i);
            fb.SetRectangle("😂", 0, 0, i/2, i++);
            fb.Display();
            Console.ReadLine();
            //Console.Clear();
            // Clear the scrollback
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\x1b[3J");
        }
    }



}
