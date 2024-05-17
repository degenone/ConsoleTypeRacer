namespace TypeRacer;
internal static class Screen
{
    public const int MinHeight = Header.Height + Keyboard.Height + RaceState.Height + 2; // 2 for footer
    public const int MinWidth = Keyboard.Width;
    public static void Print(Keyboard keyboard, RaceState race, RaceType raceType)
    {
        Console.Clear();

        if (Console.WindowHeight < MinHeight || Console.WindowWidth < MinWidth)
        {
            Error();
            return;
        }

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

        Console.SetCursorPosition(0, Header.Height + Keyboard.Height);
    }

    public static void ShowCursor()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            Console.CursorVisible = true;
#pragma warning disable CA1416 // Validate platform compatibility
            Console.CursorSize = 1;
#pragma warning restore CA1416 // Validate platform compatibility
        }
        else
        {
            Console.CursorVisible = false;
        }
    }

    public static void HideCursor()
    {
        Console.CursorVisible = false;
    }

    private static void Error()
    {
        Console.WriteLine($"Terminal must be at least {MinHeight} rows, {MinWidth} cols.");
        Console.WriteLine("Press `Ctrl + L` to refresh.");
    }
    
    public static bool ConfirmModal(string message)
    {
        HideCursor();
        int modalWidth = message.Length + 2 + 2; // 2 for padding, 2 for border
        int modalHeight = 7; // 3 for padding, 2 for border, 1 for message, 1 for Y/N
        int left = (Console.WindowWidth - modalWidth) / 2;
        int top = (Console.WindowHeight - modalHeight) / 2;
        Console.SetCursorPosition(left, top);
        Console.Write("╔" + new string('═', message.Length + 2) + "╗");
        Console.SetCursorPosition(left, top + 1);
        Console.Write("║ " + new string(' ', message.Length) + " ║");
        Console.SetCursorPosition(left, top + 2);
        Console.Write("║ " + message + " ║");
        Console.SetCursorPosition(left, top + 3);
        Console.Write("║ " + new string(' ', message.Length) + " ║");
        Console.SetCursorPosition(left, top + 4);
        Console.Write("║ " + "[Y]es / [N]o".PadLeft(message.Length) + " ║");
        Console.SetCursorPosition(left, top + 5);
        Console.Write("║ " + new string(' ', message.Length) + " ║");
        Console.SetCursorPosition(left, top + 6);
        Console.Write("╚" + new string('═', message.Length + 2) + "╝");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Y)
            {
                return true;
            }
            else if (key.Key == ConsoleKey.N)
            {
                return false;
            }
        }
    }
}
