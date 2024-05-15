namespace TypeRacer;
internal static class Header
{
    public const int Height = 3; // 2 rows for modes + 1 spacing
    public static void Print(RaceType raceType)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Modes:");
        foreach (var kvp in RaceModes.Modes)
        {
            if (raceType == kvp.Value.Type)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.Write((int)kvp.Key - 48);
            Console.ResetColor();
            Console.Write($" {kvp.Value.Name} ");
        }
    }
}
