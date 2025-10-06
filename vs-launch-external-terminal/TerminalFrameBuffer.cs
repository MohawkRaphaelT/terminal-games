using System.Numerics;
using System.Text;

namespace MohawkTerminalGame;

/// <summary>
///     
/// </summary>
public sealed class TerminalFrameBuffer
{
    private readonly string[,] BackingArray;
    private readonly StringBuilder stringBuilder = new();

    public string this[int x, int y]
    {
        get => BackingArray[x, y];
        set => BackingArray[x, y] = value;
    }

    public int Width { get; init; }
    public int Height { get; init; }
    public string DefaultChar { get; set; }


    public TerminalFrameBuffer(string defaultChar = " ") : this(Console.WindowWidth, Console.WindowHeight, defaultChar)
    {
    }
    public TerminalFrameBuffer(int widthInChars, int heightInChars, string defaultChar = " ")
    {
        Width = widthInChars;
        Height = heightInChars;
        BackingArray = new string[widthInChars, heightInChars];
        DefaultChar = defaultChar;
        SetCharAll(defaultChar);
    }

    public void Reset()
    {
        SetCharAll(DefaultChar);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void SetCharAll(string value)
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
    public void SetCharRow(string value, int row)
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
    public void SetCharCol(string value, int col)
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
    public void SetRectangle(string value, Rectangle area)
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

    public void SetRectangle(string value, int x, int y, int w, int h) => SetRectangle(value, new Rectangle(x, y, w, h));

    public void SetCircle(string value, int cx, int cy, float r)
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

        TerminalFrameBuffer copy = new(area.w, area.h);
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

    public void Write()
    {
        string display = this.ToString();
        Console.Write(display);
    }

    public void Overwrite()
    {
        Console.SetCursorPosition(0, 0);
        Write();
    }

    public void ClearWrite()
    {
        Console.Clear();
        Write();
    }
}
