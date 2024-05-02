using System.Text;
using TypeRacer;

Console.Title = "TypeRacer";
Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

Keyboard keyboard = new(3);

string[] text = ["Hello, World!", "The lazy dog jumps over the quick brown fox.", "The quick brown fox jumps over the lazy dog.", "Hello, World! The lazy dog jumps over the quick brown fox. The quick brown fox jumps over the lazy dog. Hello, World! The lazy dog jumps over the quick brown fox. The quick brown fox jumps over the lazy dog."];
RaceState race = new(text, 10);

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
    else if (keyboard.RegisterPress(pressed.Key, pressed.Modifiers == ConsoleModifiers.Shift))
    {
        if (Console.WindowHeight != height || Console.WindowWidth != width)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth;
            PrintScreen(keyboard, race);
        }

        if (pressed.Key == ConsoleKey.Backspace)
        {
            if (pressed.Modifiers == ConsoleModifiers.Control) race.RemoveWord();
            else race.RemoveChar();
        }
        else if (pressed.Key == ConsoleKey.W && pressed.Modifiers == ConsoleModifiers.Control)
        {
            race.RemoveWord();
        }
        else
        {
            race.AddChar(pressed.KeyChar);
        }
    }
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
    // TODO: Ctrl + Q and Ctrl + W are maybe too close together for accidental quits.
    Console.SetCursorPosition(0, Console.WindowHeight - 2);
    Console.Write("Press 'Ctrl + Q' to quit");
}
