namespace vs_launch_external_terminal;

/// <summary>
///     
/// </summary>
public static class Input
{
    private readonly static Thread InputThread = CreateInputThread();
    private readonly static Action<ConsoleKey> OnReadKey;
    private readonly static List<ConsoleKey> LastFrameKeys = [];
    private readonly static List<ConsoleKey> CurrentFrameKeys = [];

    public static void PreparePollNextInput()
    {
        LastFrameKeys.Clear();
        LastFrameKeys.AddRange(CurrentFrameKeys);
        CurrentFrameKeys.Clear();
    }

    public static bool IsKeyUp(ConsoleKey key)
    {
        // Up if not currently pressed
        bool state = !CurrentFrameKeys.Contains(key);
        return state;
    }
    public static bool IsKeyDown(ConsoleKey key)
    {
        // Down if currently pressed
        bool state = CurrentFrameKeys.Contains(key);
        return state;
    }
    public static bool IsKeyPressed(ConsoleKey key)
    {
        // Pressed if currently pressed down but was previously unpressed
        bool state = CurrentFrameKeys.Contains(key) && !LastFrameKeys.Contains(key);
        return state;
    }
    public static bool IsKeyReleased(ConsoleKey key)
    {
        // Released if currently unpressed but was previously pressed down
        bool state = !CurrentFrameKeys.Contains(key) && LastFrameKeys.Contains(key);
        return state;
    }

    public static void InitInputThread()
    {
        // Only run once
        if (InputThread != null)
            return;

        CreateInputThread();
    }

    private static Thread CreateInputThread()
    {
        if (InputThread != null)
        {
            string msg = "Input thread already exists!";
            throw new Exception(msg);
        }

        static void ThreadPollKeyboard()
        {
            while (true)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                if (consoleKeyInfo.Key != ConsoleKey.None)
                {
                    CurrentFrameKeys.Add(consoleKeyInfo.Key);
                }
            }
        }
        Thread inputThread = new(ThreadPollKeyboard);
        inputThread.Start();
        return inputThread;
    }
}