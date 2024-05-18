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
//      - [x] Should there be a max width for the text? 80?
//      - [x] There should be an `error` screen if console is too small.
// - [x] Results screen at the end of a race.
// - [x] Finishing the race should not end the program, only Ctrl + Q should.
// - [ ] Add a local DB to track scores.
// - [ ] Add `Modes`, time trial, word count, accuracy, etc.
//       - I'm not sure if what to call these though.
//       - When switching between `RaceType`s, if the type is already selected,
//         it will change the mode.
//       - Words will have a time trial, completion and accuracy modes. Quotes
//         and programming languages will have completion and accuracy modes.
// - [ ] Refactor the code to be more readable and see if I can seperate
//       `RaceState` (or others) into smaller parts.
// - [ ] Figure out what the final product looks like.
// - [x] Add 'Are you sure' confirm window, e.g. to quit.
using System.Text;
using TypeRacer;

Console.Title = "TypeRacer";
Console.OutputEncoding = Encoding.UTF8;

Keyboard keyboard = new(3);

RaceType raceType = RaceType.EnWords;
RaceFileHandler raceFileHandler = new();
string[] text = raceFileHandler.GetTextFromRaceFile(raceType);
RaceState race = new(text, raceType, 10);

Screen.Print(keyboard, race, raceType);

int width = Console.WindowWidth;
int height = Console.WindowHeight;
while (true)
{
    ConsoleKeyInfo pressed = Console.ReadKey(true);

    if (pressed.Key == ConsoleKey.Q && pressed.Modifiers == ConsoleModifiers.Control)
    {
        if (Screen.ConfirmModal("Are you sure you want to quit?"))
        {
            Console.Clear();
            break;
        }
        else
        {
            Screen.ShowCursor();
            Screen.Print(keyboard, race, raceType);
        }
    }
    else if (pressed.Key == ConsoleKey.R && pressed.Modifiers == ConsoleModifiers.Control)
    {
        race.UpdateText(raceFileHandler.GetTextFromRaceFile(raceType), raceType);
        Screen.Print(keyboard, race, raceType);
    }
    else if (pressed.Key == ConsoleKey.L && pressed.Modifiers == ConsoleModifiers.Control)
    {
        width = Console.WindowWidth;
        height = Console.WindowHeight;
        Screen.Print(keyboard, race, raceType);
    }
    else if (
        pressed.Modifiers == ConsoleModifiers.Control &&
        RaceModes.Modes.TryGetValue(pressed.Key, out RaceMode? mode)
        )
    {
        raceType = mode.Type;
        race.UpdateText(raceFileHandler.GetTextFromRaceFile(raceType), raceType);
        Screen.Print(keyboard, race, raceType);
    }
    else if (keyboard.RegisterPress(pressed.Key, pressed.Modifiers == ConsoleModifiers.Shift))
    {
        if (Console.WindowHeight != height || Console.WindowWidth != width)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth;
            Screen.Print(keyboard, race, raceType);
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
            Screen.ShowCursor();
            race.AddChar(pressed.KeyChar);

            if (race.Status == RaceStatus.Finished)
            {
                race.Results();
                Screen.HideCursor();
            }
        }
    }
}
