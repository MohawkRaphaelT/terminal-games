using System.Globalization;
using System.Text.Unicode;

namespace MohawkTerminalGame
{
    public class TerminalGame
    {
        // 🏚⛰🌊⭐✨🔰🌲🌳

        // Place your variables here
        TerminalGrid map;
        const string tree1 = "🌲"; // 
        const string tree2 = "🌳";
        const string house = "🏚";
        const string water = "🌊";
        const string mountain = "⛰ "; // mountain is 1 wide
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

            //
            player = Random.Element(player1, player2);

            // Set map to some values
            map = new(Width, Height, "..");
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    string tree = Random.Element(tree1, tree2);
                    map.Set(tree, x, y);
                }
            }

            int numMountains = (int)MathF.Sqrt(Width * Height) * 10;
            for (int i = 0; i < numMountains; i++)
            {
                int x = Random.Integer(0, Width);
                int y = Random.Integer(0, Height);
                map.Set(mountain, x, y);
            }

            // Clear window and draw map
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
                //ResetCell(oldPlayerX, oldPlayerY);
                map.Overwrite();
                DrawCharacter(playerX, playerY, player);
                inputChanged = false;
            }

            // Write time below game
            Terminal.SetCursorPosition(0, Height - 1);
            Terminal.ResetColor();
            Terminal.Write(Time.DisplayText);
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

    }
}
