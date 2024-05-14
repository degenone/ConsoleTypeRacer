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
    private readonly Stopwatch _raceTimer = new();

    public RacerState State { get; private set; } = RacerState.NotStarted;
    public int LinesShownCount { get; set; } = 5;

    public void Print(bool resised = false)
    {
        ClearRows();

        if (resised)
        {
            TextToWindowWidthLines();
        }

        if (State == RacerState.Finished) Results();
        else PrintRaceLines();
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

    public void UpdateText(string[] text)
    {
        Reset();
        _text = text;
        TextToWindowWidthLines();
    }

    public void AddChar(char ch)
    {
        if (State == RacerState.Finished) return;
        else if (State == RacerState.NotStarted)
        {
            State = RacerState.InProgress;
            _raceTimer.Start();
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
            PrintRaceLines();
        }

        if (_totalChars <= _typed.Count)
        {
            State = RacerState.Finished;
            _raceTimer.Stop();
        }
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
        // BUG: `pos` or `left` parameter can sometimes be out of range. Not
        //      sure what could cause it? Maybe the line is exaclty the width of the
        //      console window and I add a space, therefor making it out of range?
        //      Making the text have a max width (less than console) would fix this.
        Console.SetCursorPosition(pos, _rowOffset + line % LinesShownCount);
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

        for (int pos = 0; pos < _lines[line].Length; pos++)
        {
            int idx = _lines.Take(line).Sum(l => l.Length) + pos;
            if (idx >= _typed.Count) break;

            HighlightChar(line, pos, _typed[idx]);
        }
    }

    private void Unhighlight()
    {
        Console.SetCursorPosition(_currentPosition, _rowOffset + _currentLine % LinesShownCount);
        Console.ResetColor();
        Console.Write(_lines[_currentLine][_currentPosition]);
        Console.SetCursorPosition(_currentPosition, _rowOffset + _currentLine % LinesShownCount);
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
                // TODO: I want to make this a new line, at least for all code
                // races, but maybe for all types.
                // BUG(maybe): Make sure there is never an empty line at the
                //             end of a race text. Could be source text too.
                if (i < _text.Length - 1) line += " ";
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
        State = RacerState.NotStarted;
    }

    private void ClearRows()
    {
        for (int i = 0; i <= LinesShownCount; i++)
        {
            Console.SetCursorPosition(0, _rowOffset + i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }

    public void Results()
    {
        ClearRows();
        Console.SetCursorPosition(0, _rowOffset);
        Console.WriteLine("Results:");
        Console.WriteLine($"Errors: {_errorsMade}");
        Console.WriteLine($"Words per minute: {Wpm()}");
        Console.WriteLine($"Accuracy: {Accuracy()}%");
    }

    private int Wpm()
    {
        // TODO: This is not the greatest measure of WPM, but it's a start.
        //       With code it's a bit more difficult to measure.
        TimeSpan elapced = _raceTimer.Elapsed;
        return (int)(_text.Select(l => l.Split(' ').Length).Sum() / elapced.TotalMinutes);
    }

    private double Accuracy()
    {
        // TODO: is this the correct measure for accuracy?
        return (1 - (double)_errorsMade / _typed.Count) * 100;
    }
}

internal enum RacerState
{
    NotStarted,
    InProgress,
    Finished
}
