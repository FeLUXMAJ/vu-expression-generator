using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ExpressionGenerator.Helpers
{
    /// <summary>
    /// Makes the job of outputting colored text to console easier.
    /// </summary>
    internal class IOHelper
    {
        #region Input
        private const ConsoleColor INPUT_COLOR = ConsoleColor.Magenta;

        public static string ReadLine(string prompt = "", bool toLowerCase = true, bool removeWhiteSpace = true)
        {
            WriteInternal(ConsoleColor.Gray, prompt);
            Console.ForegroundColor = INPUT_COLOR;
            string ret = Console.ReadLine();
            if (toLowerCase)
                ret = ret.ToLower();
            if (removeWhiteSpace)
                ret = Regex.Replace(ret, @"\s+", "");
            Console.ResetColor();
            return ret;
        }

        public static int Read(string prompt = "")
        {
            WriteInternal(ConsoleColor.Gray, prompt);
            Console.ForegroundColor = INPUT_COLOR;
            int ret = Console.Read();
            Console.ResetColor();
            return ret;
        }

        public static ConsoleKeyInfo ReadKey(string prompt = "")
        {
            WriteInternal(ConsoleColor.Gray, prompt);
            Console.ForegroundColor = INPUT_COLOR;
            ConsoleKeyInfo ret = Console.ReadKey();
            Console.ResetColor();
            return ret;
        }

        public static ConsoleKeyInfo ReadKey(bool intercept, string prompt = "")
        {
            WriteInternal(ConsoleColor.Gray, prompt);
            Console.ForegroundColor = INPUT_COLOR;
            ConsoleKeyInfo ret = Console.ReadKey(intercept);
            Console.ResetColor();
            return ret;
        }
        #endregion

        #region Output
        private const char HIGHLIGHT_DELIMETER = '`';

        #region WriteLine
        public static void WriteLineWarning(string format, params object[] args)
        {
            WriteLineInternal(ConsoleColor.Yellow, format, args);
        }

        public static void WriteLineError(string format, params object[] args)
        {
            WriteLineInternal(ConsoleColor.Red, format, args);
        }

        public static void WriteLineSuccess(string format, params object[] args)
        {
            WriteLineInternal(ConsoleColor.Green, format, args);
        }

        public static void WriteLine(string format, params object[] args)
        {
            string formatted = string.Format(format, args);
            string[] parts = formatted.Split(new char[] { HIGHLIGHT_DELIMETER });
            if (parts.Length % 2 != 1)
                throw new FormatException("Invalid message format");
            for (int i = 0; i < parts.Length; i++)
            {
                if (i % 2 == 1)
                    WriteInternal(ConsoleColor.Cyan, parts[i]);
                else
                    Console.Write(parts[i]);
            }
            Console.WriteLine();
        }
        #endregion
        #region Write
        public static void WriteWarning(string format, params object[] args)
        {
            WriteInternal(ConsoleColor.Yellow, format, args);
        }

        public static void WriteError(string format, params object[] args)
        {
            WriteInternal(ConsoleColor.Red, format, args);
        }

        public static void WriteSuccess(string format, params object[] args)
        {
            WriteInternal(ConsoleColor.Green, format, args);
        }

        public static void Write(string format, params object[] args)
        {
            string formatted = string.Format(format, args);
            string[] parts = formatted.Split(new char[] { HIGHLIGHT_DELIMETER });
            if (parts.Length % 2 != 1)
                throw new FormatException("Invalid message format");
            for (int i = 0; i < parts.Length; i++)
            {
                if (i % 2 == 1)
                    WriteInternal(ConsoleColor.Cyan, parts[i]);
                else
                    Console.Write(parts[i]);
            }
        }
        #endregion

        private static void WriteLineInternal(ConsoleColor color, string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        private static void WriteInternal(ConsoleColor color, string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.Write(format, args);
            Console.ResetColor();
        }

        #region RedirectOutput
        private static TextWriter _consoleOutput = Console.Out;
        private static FileStream _fileStream = null;

        public static void RedirectOutputToFile(string fileName, FileMode fileMode)
        {
            if (fileName == string.Empty)
                return;
            _fileStream = new FileStream(fileName, fileMode);
            Console.SetOut(new StreamWriter(_fileStream));
        }

        public static void RedirectOutputToConsole()
        {
            if(_fileStream != null)
            {
                _fileStream.Close();
                _fileStream = null;
            }
            Console.SetOut(_consoleOutput);
        }
        #endregion
        #endregion
    }
}
