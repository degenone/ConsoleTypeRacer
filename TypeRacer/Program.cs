// Major TODOs:
// - [x] Add 5-10 files for each game mode (quotes / programming languages).
//      - [x] Quotes
//      - [ ] Consider if it would be better to have them in actual c# files.
// - [x] Add scrolling to race text.
// - [x] Add a timer (I don't think I want to show it, just for the final
// score).
// - [x] Think about the layout of the screen. All centered? Left aligned?
//       For now it will be easier to left align. For future, there is
//       `Centered` property in Keyboard.
// - [x] Results screen at the end of a race.
// - [ ] Finishing the race should not end the program, only Ctrl + Q should.
// - [ ] Add a local DB to track scores.
using System.Text;
using TypeRacer;

Console.Title = "TypeRacer";
Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

Keyboard keyboard = new(3);

RaceType raceType = RaceType.EnWords;
RaceFileHandler raceFileHandler = new();
string[] text = raceFileHandler.GetTextFromRaceFile(raceType);
RaceState race = new(text, 10);

PrintScreen(keyboard, race, raceType);

int width = Console.WindowWidth;
int height = Console.WindowHeight;
while (true)
{
    ConsoleKeyInfo pressed = Console.ReadKey(true);

    if (
        pressed.Key == ConsoleKey.Q &&
        pressed.Modifiers == ConsoleModifiers.Control ||
        race.State == RacerState.Finished
        )
    {
        Console.Clear();
        race.Results();
        break;
    }
    else if (
        pressed.Modifiers == ConsoleModifiers.Control &&
        RaceModes.Modes.TryGetValue(pressed.Key, out RaceMode? mode)
        )
    {
        raceType = mode.Type;
        race.Reset();
        race.UpdateText(raceFileHandler.GetTextFromRaceFile(raceType));
        PrintScreen(keyboard, race, raceType);
    }
    else if (keyboard.RegisterPress(pressed.Key, pressed.Modifiers == ConsoleModifiers.Shift))
    {
        if (Console.WindowHeight != height || Console.WindowWidth != width)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth;
            PrintScreen(keyboard, race, raceType);
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

static void PrintScreen(Keyboard keyboard, RaceState race, RaceType raceType)
{
    Console.Clear();

    // Header
    Header.Print(raceType);

    // Keyboard
    keyboard.Print();

    // Text to race
    race.Print();

    // Footer
    // TODO: This should be dynamic positioning below the race text, one line gap
    // TODO: Ctrl + Q and Ctrl + W are maybe too close together for accidental quits.
    Console.SetCursorPosition(0, Console.WindowHeight - 1);
    Console.Write($"Press 'Ctrl + Q' to quit | 'Ctrl + [1-{RaceModes.Modes.Count}]' to change mode");
}
