using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Helpers
{
    static class OutputStyler
    {
        #region WriteLine
        public static void WriteLineWarning(string format, params string[] args)
        {
            WriteLineInternal(ConsoleColor.Yellow, format, args);
        }

        public static void WriteLineError(string format, params string[] args)
        {
            WriteLineInternal(ConsoleColor.Red, format, args);
        }

        public static void WriteLineSuccess(string format, params string[] args)
        {
            WriteLineInternal(ConsoleColor.Green, format, args);
        }

        public static void WriteLineHighlight(string format, params string[] args)
        {
            WriteLineInternal(ConsoleColor.Cyan, format, args);
        }
        #endregion
        #region Write
        public static void WriteWarning(string format, params string[] args)
        {
            WriteInternal(ConsoleColor.Yellow, format, args);
        }

        public static void WriteError(string format, params string[] args)
        {
            WriteInternal(ConsoleColor.Red, format, args);
        }

        public static void WriteSuccess(string format, params string[] args)
        {
            WriteInternal(ConsoleColor.Green, format, args);
        }

        public static void WriteHighlight(string format, params string[] args)
        {
            WriteInternal(ConsoleColor.Cyan, format, args);
        }
        #endregion

        private static void WriteLineInternal(ConsoleColor color, string format, params string[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        private static void WriteInternal(ConsoleColor color, string format, params string[] args)
        {
            Console.ForegroundColor = color;
            Console.Write(format, args);
            Console.ResetColor();
        }
    }
}
