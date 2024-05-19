namespace TypeRacer;
internal static class RaceTypes
{
    public static RaceType SwitchRaceType(this RaceType type, RaceMode mode)
    {
        if (mode == RaceMode.EnWords)
        {
            return type switch
            {
                RaceType.Completion => RaceType.Accuracy,
                RaceType.Accuracy => RaceType.TimeTrial,
                RaceType.TimeTrial => RaceType.Completion,
                _ => throw new NotImplementedException(),
            };
        }
        return type == RaceType.Completion ? RaceType.Accuracy : RaceType.Completion;
    }

    public static (RaceType type, string name)[] GetTypes(RaceMode mode)
    {
        if (mode == RaceMode.EnWords) return [
            (RaceType.Completion, nameof(RaceType.Completion)),
            (RaceType.Accuracy, nameof(RaceType.Accuracy)),
            (RaceType.TimeTrial, "Time trial")
        ];
        return [
            (RaceType.Completion, nameof(RaceType.Completion)),
            (RaceType.Accuracy, nameof(RaceType.Accuracy))
        ];
    }
}

internal enum RaceType
{
    Completion,
    Accuracy,
    TimeTrial
}
