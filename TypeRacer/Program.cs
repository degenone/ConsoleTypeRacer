using System.Text;
using TypeRacer;

Console.Title = "TypeRacer";
Console.OutputEncoding = Encoding.UTF8;
Console.Clear();
Console.WriteLine("Hello, World!");

int rowOffset = 3;
int center = Console.WindowWidth / 2;
Keyboard.Print(center, rowOffset);

int height, width;
int cursor = 2;
bool running = true;
(ConsoleKey k, bool mod) lastPressed = default;
while (running)
{
    height = Console.WindowHeight;
    width = Console.WindowWidth;
    Console.SetCursorPosition(width / 2, height / 2);
    Console.Write($"{width}x{height}");
    Console.SetCursorPosition(0, height - 2);
    Console.Write("Press 'q' to quit");
    Console.SetCursorPosition(0, height - 1);
    Console.Write("> ");
    Console.SetCursorPosition(cursor, height - 1);
    ConsoleKeyInfo pressed = Console.ReadKey(true);

    if (pressed.Key == ConsoleKey.Q && pressed.Modifiers == ConsoleModifiers.Control)
    {
        running = false;
    }
    if (pressed.Key != ConsoleKey.Tab)
    {
        cursor++;
        Console.Write(pressed.KeyChar);
    }
    if (lastPressed != default)
    {
        Keyboard.Unhighlight(lastPressed.k, lastPressed.mod, center, rowOffset);
    }
    Keyboard.Highlight(pressed.Key, pressed.Modifiers == ConsoleModifiers.Shift, center, rowOffset);
    lastPressed = (pressed.Key, pressed.Modifiers == ConsoleModifiers.Shift);
}

Console.Clear();
