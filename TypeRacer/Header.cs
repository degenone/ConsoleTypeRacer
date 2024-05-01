namespace TypeRacer;
internal static class Header
{
    private static readonly string[] _modes = ["Words", "Quotes", "C#", "Python", "React"];

    public static void Print(int index)
    {
        if (index < 0 || index >= _modes.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Modes:");
        for (int i = 0; i < _modes.Length; i++)
        {
            if (i == index)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.Write($"{i + 1}.");
            Console.ResetColor();
            Console.Write($" {_modes[i]} ");
        }
    }
}
