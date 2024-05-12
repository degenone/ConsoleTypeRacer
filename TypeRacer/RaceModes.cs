using System.Collections.Immutable;

namespace TypeRacer;
internal static class RaceModes
{
    public static readonly ImmutableDictionary<ConsoleKey, RaceMode> Modes = ImmutableDictionary.CreateRange(
        new KeyValuePair<ConsoleKey, RaceMode>[] {
            KeyValuePair.Create<ConsoleKey, RaceMode>(ConsoleKey.D1, new("Words", RaceType.EnWords)),
            KeyValuePair.Create<ConsoleKey, RaceMode>(ConsoleKey.D2, new("Quotes", RaceType.Quotes)),
            KeyValuePair.Create<ConsoleKey, RaceMode>(ConsoleKey.D3, new("C#", RaceType.Csharp)),
            KeyValuePair.Create<ConsoleKey, RaceMode>(ConsoleKey.D4, new("Python", RaceType.Python)),
            KeyValuePair.Create<ConsoleKey, RaceMode>(ConsoleKey.D5, new("React", RaceType.React))
        });
}

internal record RaceMode(string Name, RaceType Type);

internal enum RaceType
{
    EnWords,
    Quotes,
    Csharp,
    Python,
    React
}
