
using System;

namespace vs_launch_external_terminal;

/// <summary>
///     A utility class built on top of <see cref="Console"/>.
/// </summary>
public static class Terminal
{
    public static bool WriteWithWordBreaks { get; set; } = true;
    public static char WordBreakCharacter { get; set; } = ' ';

    private delegate void WriteAction(string value);
    private delegate void WriteObjAction(object obj);
    private static WriteAction OnWrite => WriteWithWordBreaks ? FancyWrite : Console.Write;
    private static WriteAction OnWriteLine => WriteWithWordBreaks ? FancyWriteLine : Console.WriteLine;
    private static WriteObjAction OnWriteObj => WriteWithWordBreaks ? FancyWrite : Console.Write;
    private static WriteObjAction OnWriteLineObj => WriteWithWordBreaks ? FancyWriteLine : Console.WriteLine;

    private static void FancyWrite(string value)
    {
        string[] elements = value.Split(WordBreakCharacter);
        foreach (string element in elements)
        {
            int cursorPositionX = Console.GetCursorPosition().Left;
            int windowWidth = Console.WindowWidth;
            int remainingLineCharacters = windowWidth - cursorPositionX;
            // If no room, new line
            bool canFit = element.Length < remainingLineCharacters - 1;
            if (!canFit)
                Console.WriteLine();
            // A. Print as usual, or
            // B. falls through and still doesn't fit on its own, print anyways
            Console.Write(element);
            Console.Write(" ");
        }
    }
    private static void FancyWrite(object obj) => FancyWrite(obj.ToString() ?? string.Empty);
    private static void FancyWriteLine(string value)
    {
        FancyWrite(value);
        Console.WriteLine();
    }
    private static void FancyWriteLine(object obj) => FancyWriteLine(obj.ToString() ?? string.Empty);


    private static void Write(Action consoleWrite, ConsoleColor foregroundColor)
    {
        var fgColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        consoleWrite.Invoke();
        Console.ForegroundColor = fgColor;
    }
    private static void Write(Action consoleWrite, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        var fgColor = Console.ForegroundColor;
        var bgColor = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        consoleWrite.Invoke();
        Console.ForegroundColor = fgColor;
        Console.BackgroundColor = bgColor;
    }

    // Write
    public static void Write(string value) => OnWrite(value);
    public static void Write(ulong value) => OnWriteObj(value);
    public static void Write(uint value) => OnWriteObj(value);
    public static void Write(object value) => OnWriteObj(value);
    public static void Write(float value) => OnWriteObj(value);
    public static void Write(char value) => OnWriteObj(value);
    public static void Write(char[] value) => OnWriteObj(value);
    public static void Write(bool value) => OnWriteObj(value);
    public static void Write(double value) => OnWriteObj(value);
    public static void Write(int value) => OnWriteObj(value);
    public static void Write(long value) => OnWriteObj(value);
    public static void Write(decimal value) => OnWriteObj(value);

    // WriteLine
    public static void WriteLine() => Console.WriteLine();
    public static void WriteLine(string value) => OnWriteLine(value);
    public static void WriteLine(ulong value) => OnWriteLineObj(value);
    public static void WriteLine(uint value) => OnWriteLineObj(value);
    public static void WriteLine(object value) => OnWriteLineObj(value);
    public static void WriteLine(float value) => OnWriteLineObj(value);
    public static void WriteLine(char value) => OnWriteLineObj(value);
    public static void WriteLine(char[] value) => OnWriteLineObj(value);
    public static void WriteLine(bool value) => OnWriteLineObj(value);
    public static void WriteLine(double value) => OnWriteLineObj(value);
    public static void WriteLine(int value) => OnWriteLineObj(value);
    public static void WriteLine(long value) => OnWriteLineObj(value);
    public static void WriteLine(decimal value) => OnWriteLineObj(value);

    // Write with console color fg
    public static void Write(string value, ConsoleColor foregroundColor) => Write(() => OnWrite(value), foregroundColor);
    public static void Write(object value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(char[] value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(char value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(bool value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(int value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(long value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(uint value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(ulong value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(float value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(double value, ConsoleColor foregroundColor) => Write(() => OnWriteObj(value), foregroundColor);
    public static void Write(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWrite(value), foregroundColor, backgroundColor);
    public static void Write(object value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(char[] value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(char value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(bool value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(int value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(long value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(uint value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(ulong value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(float value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);
    public static void Write(double value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteObj(value), foregroundColor, backgroundColor);

    // Write with console color fg/bg
    public static void WriteLine(string value, ConsoleColor foregroundColor) => Write(() => OnWriteLine(value), foregroundColor);
    public static void WriteLine(object value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(char[] value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(char value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(bool value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(int value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(long value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(uint value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(ulong value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(float value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(double value, ConsoleColor foregroundColor) => Write(() => OnWriteLineObj(value), foregroundColor);
    public static void WriteLine(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLine(value), foregroundColor, backgroundColor);
    public static void WriteLine(object value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(char[] value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(char value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(bool value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(int value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(long value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(uint value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(ulong value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(float value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);
    public static void WriteLine(double value, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(() => OnWriteLineObj(value), foregroundColor, backgroundColor);

}