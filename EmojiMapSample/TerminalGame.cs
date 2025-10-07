using System.Globalization;
using System.Text.Unicode;

namespace MohawkTerminalGame
{
    public class TerminalGame
    {
        // 🏚⛰🌊⭐✨🔰🌲🌳

        // Place your variables here
        TerminalGrid map;
        Stack<ConsoleColor> fgColors = [];
        Stack<ConsoleColor> bgColors = [];
        const string tree1 = "🌲"; // 2 wide
        const string tree2 = "🌳"; // 2 wide
        const string house = "🏚 "; // 1 wide
        const string water = "🌊"; // 2 wide
        const string mountain = "⛰ "; // 1 wide
        const string sparkle = "✨";
        const string player1 = "🧑";
        const string player2 = "👧";
        bool inputChanged;
        int oldPlayerX;
        int oldPlayerY;
        int playerX = 5;
        int playerY = 0;
        string player;

        int Width => Terminal.WindowWidth / 2;
        int Height => Terminal.WindowHeight - 1;

        /// Run once before Execute begins
        public void Setup()
        {
            // Run program at timed intervals.
            Program.TerminalExecuteMode = TerminalExecuteMode.ExecuteTime;
            Program.TerminalInputMode = TerminalInputMode.EnableInputDisableReadLine;
            Program.TargetFPS = 60;
            // Prepare some terminal settings
            Terminal.SetTitle("Dungeon Crawler Sample");
            Terminal.CursorVisible = false; // hide cursor

            // Choose randmo player icon
            player = Random.Element(player1, player2);

            // Set map to some values
            map = new(Width, Height, "XX");

            // Set up forest
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    string tree = Random.Element(tree1, tree2);
                    map.Set(tree, x, y);
                }
            }

            // Add some random mountain clusters
            int numMountains = (int)MathF.Sqrt(Width * Height) * 4;
            for (int i = 0; i < numMountains; i++)
            {
                int x = Random.Integer(0, Width);
                int y = Random.Integer(0, Height);
                int r = Random.Element(0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2);
                map.SetCircle(mountain, x, y, r);
            }

            // Clear an area in the middle
            int radius = 10;
            int midX = Width / 2;
            int midY = Height / 2;
            map.SetCircle("  ", midX, midY, radius);

            // Draw some houses in that centre area
            int numHouses = Random.Integer(20, 40);
            for (int i = 0; i < numHouses; i++)
            {
                int r = radius * 2 / 3;
                int x = Random.Integer(midX - r, midX + r);
                int y = Random.Integer(midY - r, midY + r);
                map.Set(house, x, y);
            }

            // Clear window and draw map
            Terminal.BackgroundColor = ConsoleColor.DarkGreen;
            map.ClearWrite();
            // Draw player. x2 because my tileset is 2 columns wide.
            DrawCharacter(playerX, playerY, player);
        }

        // Execute() runs based on Program.TerminalExecuteMode (assign to it in Setup).
        //  ExecuteOnce: runs only once. Once Execute() is done, program closes.
        //  ExecuteLoop: runs in infinite loop. Next iteration starts at the top of Execute().
        //  ExecuteTime: runs at timed intervals (eg. "FPS"). Code tries to run at Program.TargetFPS.
        //               Code must finish within the alloted time frame for this to work well.
        public void Execute()
        {
            // Move player
            CheckMovePlayer();

            // Naive approach, works but it's much but slower
            //map.Overwrite(0,0);
            //map.Poke(playerX * 2, playerY, player);

            // Only move player if needed
            if (inputChanged)
            {
                ResetCell(oldPlayerX, oldPlayerY);
                DrawCharacter(playerX, playerY, player);
                inputChanged = false;
            }

            // Write time below game
            Terminal.SetCursorPosition(0, Height);
            PushTerminalColors();
            Terminal.ResetColor();
            Terminal.ClearLine();
            Terminal.Write(Time.DisplayText);
            PopTerminalColors();
        }

        void CheckMovePlayer()
        {
            //
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

            playerX = Math.Clamp(playerX, 0, Width - 1);
            playerY = Math.Clamp(playerY, 0, Height - 1);

            if (oldPlayerX != playerX || oldPlayerY != playerY)
                inputChanged = true;
        }

        void DrawCharacter(int x, int y, string character)
        {
            // Character (eg. player) and grid are 2-width characters
            map.Poke(x * 2, y, player);
        }

        void ResetCell(int x, int y)
        {
            string mapText = map.Get(x, y);
            // Player and grid are 2-width characters
            map.Poke(x * 2, oldPlayerY, mapText);
        }

        void PushTerminalColors()
        {
            fgColors.Push(Terminal.ForegroundColor);
            bgColors.Push(Terminal.BackgroundColor);
        }

        void PopTerminalColors()
        {
            if (fgColors.Count > 0)
                Terminal.ForegroundColor = fgColors.Pop();
            if (bgColors.Count > 0)
                Terminal.BackgroundColor = bgColors.Pop();
        }

    }
}
