using System.Diagnostics;

namespace vs_launch_external_terminal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Process process = new();

            // Config
            process.StartInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
            process.StartInfo.Arguments = "";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            // Open in new window
            process.StartInfo.UseShellExecute = true;

            process.Start();
            process.WaitForExit();
        }
    }
}
