using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Numerics;

namespace vs_launch_external_terminal
{
    public enum TerminalMode
    {
        Standard,
        InputWithCharBuffer,
    }

    public struct Rectangle
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public readonly int StartX => x;
        public readonly int EndX => x + w;
        public readonly int StartY => y;
        public readonly int EndY => y + h;

        public Rectangle()
        {
        }
        public Rectangle(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public static Rectangle FromCorners(int x1, int y1, int x2, int y2)
        {
            // Lower bounds for X1
            if (x1 < 0)
            {
                string msg = $"Corner value {nameof(x1)} is {x1} but must be a minimum of 0.";
                throw new ArgumentOutOfRangeException(msg);
            }
            // Lower bounds for X2
            if (x2 < 0)
            {
                string msg = $"Corner value {nameof(x2)} is {x2} but must be a minimum of 0.";
                throw new ArgumentOutOfRangeException(msg);
            }
            // Lower bounds for Y1
            if (y1 < 0)
            {
                string msg = $"Corner value {nameof(y1)} is {y1} but must be a minimum of 0.";
                throw new ArgumentOutOfRangeException(msg);
            }
            // Lower bounds for Y2
            if (y1 < 0)
            {
                string msg = $"Corner value {nameof(y2)} is {y2} but must be a minimum of 0.";
                throw new ArgumentOutOfRangeException(msg);
            }

            Rectangle rectangle = new()
            {
                x = x1,
                y = y1,
                w = x2 - x1,
                h = y2 - y1,
            };
            return rectangle;
        }
        public static Rectangle FromCentre(int x, int y, int w, int h)
        {
            // Lower bounds for X
            if (x < 0)
            {
                string msg = $"Centre value {nameof(x)} is {x} but must be a minimum of 0.";
                throw new ArgumentOutOfRangeException(msg);
            }
            // Lower bounds for Y
            if (y < 0)
            {
                string msg = $"Centre value {nameof(y)} is {y} but must be a minimum of 0.";
                throw new ArgumentOutOfRangeException(msg);
            }
            // Lower bounds for W
            if (w < 1)
            {
                string msg = $"Width {nameof(w)} is {w} but must be a minimum of 1.";
                throw new ArgumentOutOfRangeException(msg);
            }
            // Lower bounds for H
            if (h < 1)
            {
                string msg = $"Height {nameof(h)} is {h} but must be a minimum of 1.";
                throw new ArgumentOutOfRangeException(msg);
            }

            int offsetX = (w - 1) / 2;
            int offsetY = (h - 1) / 2;
            Rectangle rectangle = new()
            {
                x = x - offsetX,
                y = y - offsetY,
                w = w,
                h = h,
            };
            return rectangle;
        }
    }

    public struct TerminalText
    {
        public ConsoleColor fgColor;
        public ConsoleColor bgColor;
        public string text;

        public TerminalText(string text = "  ") : this(text, ConsoleColor.White, ConsoleColor.Black)
        {
        }

        public TerminalText(string text, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            this.text = text;
            this.fgColor = fgColor;
            this.bgColor = bgColor;
        }

        public static implicit operator string(TerminalText terminalText)
        {
            return terminalText.text;
        }

        public static implicit operator TerminalText(string stringValue)
        {
            return new TerminalText(stringValue);
        }
    }

    public sealed class TerminalFrameBuffer
    {
        public const string DefaultText = "  ";

        private readonly TerminalText[,] BackingArray;
        private readonly StringBuilder stringBuilder = new();

        public TerminalText this[int x, int y]
        {
            get => BackingArray[x, y];
            set => BackingArray[x, y] = value;
        }

        public int Width { get; init; }
        public int Height { get; init; }
        public TerminalText DefaultValue { get; set; }


        public TerminalFrameBuffer(TerminalText defaultValue) : this(Console.WindowWidth, Console.WindowHeight, defaultValue)
        {
        }

        public TerminalFrameBuffer(int widthInChars, int heightInChars) : this(Console.WindowWidth, Console.WindowHeight, new TerminalText(DefaultText))
        {
        }

        public TerminalFrameBuffer(int widthInChars, int heightInChars, TerminalText defaultValue)
        {
            Width = widthInChars;
            Height = heightInChars;
            BackingArray = new TerminalText[widthInChars, heightInChars];
            DefaultValue = defaultValue;
            SetAll(defaultValue);
        }

        public void Reset()
        {
            SetAll(DefaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetAll(TerminalText value)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    BackingArray[x, y] = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        public void SetRow(TerminalText value, int row)
        {
            // Ignore malformed request
            if (row < 0 || row >= Height)
                return;

            //// Min bounds
            //if (row < 0)
            //{
            //    string msg = $"Row value {row} is less than zero.";
            //    throw new ArgumentOutOfRangeException(msg);
            //}
            //// Max bounds
            //if (row >= Height)
            //{
            //    string msg = $"Row value {row} is is greater than or equal to {Height}.";
            //    throw new ArgumentOutOfRangeException(msg);
            //}

            // Set row
            for (int x = 0; x < Width; x++)
            {
                BackingArray[x, row] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="col"></param>
        public void SetCol(TerminalText value, int col)
        {
            // Ignore malformed request
            if (col < 0 || col >= Width)
                return;

            //// Min bounds
            //if (col < 0)
            //{
            //    string msg = $"Column value {col} is less than zero.";
            //    throw new ArgumentOutOfRangeException(msg);
            //}
            //// Max bounds
            //if (col >= Width)
            //{
            //    string msg = $"Column value {col} is is greater than or equal to {Width}.";
            //    throw new ArgumentOutOfRangeException(msg);
            //}

            // Set row
            for (int y = 0; y < Height; y++)
            {
                BackingArray[col, y] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="area"></param>
        public void SetRectangle(TerminalText value, Rectangle area)
        {
            int minX = int.Max(area.StartX, 0);
            int minY = int.Max(area.StartY, 0);
            int maxX = int.Min(area.EndX, Width);
            int maxY = int.Min(area.EndY, Height);
            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    BackingArray[x, y] = value;
                }
            }
        }

        public void SetRectangle(TerminalText value, int x, int y, int w, int h) => SetRectangle(value, new Rectangle(x, y, w, h));

        public void SetCircle(TerminalText value, int cx, int cy, float r)
        {
            int minX = int.Max((int)Math.Floor(cx - r), 0);
            int minY = int.Max((int)Math.Floor(cy - r), 0);
            int maxX = int.Min((int)Math.Ceiling(cx + r), Width - 1);
            int maxY = int.Min((int)Math.Ceiling(cy + r), Height - 1);
            float threshold = r + 0.25f;
            Vector2 centre = new(cx, cy);
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    // Is cell inside circle?
                    Vector2 point = new(x, y);
                    float distance = Vector2.Distance(centre, point);
                    if (distance <= threshold)
                    {
                        BackingArray[x, y] = value;
                    }
                }
            }
        }


        /// <summary>
        ///     Create a new buffer from the <paramref name="area"/> specified of
        ///     the source buffer <paramref name="src"/>.
        /// </summary>
        /// <param name="src">The source buffer to copy from.</param>
        /// <param name="area">The area to copy.</param>
        /// <returns>
        ///     A new buffer of the specified <paramref name="area"/> of <paramref name="src"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="area"/> specified does not fit into <paramref name="src"/>.
        /// </exception>

        public static TerminalFrameBuffer CopyArea(TerminalFrameBuffer src, Rectangle area)
        {
            if (area.EndX > src.Width)
            {
                string msg = $"{nameof(area)}.{nameof(area.EndX)} value {area.EndX} is larger than {nameof(src)} width {src.Width}.";
                throw new ArgumentOutOfRangeException(msg);
            }
            if (area.EndY > src.Height)
            {
                string msg = $"{nameof(area)}.{nameof(area.EndY)} value {area.EndY} is larger than {nameof(src)} height {src.Height}.";
                throw new ArgumentOutOfRangeException(msg);
            }

            TerminalFrameBuffer copy = new TerminalFrameBuffer(area.w, area.h);
            for (int y = 0; y < area.y; y++)
            {
                int srcY = y + area.StartY;
                int dstY = y;
                for (int x = 0; x < area.x; x++)
                {
                    int srcX = x + area.StartX;
                    int dstX = x;
                    copy.BackingArray[dstX, dstY] = src.BackingArray[srcX, srcY];
                }
            }
            return copy;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcRect"></param>
        /// <param name="dest"></param>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 
        /// </exception>
        public static void CopyFromTo(TerminalFrameBuffer src, Rectangle srcRect, TerminalFrameBuffer dest, int destX, int destY)
        {
            if (srcRect.EndX > src.Width)
            {
                string msg = $"{nameof(srcRect)}.{nameof(srcRect.EndX)} value {srcRect.EndX} is larger than {nameof(src)} width {src.Width}.";
                throw new ArgumentOutOfRangeException(msg);
            }
            if (srcRect.EndY > src.Height)
            {
                string msg = $"{nameof(srcRect)}.{nameof(srcRect.EndY)} value {srcRect.EndY} is larger than {nameof(src)} height {src.Height}.";
                throw new ArgumentOutOfRangeException(msg);
            }

            // Copy area of source
            TerminalFrameBuffer srcAreaCopy = CopyArea(src, srcRect);
            // Place into destination
            for (int y = 0; y < srcRect.y; y++)
            {
                int srcY = y;
                int dstY = y + srcRect.h;
                for (int x = 0; x < srcRect.w; x++)
                {
                    int srcX = x;
                    int dstX = x + srcRect.x;
                    dest.BackingArray[dstX, dstY] = srcAreaCopy.BackingArray[srcX, srcY];
                }
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="area"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void CopyToSelf(Rectangle area, int newX, int newY) => CopyFromTo(this, area, this, newX, newY);

        public override string ToString()
        {
            stringBuilder.Clear();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    stringBuilder.Append(BackingArray[x, y]);
                }
                stringBuilder.AppendLine();
            }
            string result = stringBuilder.ToString();
            return result;
        }

        public void WriteToConsole()
        {
            ////Console.Clear();
            //string display = this.ToString();
            //// Reset cursor position
            //Console.SetCursorPosition(0, 0);
            //Console.Write(display);

            // Store
            var bg = Console.BackgroundColor;
            var fg = Console.BackgroundColor;
            // Write
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    TerminalText value = BackingArray[x, y];
                    if (Console.BackgroundColor != value.bgColor)
                        Console.BackgroundColor = value.bgColor;
                    if (Console.ForegroundColor != value.fgColor)
                        Console.ForegroundColor = value.fgColor;
                    Console.Write(value.text);
                }
                Console.WriteLine();
            }
            // Restore
            if (Console.BackgroundColor != bg)
                Console.BackgroundColor = bg;
            if (Console.ForegroundColor != fg)
                Console.ForegroundColor = fg;
        }
    }



    internal class Program
    {
        static int i = 0;
        static bool runNatively = false;
        const TerminalMode mode = TerminalMode.Standard;
        const ProcessWindowStyle Window = ProcessWindowStyle.Maximized;
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
                // test
                //Console.SetWindowPosition(0, 0);
                //Console.SetWindowSize(Console.ma);

                Process process = new();
                string path = Directory.GetCurrentDirectory() + @"\" + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
                Console.WriteLine(path);

                // Config
                //process.StartInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                process.StartInfo.FileName = @"C:\Users\Raphael\AppData\Local\Microsoft\WindowsApps\wt.exe";
                //process.StartInfo.FileName = @"C:\Windows\system32\cmd.exe";
                //process.StartInfo.FileName = @"C:\Windows\system32\cmd.exe";
                process.StartInfo.Arguments = $"\"{path}\" \"IsBootstrapped\"";
                process.StartInfo.WindowStyle = Window;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                // Open in new window
                process.Start();
                Console.SetIn(process.StandardOutput);
                Console.SetOut(process.StandardInput);
                process.WaitForExit();
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
            fb.SetCol("💚", 4); // green
            fb.SetRow("💛", 4); // yellow
            fb.SetCircle("😂", fb.Width / 2, fb.Height / 2, 1); // 
            fb.SetCircle("😂", -3, -3, 16); // 
            //fb.SetCharRow(" ", Console.WindowHeight - 1); // clear bottom line for typing
            Console.Clear();

            int i = 0;
            while (true)
            {
                fb.Reset();
                //fb.SetCircle("😂", fb.Width / 2, fb.Height / 2, ++i);
                fb.SetRectangle("😂", 0, 0, i / 2, i++);
                fb.WriteToConsole();
                Console.ReadLine();
                //Console.Clear();
                // Clear the scrollback
                Console.WriteLine("\x1b[3J");
            }
        }



    }
}
