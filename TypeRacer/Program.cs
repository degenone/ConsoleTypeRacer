// Major TODOs:
// - [x] Add 5-10 files for each game mode (quotes / programming languages).
//      - [x] Quotes
//      - [x] Consider if it would be better to have them in actual c# files.
//            - I think for extensibility, it would be better to have them as
//              `.txt` files. Once built, they can be added to the program by
//              placing them in appropriate folders.
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
// - [x] Add `Modes`, time trial, word count, accuracy, etc.
//       - I'm not sure if what to call these though.
//       - When switching between `RaceType`s, if the type is already selected,
//         it will change the mode.
//       - Words will have a time trial, completion and accuracy modes. Quotes
//         and programming languages will have completion and accuracy modes.
// - [ ] Refactor the code to be more readable and see if I can seperate
//       `RaceState` (or others) into smaller parts.
//       - [ ] Check code style too.
// - [ ] Figure out what the final product looks like.
// - [x] Add 'Are you sure' confirm window, e.g. to quit.
// - [ ] Convert this to use a TUI (Text User Interface) library.
//       Choose between:
//       - [ ] Terminal.Gui https://gui-cs.github.io/Terminal.Gui/index.html
//       - [ ] Spectre.Console https://spectreconsole.net/
// BUG: (in a C# file) if line only contains `{` (or`}`), it will count a new
//      line as an error (new line after the character).
// BUG: should not be able to <BS> if no erros and last char is a space.
using System.Text;
using TypeRacer;

Console.Title = "TypeRacer";
Console.OutputEncoding = Encoding.UTF8;

Keyboard keyboard = new(3);

RaceMode raceMode = RaceMode.EnWords;
RaceFileHandler raceFileHandler = new();
string[] text = raceFileHandler.GetTextFromRaceFile(raceMode);
RaceState raceState = new(text, raceMode, 10);

Screen.HideCursor();
Screen.Print(keyboard, raceState, raceMode);

int width = Console.WindowWidth;
int height = Console.WindowHeight;
while (true)
{
    ConsoleKeyInfo pressed = Console.ReadKey(true);

    if (pressed.Key == ConsoleKey.Q
        && pressed.Modifiers == ConsoleModifiers.Control)
    {
        if (Screen.ConfirmModal("Are you sure you want to quit?"))
        {
            Console.Clear();
            Screen.ResetCursor();
            break;
        }
        else
        {
            Screen.Print(keyboard, raceState, raceMode);
        }
    }
    else if (pressed.Key == ConsoleKey.R
             && pressed.Modifiers == ConsoleModifiers.Control)
    {
        raceState.UpdateText(raceFileHandler.GetTextFromRaceFile(raceMode),
                             raceMode);
        Screen.Print(keyboard, raceState, raceMode);
        Screen.HideCursor();
    }
    else if (pressed.Key == ConsoleKey.L
             && pressed.Modifiers == ConsoleModifiers.Control)
    {
        width = Console.WindowWidth;
        height = Console.WindowHeight;
        Screen.Print(keyboard, raceState, raceMode);
    }
    else if (pressed.Modifiers == ConsoleModifiers.Control
             && RaceModes.Modes.TryGetValue(pressed.Key,
                                            out RaceModeRecord? newRaceMode))
    {
        if (raceMode == newRaceMode.Mode)
        {
            RaceType kind = raceState.Type.SwitchRaceType(raceMode);
            raceState.UpdateRaceType(kind);
            Header.Print(raceMode, kind);
        }
        else
        {
            raceMode = newRaceMode.Mode;
            raceState.UpdateRaceType(RaceType.Completion);
            raceState.UpdateText(raceFileHandler.GetTextFromRaceFile(raceMode),
                                 raceMode);
            Screen.Print(keyboard, raceState, raceMode);
        }
    }
    else if (keyboard.RegisterPress(pressed.Key,
                                    pressed.Modifiers == ConsoleModifiers.Shift))
    {
        if (Console.WindowHeight != height || Console.WindowWidth != width)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth;
            Screen.Print(keyboard, raceState, raceMode);
        }

        if (pressed.Key == ConsoleKey.Backspace)
        {
            if (pressed.Modifiers == ConsoleModifiers.Control)
                raceState.RemoveWord();
            else raceState.RemoveChar();
        }
        else if (pressed.Key == ConsoleKey.W
                 && pressed.Modifiers == ConsoleModifiers.Control)
        {
            raceState.RemoveWord();
        }
        else
        {
            raceState.AddChar(pressed.KeyChar);

            if (raceState.Status == RaceStatus.Finished)
            {
                Screen.HideCursor();
                Screen.MessageModal(raceState.Results());
                raceState.UpdateText(
                    raceFileHandler.GetTextFromRaceFile(raceMode), raceMode);
                Screen.Print(keyboard, raceState, raceMode);
            }
        }
    }
}
