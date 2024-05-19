namespace TypeRacer;
internal static class Header
{
    public const int Height = 3; // 2 rows for modes + 1 spacing
    public static void Print(RaceType raceType, RaceKind raceKind)
    {
        Console.SetCursorPosition(0, 0);
        Console.Write($"Modes:");
        foreach (var (kind, name) in RaceKinds.GetKinds(raceType))
        {
            if (raceKind == kind)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.Write($" {name} ");
            Console.ResetColor();
        }
        Console.SetCursorPosition(0, 1);
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
