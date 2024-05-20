namespace TypeRacer;
internal class RaceFileHandler
{
    private readonly Random _random = new();

    private static string GetAssetsPath()
    {
        string? processDirName = Path.GetDirectoryName(Environment.ProcessPath);
        if (string.IsNullOrEmpty(processDirName))
            throw new InvalidOperationException(
                "GetAssetsPath: Process directory name is null or empty");
        return Path.GetFullPath(Path.Combine(processDirName, "./assets/"));
    }

    public string[] GetTextFromRaceFile(RaceMode raceType)
    {
        return raceType switch
        {
            RaceMode.EnWords => GetTextFromEnWordsFile(),
            RaceMode.Quotes => GetTextFromRandomFileFromDirectory("./Quotes/"),
            //RaceType.Quotes => DebuggingExample(),
            RaceMode.Csharp => GetTextFromRandomFileFromDirectory("./Csharp/"),
            RaceMode.Python => GetTextFromRandomFileFromDirectory("./Python/"),
            RaceMode.React => GetTextFromRandomFileFromDirectory("./React/"),
            _ => throw new NotImplementedException("GetTextFromRaceFile: Should be unreachable")
        };
    }

    private static string[] DebuggingExample()
    {
        return
        [
            "a line",
            "another line",
            "a third line",
            "a fourth line",
            "a fifth line",
            "a sixth line",
            "a seventh line",
            "an eighth line",
            "a ninth line",
            "a tenth line"
        ];
    }

    private string[] GetTextFromRandomFileFromDirectory(string directory)
    {
        var file = Directory
                            .GetFiles(Path.Combine(GetAssetsPath(), directory))
                            .OrderBy(x => _random.Next())
                            .First();
        return File.ReadAllLines(file);
    }

    private string[] GetTextFromEnWordsFile()
    {
        string filePath = Path.Combine(GetAssetsPath(), "./words/en.txt");
        string[] lines = File.ReadAllLines(filePath);
        _random.Shuffle(lines);
        return [string.Join(' ', lines[..100])];
    }
}
