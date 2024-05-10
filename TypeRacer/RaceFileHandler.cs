namespace TypeRacer;
internal class RaceFileHandler
{
    private readonly string assetsPath = Path.GetFullPath("./assets/");
    private readonly Random random = new();

    public string[] GetTextFromRaceFile(RaceType raceType)
    {
        return raceType switch
        {
            RaceType.EnWords => GetTextFromEnWordsFile(),
            RaceType.Quotes => GetTextFromRandomFileFromDirectory("./Quotes/"),
            RaceType.Csharp => GetTextFromRandomFileFromDirectory("./Csharp/"),
            RaceType.Python => GetTextFromRandomFileFromDirectory("./Python/"),
            RaceType.React => GetTextFromRandomFileFromDirectory("./React/"),
            _ => throw new NotImplementedException("GetTextFromRaceFile: Should be unreachable")
        };
    }

    private string[] GetTextFromRandomFileFromDirectory(string directory)
    {
        var file = Directory.GetFiles(Path.Combine(assetsPath, directory))
                            .OrderBy(x => random.Next())
                            .First();
        return File.ReadAllLines(file);
    }

    private string[] GetTextFromEnWordsFile()
    {
        string filePath = Path.Combine(assetsPath, "./words/en.txt");
        string[] lines = File.ReadAllLines(filePath);
        random.Shuffle(lines);
        return [string.Join(' ', lines[..100])];
    }
}
