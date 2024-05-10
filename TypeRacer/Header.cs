namespace TypeRacer;
internal static class Header
{
    // TODO: this is now tightly coupled with the RaceType enum. Think of a
    // better way to do this.
    private static readonly string[] _modes = ["Words", "Quotes", "C#",
                                               "Python", "React"];

    public static void Print(RaceType raceType)
    {
        int index = (int)raceType;
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
