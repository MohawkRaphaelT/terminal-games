# Windows Terminal ANSI-compatible commands

Resources

* A useful resource though not all work. https://www2.ccs.neu.edu/research/gpc/VonaUtils/vona/terminal/vtansi.htm
* Further explanation of what attributes would be expected. https://en.wikipedia.org/wiki/ANSI_escape_code#Select_Graphic_Rendition_parameters

Commands I've seen work.

* `"\x1B"` Escape sequence (following chars reinterpreted).
* `"\x1B[#A"` Cursor up, # is count, omit for inferred 1.
* `"\x1B[#B"` Cursor down, # is count, omit for inferred 1.
* `"\x1B[#C"` Cursor forward, # is count, omit for inferred 1.
* `"\x1B[#D"` Cursor backward, # is count, omit for inferred 1.
* `"\x1B[#;#H"` Set cursor position, first # is line/row, second # is column.
* `"\x1B[K"` Erases from the current cursor position to the end of the current line.
* `"\x1B[1K"` Erases from the current cursor position to the start of the current line.
* `"\x1B[2K"` Erases the entire current line.
* `"\x1B[J"` Erases the screen from the current line down to the bottom of the screen.
* `"\x1B[1J"` Erases the screen from the current line up to the top of the screen.
* `"\x1B[2J"` Erases the screen with the background colour and moves the cursor to *home*.
* `"\x1B#m"` where # is a number.
  * 0 reset attribute (`m` is the attribute marker)
  * 1 bright text, 2 dim text
    * NOTE: you can combine dim and bright for a shade lighter than dim, but dimmer than standard
  * 3 italicized text
  * 4 underlined text
  * 5/6 blinking text
  * 7 reverse fg and bg colors (doesn't swap, directly uses the inverse color)
  * 8 hidden - hides text but can be highlighted to show it
  * 9 strikethrough
  * 21 bold underlined text
  * TEXT COLOR
    * 30 text color black
    * 31 text color dark red
    * 32 text color dark green
    * 33 text color dark yellow
    * 34 text color dark blue
    * 35 text color dark magenta (purple)
    * 36 text color dark cyan
    * 37 text color light grey
    * 90 text color dark grey
    * 91 text color red 
    * 92 text color green
    * 93 text color yellow
    * 94 text color blue
    * 95 text color magenta
    * 96 text color cyan
    * 97 text color white
  * TEXT BACKGROUND COLOR
    * 40 text background black
    * 41 text background dark red
    * 42 text background dark green
    * 43 text background dark yellow
    * 44 text background dark blue
    * 45 text background dark magenta (purple)
    * 46 text background dark cyan
    * 47 text background light grey
    * 52 lower underlined text (underline is farther down)
    * 100 text background dark grey
    * 101 text background red
    * 102 text background green
    * 103 text background yellow
    * 104 text background blue
    * 105 text background magenta (purple)
    * 106 text background cyan
    * 107 text background white

```C#
// A fun line of code to see what comes out
for (int i = 0; i < 128; i++)
{
    Console.Write("\x1b[0m");
    Console.WriteLine($"\x1b[{i}mATTRIBUTE {i.ToString().PadLeft(3)} \\x1b[{i}m");
}
Console.ReadLine();
```

