using System.Dynamic;
using System.Numerics;
using System.Text;

namespace MohawkTerminalGame;

/// <summary>
///     A grid of strings for easy addressing and writing.
/// </summary>
public class TerminalGridBase<T>
{
    protected readonly T[,] BackingArray;
    protected readonly StringBuilder stringBuilder = new();

    //public string this[int x, int y]
    //{
    //    get => BackingArray[x, y];
    //    set => BackingArray[x, y] = value;
    //}

    public int Width { get; init; }
    public int Height { get; init; }
    public T DefaultValue { get; set; }


    public TerminalGridBase(T defaultValue) : this(Terminal.WindowWidth, Terminal.WindowHeight, defaultValue)
    {
    }
    public TerminalGridBase(int widthInChars, int heightInChars, T defaultValue)
    {
        Width = widthInChars;
        Height = heightInChars;
        BackingArray = new T[widthInChars, heightInChars];
        DefaultValue = defaultValue;
        SetAll(defaultValue);
    }

    public void Reset()
    {
        SetAll(DefaultValue);
    }

    /// <summary>
    ///     Non-permanent write of <paramref name="value"/> into coordinate
    ///     (<paramref name="x"/>, <paramref name="y"/>).
    /// </summary>
    /// <param name="x">The x coordinate (column, assumes single character width).</param>
    /// <param name="y">The y coordinate (row).</param>
    /// <param name="value">The value to poke.</param>
    public virtual void Poke(int x, int y, T value)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(value);
    }

    public T Get(int x, int y)
    {
        T value = BackingArray[x, y];
        return value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void SetAll(T value)
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
    public void SetRow(T value, int row)
    {
        // Ignore malformed request
        if (row < 0 || row >= Height)
            return;

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
    public void SetCol(T value, int col)
    {
        // Ignore malformed request
        if (col < 0 || col >= Width)
            return;

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
    public void SetRectangle(T value, Rectangle area)
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

    public void SetRectangle(T value, int x, int y, int w, int h)
        => SetRectangle(value, new Rectangle(x, y, w, h));

    public void SetCircle(T value, int cx, int cy, float r)
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

    public static TerminalGridBase<T> CopyArea(TerminalGridBase<T> src, Rectangle area)
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

        TerminalGridBase<T> copy = new(area.w, area.h, src.DefaultValue);
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
    public static void CopyFromTo(TerminalGridBase<T> src, Rectangle srcRect, TerminalGridBase<T> dest, int destX, int destY)
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
        TerminalGridBase<T> srcAreaCopy = CopyArea(src, srcRect);
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

    public virtual void Write()
    {
        string display = this.ToString();
        Terminal.Write(display);
    }

    public void Overwrite(int x = 0, int y = 0)
    {
        Terminal.SetCursorPosition(x, y);
        Write();
    }

    public void ClearWrite()
    {
        Terminal.Clear();
        Write();
    }
}
