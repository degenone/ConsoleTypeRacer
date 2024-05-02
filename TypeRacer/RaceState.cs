namespace TypeRacer;
internal class RaceState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RaceState"/> class.
    /// Text should come from a file or other source, and should be split into lines.
    /// If a line is too long to fit the console window, it will be truncated.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="rowOffset"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public RaceState(string[] text, int rowOffset)
    {
        if (text.Length == 0)
        {
            throw new ArgumentException("Text must contain at least one line.");
        }
        if (rowOffset < 0 || rowOffset >= Console.WindowHeight)
        {
            throw new ArgumentOutOfRangeException(nameof(rowOffset), "Offset must be within the bounds of the console window.");
        }

        _text = text;
        _rowOffset = rowOffset;
        TextToWindowWidthLines();
    }

    private string[] _text;
    private readonly List<string> _lines = [];
    private readonly List<char> _typed = [];
    private List<int> _errorsAt = [];
    private readonly int _rowOffset;
    private int _totalChars = 0;
    private int _currentPosition = 0;
    private int _currentLine = 0;

    public bool IsFinished { get; private set; } = false;

    public void Print(bool resised = false)
    {
        if (resised)
        {
            TextToWindowWidthLines();
        }

        Console.SetCursorPosition(0, _rowOffset);
        foreach (string line in _lines)
        {
            // Highlight typed words

            Console.WriteLine(line);
        }
    }

    public void UpdateText(string[] text)
    {
        _text = text;
        TextToWindowWidthLines();
    }

    public void AddChar(char ch)
    {
        // TODO: - Handle Tab
        _typed.Add(ch);
        Highlight();

        _currentPosition++;
        if (_currentPosition >= _lines[_currentLine].Length)
        {
            _currentPosition = 0;
            _currentLine++;
        }

        if (_totalChars == _typed.Count) IsFinished = true;
    }

    public void RemoveChar()
    {
        if (_typed.Count == 0) return;
        if (_errorsAt.Count == 0 && _typed[^1] == ' ') return;

        _typed.RemoveAt(_typed.Count - 1);
        if (_errorsAt.Count > 0 && _errorsAt[^1] == _typed.Count)
        {
            _errorsAt.RemoveAt(_errorsAt.Count - 1);
        }

        _currentPosition--;
        if (_currentPosition < 0)
        {
            _currentLine--;
            _currentPosition = _lines[_currentLine].Length - 1;
        }

        Unhighlight();
    }

    public void RemoveWord()
    {
        if (_typed.Count == 0) return;
        if (_typed[^1] == ' ')
        {
            while (_typed.Count > 0 && _errorsAt.Count > 0 && _typed[^1] == ' ')
            {
                RemoveChar();
            }
        }
        else
        {
            while (_typed.Count > 0 && _typed[^1] != ' ')
            {
                RemoveChar();
            }
        }
    }

    private void Highlight()
    {
        Console.SetCursorPosition(_currentPosition, _rowOffset + _currentLine);
        char ch = _lines[_currentLine][_currentPosition];
        bool correct = ch == _typed[^1];
        if (ch == ' ')
        {
            if (!correct) Console.BackgroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = correct ? ConsoleColor.Green : ConsoleColor.Red;
        }
        Console.Write(ch);

        if (!correct)
        {
            _errorsAt.Add(_typed.Count - 1);
        }

        Console.ResetColor();
    }

    private void Unhighlight()
    {
        Console.SetCursorPosition(_currentPosition, _rowOffset + _currentLine);
        Console.ResetColor();
        Console.Write(_lines[_currentLine][_currentPosition]);
    }

    private void TextToWindowWidthLines()
    {
        _lines.Clear();

        for (int i = 0; i < _text.Length; i++)
        {
            string line = _text[i];

            while (line.Length > Console.WindowWidth)
            {
                int lastSpace = line.LastIndexOf(' ', Console.WindowWidth);
                if (lastSpace == -1)
                {
                    throw new InvalidOperationException("Line contains no spaces and is too long to fit window.");
                }
                // Add the line up to and including the last space
                _lines.Add(line[..(lastSpace + 1)]);
                _totalChars += lastSpace + 1;
                line = line[(lastSpace + 1)..];
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                if (i < _text.Length - 1) line += " ";
                _totalChars += line.Length;
                _lines.Add(line);
            }
        }
    }
}
