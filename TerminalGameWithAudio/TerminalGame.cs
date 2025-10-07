using System;

namespace MohawkTerminalGame
{
    public class TerminalGame
    {
        // Place your variables here
        Music bgm;
        Sound sfx1;
        Sound sfx2;
        Sound sfx3;

        /// Run once before Execute begins
        public void Setup()
        {
            // Program configuration
            Program.TerminalExecuteMode = TerminalExecuteMode.ExecuteLoop;
            Program.TerminalInputMode = TerminalInputMode.KeyboardReadAndReadLine;

            // Hide raylib console output
            Terminal.ForegroundColor = ConsoleColor.Black;
            Terminal.CursorVisible = false;
            Audio.Initialize();
            // Load audio files. This must happen AFTER initializing audio.
            // If you are so inclined, you can move Audio.Initialize() into Program beside Input.InitInputThread()
            bgm = Audio.LoadMusic("../../../../assets/audio/country.mp3");
            sfx1 = Audio.LoadSound("../../../../assets/audio/sound.wav");
            sfx2 = Audio.LoadSound("../../../../assets/audio/target.ogg");
            sfx3 = Audio.LoadSound("../../../../assets/audio/boom.wav");
            // Move curosr to overwrite previously drawn (black) text
            Terminal.SetCursorPosition(0, 0);
            Terminal.ResetColor();
            Terminal.CursorVisible = true;
        }

        // Execute() runs based on Program.TerminalExecuteMode (assign to it in Setup).
        //  ExecuteOnce: runs only once. Once Execute() is done, program closes.
        //  ExecuteLoop: runs in infinite loop. Next iteration starts at the top of Execute().
        //  ExecuteTime: runs at timed intervals (eg. "FPS"). Code tries to run at Program.TargetFPS.
        //               Code must finish within the alloted time frame for this to work well.
        public void Execute()
        {
            Terminal.WriteLine("Select a sound file to play.");
            Terminal.WriteLine("SOUND, TARGET, BOOM, BGM");
            string audioToPlay = Terminal.ReadLine();
            if (audioToPlay.ToUpper().Equals("SOUND"))
            {
                Audio.Play(sfx1);
            }
            else if (audioToPlay.ToUpper().Equals("TARGET"))
            {
                Audio.Play(sfx2);
            }
            else if (audioToPlay.ToUpper().Equals("BOOM"))
            {
                Audio.Play(sfx3);
            }
            else if (audioToPlay.ToUpper().Equals("BGM"))
            {
                // Toggle BGM
                if (!Audio.IsPlaying(bgm))
                    Audio.Play(bgm);
                else
                    Audio.Stop(bgm);
            }
            else
            {
                Terminal.WriteLine($"{audioToPlay.ToUpper()} is not an option.");
            }
            Terminal.WriteLine();
        }
    }
}
