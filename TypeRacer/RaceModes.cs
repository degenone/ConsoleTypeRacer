using System.Collections.Immutable;

namespace TypeRacer;
internal static class RaceModes
{
    public static readonly ImmutableDictionary<ConsoleKey, RaceModeRecord> Modes = ImmutableDictionary.CreateRange(
        new KeyValuePair<ConsoleKey, RaceModeRecord>[] {
            KeyValuePair.Create<ConsoleKey, RaceModeRecord>(ConsoleKey.D1, new("Words", RaceMode.EnWords)),
            KeyValuePair.Create<ConsoleKey, RaceModeRecord>(ConsoleKey.D2, new("Quotes", RaceMode.Quotes)),
            KeyValuePair.Create<ConsoleKey, RaceModeRecord>(ConsoleKey.D3, new("C#", RaceMode.Csharp)),
            KeyValuePair.Create<ConsoleKey, RaceModeRecord>(ConsoleKey.D4, new("Python", RaceMode.Python)),
            KeyValuePair.Create<ConsoleKey, RaceModeRecord>(ConsoleKey.D5, new("React", RaceMode.React))
        });
}

internal record RaceModeRecord(string Name, RaceMode Mode);

internal enum RaceMode
{
    EnWords,
    Quotes,
    Csharp,
    Python,
    React
}
