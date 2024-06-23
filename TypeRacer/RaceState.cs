﻿using System.Diagnostics;

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
    public RaceState(string[] text,
                     RaceMode raceMode,
                     int rowOffset)
    {
        if (text.Length == 0)
        {
            throw new ArgumentException(
                "Text must contain at least one line.");
        }
        if (rowOffset < 0 || rowOffset >= Console.WindowHeight)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rowOffset),
                "Offset must be within the bounds of the console window.");
        }

        _text = text;
        _raceMode = raceMode;
        _rowOffset = rowOffset;
        TextToWindowWidthLines();
    }

    private string[] _text;
    private RaceMode _raceMode;
    private readonly List<string> _lines = [];
    private readonly List<char> _typed = [];
    private readonly List<int> _errorsAt = [];
    private readonly int _rowOffset;
    private int _errorsMade = 0;
    private int _totalChars = 0;
    private int _currentPosition = 0;
    private int _currentLine = 0;
    private const int _lineLength = 90;
    private readonly Stopwatch _raceTimer = new();
    private const char _newLineChar = '⏎';

    public RaceStatus Status { get; private set; } = RaceStatus.NotStarted;
    public RaceType Type { get; private set; } = RaceType.Completion;
    public const int LinesShownCount = 8;
    public const int Height = LinesShownCount + 1; // 1 for the "..." line / spacing

    public void Print(bool resised = false)
    {
        ClearRows();

        if (resised)
        {
            TextToWindowWidthLines();
        }

        PrintRaceLines();
    }

    private void PrintRaceLines()
    {
        if (_currentLine % LinesShownCount != 0) return;

        ClearRows();
        int start = _currentLine / LinesShownCount * LinesShownCount;
        int end = Math.Min(_lines.Count - start, LinesShownCount);
        for (int i = 0; i < end; i++)
        {
            Console.SetCursorPosition(0, _rowOffset + i);
            Console.WriteLine(_lines[start + i]);

            HighlightLine(start + i);
        }

        if (LinesShownCount < _lines.Count - start)
        {
            Console.SetCursorPosition(0, _rowOffset + end);
            Console.WriteLine("...");
        }
    }

    public void UpdateText(string[] text, RaceMode raceType)
    {
        Reset();
        _text = text;
        _raceMode = raceType;
        TextToWindowWidthLines();
    }

    public void UpdateRaceType(RaceType type) => Type = type;

    public void AddChar(char ch)
    {
        if (Status == RaceStatus.Finished) return;
        else if (Status == RaceStatus.NotStarted)
        {
            Status = RaceStatus.InProgress;
            _raceTimer.Start();
            Screen.ShowCursor();
        }

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

        bool correct = _lines[_currentLine][_currentPosition] != _newLineChar ?
            ch == _lines[_currentLine][_currentPosition] :
            ch == '\r';
        if (!correct)
        {
            _errorsAt.Add(_typed.Count - 1);
            _errorsMade++;
        }

        _currentPosition++;
        if (_currentPosition >= _lines[_currentLine].Length)
        {
            _currentPosition = 0;
            _currentLine++;
            PrintRaceLines();
            Console.SetCursorPosition(
                0, _rowOffset + _currentLine % LinesShownCount);
        }

        if (RaceIsFinished())
        {
            Status = RaceStatus.Finished;
            _raceTimer.Stop();
            Screen.HideCursor();
        }
    }

    private bool RaceIsFinished()
    {
        if (_totalChars <= _typed.Count)
            return true;
        else if (Type == RaceType.Accuracy && Accuracy() < 95) // TODO: make this a setting?
            return true;
        else if (Type == RaceType.TimeTrial
                 && _raceTimer.Elapsed.TotalSeconds >= 30) // TODO: make this a setting?
            return true;
        return false;
    }

    public void RemoveChar()
    {
        if (_typed.Count == 0) return;
        if (
            _errorsAt.Count == 0 &&
            (_currentPosition == 0 ||
            _lines[_currentLine][_currentPosition - 1] == ' ')
            )
        {
            Console.SetCursorPosition(_currentPosition,
                _rowOffset + _currentLine % LinesShownCount);
            return;
        }

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

        if (_currentPosition == 0)
        {
            if (_errorsAt.Count == 0)
            {
                Console.SetCursorPosition(
                    _currentPosition,
                    _rowOffset + _currentLine % LinesShownCount);
                return;
            }
            RemoveChar();
        }

        if (_lines[_currentLine][_currentPosition - 1] == ' ')
        {
            while (_typed.Count > 0
                   && _errorsAt.Count > 0
                   && _currentPosition > 0
                   && _lines[_currentLine][_currentPosition - 1] == ' ')
            {
                RemoveChar();
            }
        }
        else
        {
            while (_typed.Count > 0
                   && _currentPosition > 0
                   && _lines[_currentLine][_currentPosition - 1] != ' ')
            {
                RemoveChar();
            }
        }

        Console.SetCursorPosition(_currentPosition,
                                  _rowOffset + _currentLine % LinesShownCount);
    }

    private void HighlightCurrent() => HighlightChar(_currentLine,
                                                     _currentPosition,
                                                     _typed[^1]);

    private void HighlightChar(int line, int pos, char typedCh)
    {
        Console.SetCursorPosition(pos, _rowOffset + line % LinesShownCount);

        char ch = _lines[line][pos];
        // TODO: this might not work for non-windows systems.
        bool correct = ch != _newLineChar ? ch == typedCh : typedCh == '\r';

        if (ch != ' ')
        {
            Console.ForegroundColor = correct ? ConsoleColor.Green : ConsoleColor.Red;
        }
        else if (ch == ' ' && !correct)
        {
            Console.BackgroundColor = ConsoleColor.Red;
        }

        Console.Write(ch);
        Console.ResetColor();
    }

    private void HighlightLine(int line)
    {
        if (_typed.Count == 0 || line > _currentLine) return;

        for (int pos = 0; pos < _lines[line].Length; pos++)
        {
            int idx = _lines.Take(line).Sum(l => l.Length) + pos;
            if (idx >= _typed.Count) break;

            HighlightChar(line, pos, _typed[idx]);
        }
    }

    private void Unhighlight()
    {
        Console.SetCursorPosition(_currentPosition,
                                  _rowOffset + _currentLine % LinesShownCount);
        Console.ResetColor();
        Console.Write(_lines[_currentLine][_currentPosition]);
        Console.SetCursorPosition(_currentPosition,
                                  _rowOffset + _currentLine % LinesShownCount);
    }

    private void TextToWindowWidthLines()
    {
        // TODO: maybe should enforce that there are spaces in the text?
        _lines.Clear();

        int maxLineLength = Math.Min(_lineLength, Console.WindowWidth - 1);
        string lineEnding = _raceMode
                            != RaceMode.EnWords ? _newLineChar.ToString() : " ";

        for (int i = 0; i < _text.Length; i++)
        {
            string line = _text[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            while (line.Length > maxLineLength)
            {
                int lastSpace = line.LastIndexOf(' ', maxLineLength);
                if (lastSpace == -1)
                {
                    throw new InvalidOperationException(
                        "Line contains no spaces and is too long to fit window.");
                }
                // Add the line up to and including the last space
                _lines.Add(line[..(lastSpace + 1)]);
                _totalChars += lastSpace + 1;
                line = line[(lastSpace + 1)..];
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                if (i < _text.Length - 1) line += lineEnding;
                _totalChars += line.Length;
                _lines.Add(line);
            }
        }
    }

    private void Reset()
    {
        ClearRows();
        _typed.Clear();
        _errorsAt.Clear();
        _errorsMade = 0;
        _currentPosition = 0;
        _currentLine = 0;
        _totalChars = 0;
        _raceTimer.Reset();
        Status = RaceStatus.NotStarted;
    }

    private void ClearRows()
    {
        for (int i = 0; i <= LinesShownCount; i++)
        {
            Console.SetCursorPosition(0, _rowOffset + i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }

    public string[] Results() => [
            "Results:",
            $"Errors: {_errorsMade}",
            $"Words per minute: {Wpm()}",
            $"Accuracy: {Accuracy():0.00}%"
        ];

    private int Wpm()
    {
        // TODO: With code, I need to think more about how to measure WPM.
        // TODO: Improve Wpm calculation. Words don't have to be correct but
        //       should be the same length (?) to be counted as a completed
        //       word.
        if (_typed.Count == 0) return 0;

        TimeSpan elapced = _raceTimer.Elapsed;
        string typedText = string.Join("", _typed);

        // TODO: There is a weird case, if you type all the words correctly but
        //       never use space, does this count as 0 WPM?
        // edge case: if only one word is typed and it's wrong.
        if (
            !typedText.Contains(' ') &&
            typedText != _lines[0].Split(' ')[0]
            ) return 0;

        int typedWordCount = typedText.Split(' ').Length;
        // TODO: Still not sure if this is correct. Needs more testing.
        return (int)(typedWordCount / elapced.TotalSeconds * 60);
    }

    private double Accuracy()
    {
        // TODO: is this the correct measure for accuracy?
        return (1 - (double)_errorsMade / _typed.Count) * 100;
    }

    internal void SetCursorPosition() => Console.SetCursorPosition(
            _currentPosition, _rowOffset + _currentLine % LinesShownCount);
}

internal enum RaceStatus
{
    NotStarted,
    InProgress,
    Finished
}
