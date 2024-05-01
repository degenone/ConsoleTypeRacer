using System.Text;
using TypeRacer;

Console.Title = "TypeRacer";
Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

Keyboard keyboard = new(3);

string[] text = ["Hello, World!", "The lazy dog jumps over the quick brown fox.", "The quick brown fox jumps over the lazy dog.", "Hello, World! The lazy dog jumps over the quick brown fox. The quick brown fox jumps over the lazy dog. Hello, World! The lazy dog jumps over the quick brown fox. The quick brown fox jumps over the lazy dog."];
RaceState race = new(text, 9);

PrintScreen(keyboard, race);

int width = Console.WindowWidth;
int height = Console.WindowHeight;
while (true)
{
    ConsoleKeyInfo pressed = Console.ReadKey(true);

    if (pressed.Key == ConsoleKey.Q && pressed.Modifiers == ConsoleModifiers.Control || race.IsFinished)
    {
        break;
    }
    else if (pressed.Key != ConsoleKey.Tab)
    {
        if (Console.WindowHeight != height || Console.WindowWidth != width)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth;
            PrintScreen(keyboard, race);
        }
        else
        {
            race.AddChar(pressed.KeyChar);
        }
    }

    keyboard.AddKey(pressed.Key, pressed.Modifiers == ConsoleModifiers.Shift);
}

Console.Clear();

static void PrintScreen(Keyboard keyboard, RaceState race, int modeIndex = 0)
{
    Console.Clear();

    // Header
    Header.Print(modeIndex);

    // Keyboard
    keyboard.Print();

    // Text to race
    race.Print();

    // Footer
    // TODO: This should be dynamic positioning below the race text, one line gap
    Console.SetCursorPosition(0, Console.WindowHeight - 2);
    Console.Write("Press 'Ctrl + Q' to quit");
}
