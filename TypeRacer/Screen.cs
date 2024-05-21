namespace TypeRacer;
internal static class Screen
{
    public const int MinHeight = Header.Height + Keyboard.Height + RaceState.Height + 4; // 4 for footer
    public const int MinWidth = Keyboard.Width;
    public static void Print(Keyboard keyboard, RaceState raceState, RaceMode raceMode)
    {
        Console.Clear();

        if (Console.WindowHeight < MinHeight || Console.WindowWidth < MinWidth)
        {
            Error();
            return;
        }

        // Header
        Header.Print(raceMode, raceState.Type);

        // Keyboard
        keyboard.Print();

        // Text to race
        raceState.Print();

        // Footer
        // TODO: This should be dynamic positioning below the race text, one line gap?
        Console.SetCursorPosition(0, Console.WindowHeight - 3);
        Console.Write($"'Ctrl + Q' to quit");
        Console.SetCursorPosition(0, Console.WindowHeight - 2);
        Console.Write("'Ctrl + R' to restart | 'Ctrl + L' to refresh");
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write($"'Ctrl + [1-{RaceModes.Modes.Count}]' to change mode or type of selected mode");

        raceState.SetCursorPosition();
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

    public static void ResetCursor()
    {
        Console.CursorVisible = true;
#pragma warning disable CA1416 // Validate platform compatibility
        Console.CursorSize = 100;
#pragma warning restore CA1416 // Validate platform compatibility
    }

    private static void Error()
    {
        Console.WriteLine($"Terminal must be at least {MinHeight} rows, {MinWidth} cols.");
        Console.WriteLine("Press `Ctrl + L` to refresh.");
    }

    public static bool ConfirmModal(string message)
    {
        HideCursor();

        (string[] lines, int maxLength) = MessageToLines(message);
        int modalWidth = maxLength + 2 + 2; // 2 for padding, 2 for border
        int modalHeight = lines.Length + 3 + 2 + 1; // 3 for padding, 2 for border, 1 for Y/N

        if (modalHeight > Console.WindowHeight - 4)
        {
            throw new InvalidOperationException("Message too long for modal.");
        }

        int left = (Console.WindowWidth - modalWidth) / 2;
        int top = (Console.WindowHeight - modalHeight) / 2;

        ModalBorder(left, top, modalWidth, modalHeight);

        for (int i = 0; i < lines.Length; i++)
        {
            Console.SetCursorPosition(left + 2, top + 2 + i);
            Console.Write(lines[i]);
        }

        Console.SetCursorPosition(left + modalWidth - 13 - 2, top + modalHeight - 3);
        Console.Write("[Y]es / [N]o"); // Length 13 + 2 padding

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

    public static void MessageModal(string message)
    {
        (string[] lines, _) = MessageToLines(message);
        MessageModal(lines);
    }

    public static void MessageModal(string[] messageLines)
    {
        HideCursor();

        int maxLength = messageLines.Max(line => line.Length);
        if (maxLength > Console.WindowWidth - 10)
        {
            throw new InvalidOperationException("Message too long for modal.");
        }

        int modalWidth = maxLength + 2 + 2; // 2 for padding, 2 for border
        int modalHeight = messageLines.Length + 3 + 2; // 3 for padding, 2 for border

        if (modalHeight > Console.WindowHeight - 4)
        {
            throw new InvalidOperationException("Message too long for modal.");
        }

        int left = (Console.WindowWidth - modalWidth) / 2;
        int top = (Console.WindowHeight - modalHeight) / 2;

        ModalBorder(left, top, modalWidth, modalHeight);

        for (int i = 0; i < messageLines.Length; i++)
        {
            Console.SetCursorPosition(left + 2, top + 2 + i);
            Console.Write(messageLines[i]);
        }


        Console.SetCursorPosition(left + modalWidth - 4 - 2, top + modalHeight - 3);
        Console.Write("[O]k"); // Length 4 + 2 padding

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.O) break;
        }
    }

    public static void ModalBorder(int left, int top, int width, int height)
    {
        Console.SetCursorPosition(left, top);
        Console.Write("╔" + new string('═', width - 2) + "╗");

        for (int i = 1; i < height - 1; i++)
        {
            Console.SetCursorPosition(left, top + i);
            Console.Write("║" + new string(' ', width - 2) + "║");
        }

        Console.SetCursorPosition(left, top + height - 1);
        Console.Write("╚" + new string('═', width - 2) + "╝");
    }

    private static (string[] Lines, int MaxLength) MessageToLines(string message)
    {
        // TODO: Is Console.WindowWidth - 10 a good number?
        if (message.Length <= Console.WindowWidth - 10)
        {
            return ([message], message.Length);
        }

        List<string> lines = [];
        int maxLength = 0;

        while (message.Length > Console.WindowWidth - 10)
        {
            int index = message.LastIndexOf(' ', Console.WindowWidth - 10);
            string line = message[..index];
            lines.Add(line);
            if (line.Length > maxLength)
            {
                maxLength = line.Length;
            }
            message = message[(index + 1)..];
        }

        return ([.. lines], maxLength);
    }
}
