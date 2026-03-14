using System;
using System.Collections.Generic;
using System.Text;

namespace MyCodingAgent.Helpers;

public class ConsoleEditor
{
    public static string ReadMultilineInput()
    {
        var lines = new List<StringBuilder> { new StringBuilder() };

        int cursorX = 0;
        int cursorY = 0;

        int startTop = Console.CursorTop;
        int lastRenderLineCount = 0;

        void Render()
        {
            Console.SetCursorPosition(0, startTop);

            int lineIndex = 0;

            foreach (var l in lines)
            {
                Console.WriteLine(l.ToString().PadRight(Console.WindowWidth));
                lineIndex++;
            }

            // oude regels wissen
            for (int i = lineIndex; i < lastRenderLineCount; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }

            // extra lege regel (jouw fix)
            Console.WriteLine(new string(' ', Console.WindowWidth));

            lastRenderLineCount = lines.Count;

            Console.SetCursorPosition(cursorX, startTop + cursorY);
        }

        Render();

        while (true)
        {
            var key = Console.ReadKey(true);

            // CTRL+ENTER = submit
            if (key.Key == ConsoleKey.Enter &&
                key.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                Console.SetCursorPosition(0, startTop + lines.Count + 1);
                Console.WriteLine();
                break;
            }

            // ENTER = newline
            if (key.Key == ConsoleKey.Enter)
            {
                var current = lines[cursorY];
                var newLine = new StringBuilder(current.ToString().Substring(cursorX));

                current.Length = cursorX;

                lines.Insert(cursorY + 1, newLine);

                cursorY++;
                cursorX = 0;

                Render();
                continue;
            }

            // BACKSPACE
            if (key.Key == ConsoleKey.Backspace)
            {
                if (cursorX > 0)
                {
                    lines[cursorY].Remove(cursorX - 1, 1);
                    cursorX--;
                }
                else if (cursorY > 0)
                {
                    cursorX = lines[cursorY - 1].Length;
                    lines[cursorY - 1].Append(lines[cursorY]);
                    lines.RemoveAt(cursorY);
                    cursorY--;
                }

                Render();
                continue;
            }

            // ARROWS
            if (key.Key == ConsoleKey.LeftArrow)
            {
                if (cursorX > 0) cursorX--;
                else if (cursorY > 0)
                {
                    cursorY--;
                    cursorX = lines[cursorY].Length;
                }

                Render();
                continue;
            }

            if (key.Key == ConsoleKey.RightArrow)
            {
                if (cursorX < lines[cursorY].Length) cursorX++;
                else if (cursorY < lines.Count - 1)
                {
                    cursorY++;
                    cursorX = 0;
                }

                Render();
                continue;
            }

            if (key.Key == ConsoleKey.UpArrow)
            {
                if (cursorY > 0)
                {
                    cursorY--;
                    cursorX = Math.Min(cursorX, lines[cursorY].Length);
                }

                Render();
                continue;
            }

            if (key.Key == ConsoleKey.DownArrow)
            {
                if (cursorY < lines.Count - 1)
                {
                    cursorY++;
                    cursorX = Math.Min(cursorX, lines[cursorY].Length);
                }

                Render();
                continue;
            }

            // normale tekst
            if (!char.IsControl(key.KeyChar))
            {
                lines[cursorY].Insert(cursorX, key.KeyChar);
                cursorX++;
                Render();
            }
        }

        return string.Join(Environment.NewLine, lines);
    }
}
