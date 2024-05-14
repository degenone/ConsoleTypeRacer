namespace TypeRacer;
internal static class Screen
{
    public static void Print(Keyboard keyboard, RaceState race, RaceType raceType)
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
}
