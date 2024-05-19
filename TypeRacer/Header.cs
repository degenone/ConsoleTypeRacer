namespace TypeRacer;
internal static class Header
{
    public const int Height = 3; // 2 rows for modes + 1 spacing
    public static void Print(RaceMode raceMode, RaceType raceType)
    {
        Console.SetCursorPosition(0, 0);
        Console.Write($"Types:");
        foreach (var (type, name) in RaceTypes.GetTypes(raceMode))
        {
            if (raceType == type)
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
            if (raceMode == kvp.Value.Mode)
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
