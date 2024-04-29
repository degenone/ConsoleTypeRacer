using System.Collections.Immutable;

namespace TypeRacer;
public static class Keyboard
{
    public const int Width = 68;

    public static readonly ImmutableDictionary<ConsoleKey, Key> Keys = ImmutableDictionary.CreateRange(
        new KeyValuePair<ConsoleKey, Key>[] {
            // Row 0
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem3, new(['`', '~'], 1, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D1, new(['1', '!'], 5, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D2, new(['2', '@'], 9, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D3, new(['3', '#'], 13, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D4, new(['4', '$'], 17, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D5, new(['5', '%'], 21, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D6, new(['6', '^'], 25, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D7, new(['7', '&'], 29, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D8, new(['8', '*'], 33, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D9, new(['9', '('], 37, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D0, new(['0', ')'], 41, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.OemMinus, new(['-', '_'], 45, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.OemPlus, new(['=', '+'], 49, 0)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Backspace, new(['B', 'a', 'c', 'k', 's', 'p', 'a', 'c', 'e'], 53, 0)),
            // Row 1
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Tab, new(['T', 'a', 'b'], 0, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Q, new(['Q'], 5, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.W, new(['W'], 9, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.E, new(['E'], 13, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.R, new(['R'], 17, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.T, new(['T'], 21, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Y, new(['Y'], 25, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.U, new(['U'], 29, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.I, new(['I'], 33, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.O, new(['O'], 37, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.P, new(['P'], 41, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem4, new(['[', '{'], 45, 1)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem6, new([']', '}'], 49, 1)),
            // Row 2
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.A, new(['A'], 5, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.S, new(['S'], 9, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.D, new(['D'], 13, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.F, new(['F'], 17, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.G, new(['G'], 21, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.H, new(['H'], 25, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.J, new(['J'], 29, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.K, new(['K'], 33, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.L, new(['L'], 37, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem1, new([';', ':'], 41, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem7, new(['\'', '"'], 45, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem5, new(['\\', '|'], 49, 2)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Enter, new(['E', 'n', 't', 'e', 'r'], 53, 2)),
            // Row 3
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem102, new(['\\', '|'], 5, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Z, new(['Z'], 9, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.X, new(['X'], 13, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.C, new(['C'], 17, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.V, new(['V'], 21, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.B, new(['B'], 25, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.N, new(['N'], 29, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.M, new(['M'], 33, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.OemComma, new([',', '<'], 37, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.OemPeriod, new(['.', '>'], 41, 3)),
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Oem2, new(['/', '?'], 45, 3)),
            // Row 4
            KeyValuePair.Create<ConsoleKey, Key>(ConsoleKey.Spacebar, new(['S', 'p', 'a', 'c', 'e'], 21, 4)),
        });

    public static void Print(int center, int offset)
    {
        foreach (var key in Keys.Values)
        {
            int col = center - Width / 2 + key.Column;
            Console.SetCursorPosition(col, key.Row + offset);
            Console.Write(key.Chars);
        }
    }

    public static void Highlight(ConsoleKey pressed, bool shift, int center, int offset)
    {
        if (Keys.TryGetValue(pressed, out Key? key))
        {
            int col = center - Width / 2 + key.Column;
            if (shift && key.Chars.Length == 2) col++;
            Console.SetCursorPosition(col, key.Row + offset);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(KeyToString(key, shift));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public static void Unhighlight(ConsoleKey pressed, bool shift, int center, int offset)
    {
        if (Keys.TryGetValue(pressed, out Key? key))
        {
            int col = center - Width / 2 + key.Column;
            if (shift && key.Chars.Length == 2) col++;
            Console.SetCursorPosition(col, key.Row + offset);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(KeyToString(key, shift));
        }
    }

    private static string KeyToString(Key key, bool shift)
    {
        if (key.Chars.Length == 1) return key.Chars[0].ToString();
        else if (key.Chars.Length == 2) return shift ? key.Chars[1].ToString() : key.Chars[0].ToString();
        else return new string(key.Chars);
    }
}
