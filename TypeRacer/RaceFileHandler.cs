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
            RaceType.Quotes => GetTextFromQuotesFile(),
            RaceType.Csharp => GetTextFromCsharpFile(),
            RaceType.Python => GetTextFromPythonFile(),
            RaceType.React => GetTextFromReactFile(),
            _ => throw new NotImplementedException()
        };
    }

    private string GetRandomFileFromDirectory(string directory) =>
        Directory.GetFiles(Path.Combine(assetsPath, directory))
                 .OrderBy(x => random.Next())
                 .First();

    private string[] GetTextFromQuotesFile()
    {
        // TODO: make this more dynamic.
        string filePath = Path.Combine(assetsPath, "./Quotes/test.txt");
        return File.ReadAllLines(filePath);
    }

    private string[] GetTextFromCsharpFile()
    {
        // select random file from the directory `Csharp`
        return File.ReadAllLines(GetRandomFileFromDirectory("./Csharp/"));
    }

    private string[] GetTextFromPythonFile()
    {
        return File.ReadAllLines(GetRandomFileFromDirectory("./Python/"));
    }

    private string[] GetTextFromReactFile()
    {
        return File.ReadAllLines(GetRandomFileFromDirectory("./React/"));
    }

    private string[] GetTextFromEnWordsFile()
    {
        string filePath = Path.Combine(assetsPath, "./words/en.txt");
        string[] lines = File.ReadAllLines(filePath);
        random.Shuffle(lines);
        return [string.Join(' ', lines[..100])];
    }
}
