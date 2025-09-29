using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Timers;

namespace vs_launch_external_terminal
{
    internal class Program
    {
        static int i = 0;
        //const string temp_mode = "fps";
        const string temp_mode = "loop";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Console.OutputEncoding = Encoding.Unicode;
            Console.Clear();
            Console.WriteLine("❤️😭✨✅\U0001f979🔥⭐😂❤️‍\U0001fa79");

            // BOOTSRAP into another terminal
            if (args.Length == 0)
            {
                Process process = new();
                string path = Directory.GetCurrentDirectory() + @"\" + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
                Console.WriteLine(path);

                // Config
                //process.StartInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                process.StartInfo.FileName = @"C:\Users\Raphael\AppData\Local\Microsoft\WindowsApps\wt.exe";
                //process.StartInfo.FileName = @"C:\Windows\system32\cmd.exe";
                process.StartInfo.Arguments = $"\"{path}\" \"hi\"";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                // Open in new window
                bool isFirstBoot = args.Length == 0;
                process.StartInfo.UseShellExecute = isFirstBoot;
                process.StartInfo.RedirectStandardOutput = !isFirstBoot;
                process.StartInfo.RedirectStandardInput = !isFirstBoot;

                process.Start();
                process.WaitForExit();
            }
            // Set up core loop
            else if (temp_mode == "fps")
            {
                var timer = new System.Timers.Timer();
                // 60 FPS
                timer.Interval = 1 / 30.0;
                timer.Elapsed += Update;
                timer.Start();

                // Exit loop with ESC check
                while (Console.ReadKey().Key != ConsoleKey.Escape)
                {
                }
            }
            else if (temp_mode == "loop")
            {
                Task task1 = new(CheckForExit);
                Task task2 = new(Loop);
                task1.Start();
                task2.Start();
                Task.WaitAny(task1, task2);
            }
        }

        private static void Update(object? sender, ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine(i++);
        }

        private static void Loop()
        {
            while (true)
            {
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine(i++);
                Console.WriteLine("What's your input?");
                Console.ReadLine();
            }
        }

        private static void CreateBreakThread()
        {
            Task task = new Task(CheckForExit);
            Task.WaitAny(task);
        }
        private static void CheckForExit()
        {
            // Exit loop with ESC check
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
            }
        }

    }
}
