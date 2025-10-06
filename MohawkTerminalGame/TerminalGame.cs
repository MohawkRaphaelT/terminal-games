using System.Threading.Tasks.Dataflow;

namespace MohawkTerminalGame;

public class TerminalGame
{
    // Place your variables here
    TerminalGridWithColor grid;
    ColoredText tree = new(@"/\", ConsoleColor.Green, ConsoleColor.DarkGreen);
    ColoredText riverNS = new(@"||", ConsoleColor.Blue, ConsoleColor.DarkBlue);
    ColoredText riverEW = new(@"==", ConsoleColor.Blue, ConsoleColor.DarkBlue);
    ColoredText player = new(@"😎", ConsoleColor.White, ConsoleColor.Black);
    bool inputChanged;
    int oldPlayerX;
    int oldPlayerY;
    int playerX = 5;
    int playerY = 0;

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
        Terminal.CursorVisible = false;
        //Terminal.SetWindowSize(Terminal.LargestWindowWidth, Terminal.LargestWindowHeight);

        grid = new(10, 10, tree);
        grid.SetCol(riverNS, 3);
        grid.SetRow(riverEW, 8);

        // Clear window and draw BG
        grid.ClearWrite();
        grid.Poke(playerX * 2, playerY, player);
    }

    // Execute() runs based on Program.TerminalExecuteMode (assign to it in Setup).
    //  ExecuteOnce: runs only once. Once Execute() is done, program closes.
    //  ExecuteLoop: runs in infinite loop. Next iteration starts at the top of Execute().
    //  ExecuteTime: runs at timed intervals (eg. "FPS"). Code tries to run at Program.TargetFPS.
    //               Code must finish within the alloted time frame for this to work well.
    public void Execute()
    {
        //Terminal.ForegroundColor = Random.ConsoleColorExcept(ConsoleColor.Black);
        //Terminal.Write(Time.DisplayText);
        //Terminal.ClearLine();
        //Terminal.WriteLine("asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh asdkjash h hkjhh ");
        //Terminal.Beep();
        //Terminal.Clear();
        //Terminal.ReadAndClearLine();

        CheckMovePlayer();
        if (inputChanged)
        {
            // Reset old tile
            grid.Poke(oldPlayerX * 2, oldPlayerY, grid.Get(oldPlayerX, oldPlayerY));
            // Update new tile
            grid.Poke(playerX * 2, playerY, player);
            //
            inputChanged = false;
        }

        Console.SetCursorPosition(0, 12);
        Console.ResetColor();
       // Terminal.ClearLine();
        Console.Write(Time.DisplayText);
        //Console.Write(" ");
        //foreach (var x in Input.CurrentFrameKeys)
        //    Console.Write(x.ToString());
    }

    void CheckMovePlayer()
    {
        inputChanged = false;
        oldPlayerX = playerX;
        oldPlayerY = playerY;

        if (Input.IsKeyPressed(ConsoleKey.RightArrow))
            playerX++;
        if (Input.IsKeyPressed(ConsoleKey.LeftArrow))
            playerX--;
        if (Input.IsKeyPressed(ConsoleKey.DownArrow))
            playerY++;
        if (Input.IsKeyPressed(ConsoleKey.UpArrow))
            playerY--;

        playerX = Math.Clamp(playerX, 0, grid.Width - 1);
        playerY = Math.Clamp(playerY, 0, grid.Height - 1);

        if (oldPlayerX != playerX || oldPlayerY != playerY)
            inputChanged = true;
    }
}
