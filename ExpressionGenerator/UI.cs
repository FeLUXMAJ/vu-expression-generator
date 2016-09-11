using ExpressionGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionGenerator
{
    static class UI
    {
        private static ExpressionGenerator _generator = ExpressionGenerator.Instance;
        private static List<ExpressionFormat> _formats = new List<ExpressionFormat>();
        private static Regex _formatRegex = new Regex
            (
            @"(?x)                            # IgnorePatternWhitespace. Also allows comments
              ^                               # Anchor to start of string
                  (?>                         # Start non-backtracking (greedy) subexpression
                      (?<p> \( ) *             # Push to stack if '(' is found, repeat 0.. times
                      (?>\#)                  # Match one '#'
                      (?<-p> \) )*            # Pop off stack if ')' is found, repeat 0.. times
                  )                           # Matches a valid part of an expression up to the first operand
                  (?>                         # Start a non-backtracking (greedy) subexpression
                      [-+*/]                  # Match one operator symbol
                      (?>                     # Start non-backtracking (greedy) subexpression
                          (?<p> \( )*         # Push to stack if '(' is found, repeat 0.. times
                          (?>\#)              # Match one '#'
                          (?<-p> \) )*        # Pop off stack if ')' is found, repeat 0.. times
                      )                       # Matches a valid part of an expression up to the next operand (or the end if there is no next op.)
                  )*                          # 0.. times
                  (?(p)(?!))                  # Fail expression if stack 'p' is not empty
              $                               # Anchor to end of string"
            );

        private static Dictionary<string, Action> _actions = new Dictionary<string, Action>()
        {
            { "1", AddNewFormat},
            { "2", ShowExistingFormats },
            { "3", RemoveFormat },
            { "4", GenerateRandomExpression },
            { "5", GenerateExpressionFromFormat },
            { "6", null },
        };

        public static void Start()
        {
            while (true)
            {
                ShowMenu();
                ParseInput();
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("Enter a number corresponding to one of the commands below:");
            Console.WriteLine("\t1. Add a new format (e.g. '#+#*#/(#-#)')");
            Console.WriteLine("\t2. Show existing formats");
            Console.WriteLine("\t3. Remove a format");
            Console.WriteLine("\t4. Generate a random expression");
            Console.WriteLine("\t5. Generate an expression from a format");
            Console.WriteLine("\t6. Exit");
        }

        private static void ParseInput()
        {
            Console.Write("\nYour choice: ");
            string input = Console.ReadLine();
            Action action;
            if(!_actions.TryGetValue(input, out action))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input!");
                Console.ResetColor();
                return;
            }
            if (action == null)
                Environment.Exit(0);
            action();
        }
        
        private static void GenerateExpressionFromFormat()
        {
            if(_formats.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are no formats specified!");
                Console.ResetColor();
                return;
            }

            int resultLower, resultUpper, numberOfExpressions;
            string outFileName;

            while (!GetDesiredResult(out resultLower, out resultUpper)) ;
            while (!GetNumberOfExpressionsToGenerate(out numberOfExpressions)) ;
            while (!GetOutputFile(out outFileName)) ;
            TextWriter consoleOutput = Console.Out;
            FileStream stream = null;
            if (outFileName != string.Empty)
            {
                stream = new FileStream(outFileName, FileMode.Create);
                Console.SetOut(new StreamWriter(stream));
            }

            for (int i = 0; i < numberOfExpressions; i++)
            {
                int goal = ExpressionGenerator.Random.Next(resultLower, resultUpper + 1);
                var format = _formats.GetRandomElement();
                Console.WriteLine(_generator.FromFormat(format, goal));
            }

            Console.Out.Flush();
            stream?.Close();
            Console.SetOut(consoleOutput);
        }

        private static void GenerateRandomExpression()
        {
            int resultLower, resultUpper, numberOfExpressions, numberOfOperandsLower, numberOfOperandsUpper;
            string outFileName;

            while (!GetDesiredResult(out resultLower, out resultUpper));
            while (!GetNumberOfOperands(out numberOfOperandsLower, out numberOfOperandsUpper));
            while (!GetNumberOfExpressionsToGenerate(out numberOfExpressions));
            while (!GetOutputFile(out outFileName));
            TextWriter consoleOutput = Console.Out;
            FileStream stream = null;
            if(outFileName != string.Empty)
            {
                stream = new FileStream(outFileName, FileMode.Create);
                Console.SetOut(new StreamWriter(stream));
            }
            
            for (int i = 0; i < numberOfExpressions; i++)
            {
                int goal = ExpressionGenerator.Random.Next(resultLower, resultUpper + 1);
                int numberOfOperands = ExpressionGenerator.Random.Next(numberOfOperandsLower, numberOfOperandsUpper + 1);
                Console.WriteLine(_generator.BuildExpression(goal, numberOfOperands));
            }

            Console.Out.Flush();
            stream?.Close();
            Console.SetOut(consoleOutput);
        }

        private static void RemoveFormat()
        {
            Console.Write("Id of format to remove: ");
            string input = Console.ReadLine();
            int id;
            if(!int.TryParse(input, out id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must enter a number!");
                Console.ResetColor();
                return;
            }

            int removed = _formats.RemoveAll(x => x.Index == id);
            if(removed == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No formats with the specified ID found!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Format removed successfully.");
                Console.ResetColor();
            }
        }

        private static void ShowExistingFormats()
        {
            if(_formats == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No formats found.");
                Console.ResetColor();
                return;
            }

            foreach(var format in _formats)
                Console.WriteLine($"{format.Index}: {format.Format}");
        }

        private static void AddNewFormat()
        {
            Console.Write("Enter a format: ");
            string format = Console.ReadLine();

            format = Regex.Replace(format, @"\s+", "");

            if (!_formatRegex.IsMatch(format))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid format!");
                Console.ResetColor();
                return;
            }
            
            _formats.Add(new ExpressionFormat(format));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Format added successfully.");
            Console.ResetColor();
        }

        #region Helper methods
        private static bool GetDesiredResult(out int lower, out int upper)
        {
            lower = upper = -1;
            Console.Write("Desired result (one number for exact result, two numbers for a range): ");
            string input = Console.ReadLine();
            var args = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must enter at least one number!");
                Console.ResetColor();
                return false;
            }
            else if (args.Length == 1)
            {
                int result;
                if (!int.TryParse(args[0], out result))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter a number!");
                    Console.ResetColor();
                    return false;
                }
                lower = upper = result;
            }
            else if (args.Length == 2)
            {
                if (!int.TryParse(args[0], out lower) || !int.TryParse(args[1], out upper))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter a number!");
                    Console.ResetColor();
                    return false;
                }
                if (upper < lower)
                    MathHelper.Swap(ref lower, ref upper);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many numbers provided!");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        private static bool GetNumberOfOperands(out int lower, out int upper)
        {
            lower = upper = -1;
            Console.Write("Desired number of operands in each expression (one number for exact number of operands, two numbers for a range): ");
            string input = Console.ReadLine();
            var args = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must enter at least one number!");
                Console.ResetColor();
                return false;
            }
            else if (args.Length == 1)
            {
                int result;
                if (!int.TryParse(args[0], out result))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter a number!");
                    Console.ResetColor();
                    return false;
                }
                if (result < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There must be at least two operands!");
                    Console.ResetColor();
                    return false;
                }
                lower = upper = result;
            }
            else if (args.Length == 2)
            {
                if (!int.TryParse(args[0], out lower) || !int.TryParse(args[1], out upper))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter a number!");
                    Console.ResetColor();
                    return false;
                }
                if (upper < lower)
                    MathHelper.Swap(ref lower, ref upper);

                if (lower < 2 || upper < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There must be at least two operands!");
                    Console.ResetColor();
                    return false;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many numbers provided!");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        private static bool GetNumberOfExpressionsToGenerate(out int number)
        {
            Console.Write("Number of expressions to generate: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out number))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must enter a number!");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        private static bool GetOutputFile(out string outFileName)
        {
            Console.Write("Output file (leave blank if you want console output): ");
            outFileName = Console.ReadLine();
            if (outFileName != "")
            {
                try
                {
                    new FileInfo(outFileName);
                }
                catch (ArgumentException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid file name!");
                    Console.ResetColor();
                    return false;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
