namespace TypeRacer;
internal static class RaceKinds
{
    public static RaceKind SwitchRaceKind(this RaceKind kind, RaceType type)
    {
        if (type == RaceType.EnWords)
        {
            return kind switch
            {
                RaceKind.Completion => RaceKind.Accuracy,
                RaceKind.Accuracy => RaceKind.TimeTrial,
                RaceKind.TimeTrial => RaceKind.Completion,
                _ => throw new NotImplementedException(),
            };
        }
        return kind == RaceKind.Completion ? RaceKind.Accuracy : RaceKind.Completion;
    }

    public static (RaceKind kind, string name)[] GetKinds(RaceType type)
    {
        if (type == RaceType.EnWords) return [
            (RaceKind.Completion, nameof(RaceKind.Completion)),
            (RaceKind.Accuracy, nameof(RaceKind.Accuracy)),
            (RaceKind.TimeTrial, "Time trial")
        ];
        return [
            (RaceKind.Completion, nameof(RaceKind.Completion)),
            (RaceKind.Accuracy, nameof(RaceKind.Accuracy))
        ];
    }
}

internal enum RaceKind
{
    Completion,
    Accuracy,
    TimeTrial
}
