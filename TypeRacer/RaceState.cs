﻿namespace TypeRacer;
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
    private readonly List<int> _errorsAt = [];
    private readonly int _rowOffset;
    private int _errorsMade = 0;
    private int _totalChars = 0;
    private int _currentPosition = 0;
    private int _currentLine = 0;

    public bool IsFinished { get; private set; } = false;

    public void Print(bool resised = false)
    {
        ClearRows();

        if (resised)
        {
            TextToWindowWidthLines();
        }

        for (int i = 0; i < _lines.Count; i++)
        {
            Console.SetCursorPosition(0, _rowOffset + i);
            Console.WriteLine(_lines[i]);

            HighlightLine(i);
        }
    }

    public void UpdateText(string[] text)
    {
        _text = text;
        TextToWindowWidthLines();
    }

    public void AddChar(char ch)
    {
        // TODO: If Tab is used incorrectly it can count upto 4 errors when it should be 1.
        if (ch == '\t')
        {
            for (int i = 0; i < 4; i++)
            {
                AddChar(' ');
            }
            return;
        }

        _typed.Add(ch);
        HighlightCurrent();

        if (!(ch == _lines[_currentLine][_currentPosition]))
        {
            _errorsAt.Add(_typed.Count - 1);
            _errorsMade++;
        }

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


    private void HighlightCurrent() => HighlightChar(_currentLine, _currentPosition, _typed[^1]);

    private void HighlightChar(int line, int pos, char typedCh)
    {
        Console.SetCursorPosition(pos, _rowOffset + line);
        char ch = _lines[line][pos];
        bool correct = ch == typedCh;
        if (ch == ' ')
        {
            if (!correct) Console.BackgroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = correct ? ConsoleColor.Green : ConsoleColor.Red;
        }
        Console.Write(ch);
        Console.ResetColor();
    }

    private void HighlightLine(int line)
    {
        if (_typed.Count == 0 || line > _currentLine) return;
        // Highlight typed words

        for (int pos = 0; pos < _lines[line].Length; pos++)
        {
            int idx = _lines.Take(line).Sum(l => l.Length) + pos;
            if (idx >= _typed.Count) break;

            HighlightChar(line, pos, _typed[idx]);
        }
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

    public void Reset()
    {
        // TODO: need to make sure all previous lines are empty.
        ClearRows();
        _typed.Clear();
        _errorsAt.Clear();
        _errorsMade = 0;
        _currentPosition = 0;
        _currentLine = 0;
        // TODO: This should use RacerState
        IsFinished = false;
    }

    private void ClearRows()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            Console.SetCursorPosition(0, _rowOffset + i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }

    public void Results()
    {
        Console.Clear();
        Console.WriteLine("Results:");
        Console.WriteLine($"Errors: {_errorsMade}");
        Console.WriteLine($"Words per minute: {Wpm()}");
        Console.WriteLine($"Accuracy: {Accuracy()}%");
    }

    private object Wpm()
    {
        throw new NotImplementedException();
    }

    private object Accuracy()
    {
        throw new NotImplementedException();
    }
}
