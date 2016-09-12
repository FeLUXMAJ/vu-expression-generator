using ExpressionGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExpressionGenerator.Menu;
using IO = ExpressionGenerator.Helpers.IOHelper;

namespace ExpressionGenerator
{
    static class UI
    {
        private static ExpressionGenerator _generator = ExpressionGenerator.Instance;
        private static HashSet<ExpressionFormat> _formats = new HashSet<ExpressionFormat>(); //[[11]]
        //[[9]]
        private static Regex _formatRegex = new Regex
            (
            @"(?x)                            # IgnorePatternWhitespace. Also allows comments
              ^                               # Anchor to start of string
                  (?>                         # Start non-backtracking (greedy) subexpression
                      (?<p> \( ) *            # Push to stack if '(' is found, repeat 0.. times
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

        private static List<MenuItem> _menuItems = new List<MenuItem>()
        {
            new MenuItem(1, "`Add` a `new` format", new string[] { "1", "add", "a", "new", "n" }),
            new MenuItem(2, "`Load` formats from file", new string[] { "2", "load", "l" }),
            new MenuItem(3, "`Show` existing formats", new string[] { "3", "show", "s", "existing", "e", "ex", "exist" }),
            new MenuItem(4, "`Remove` a format", new string[] { "4", "remove", "r", "rem" }),
            new MenuItem(5, "Generate a `random` expression", new string[] { "5", "random", "rand" }),
            new MenuItem(6, "Generate an expression from a `format`", new string[] { "6", "format", "form", "f" }),
            new MenuItem(7, "`Exit`", new string[] { "7", "exit" }),
        };

        private static List<MenuAction> _menuActions = new List<MenuAction>()
        {
            new MenuAction(1, AddNewFormat),
            new MenuAction(2, LoadFormatsFromFile),
            new MenuAction(3, ShowExistingFormats),
            new MenuAction(4, RemoveFormat),
            new MenuAction(5, GenerateRandomExpression),
            new MenuAction(6, GenerateExpressionFromFormat),
            new MenuAction(7, ExitProgram),
        };

        private static Dictionary<string, Action> _actions = null;

        public static void Start()
        {
            if(_actions == null)
            {
                _actions = (from action in _menuActions
                            join item in _menuItems on action.ItemId equals item.Id into temp
                            from t in temp
                            from a in t.Aliases
                            select new { Alias = a, Action = action }
                            ).ToDictionary(x => x.Alias, x => x.Action.Action);
            }
            while (true)
            {
                ShowMenu();
                ParseInput();
            }
        }

        private static void ShowMenu()
        {
            foreach(var item in _menuItems)
                IO.WriteLine("`{0}`. {1}", item.Id, item.Description);
            IO.WriteLine("");
        }

        private static void ParseInput()
        {
            string input = IO.ReadLine("Your choice: ");
            Action action;
            if(!_actions.TryGetValue(input, out action))
            {
                IO.WriteLineError("Invalid input!");
                return;
            }
            action();
        }

        private static void LoadFormatsFromFile()
        {
            string fileName;
            while (!GetInputFile(out fileName)) ;
            if (!File.Exists(fileName))
            {
                IO.WriteLineError("The specified file could not be found.");
                return;
            }
            string[] lines = File.ReadAllLines(fileName).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            foreach(string line in lines)
                if (!AddNewFormat(line))
                    return;
            IO.WriteLineSuccess("All formats added successfully.");
        }

        private static void ExitProgram()
        {
            Environment.Exit(0);
        }
        
        private static void GenerateExpressionFromFormat()
        {
            if(_formats.Count == 0)
            {
                IO.WriteLineError("There are no formats specified!");
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

            IO.RedirectOutputToFile(outFileName, FileMode.Create);
            
            for (int i = 0; i < numberOfExpressions; i++)
            {
                int goal = ExpressionGenerator.Random.Next(resultLower, resultUpper + 1);
                int numberOfOperands = ExpressionGenerator.Random.Next(numberOfOperandsLower, numberOfOperandsUpper + 1);
                Console.WriteLine(_generator.BuildExpression(goal, numberOfOperands));
            }

            IO.RedirectOutputToConsole();
        }

        private static void RemoveFormat()
        {
            string input = IO.ReadLine("Id of format to remove: ");
            int id;
            if(!int.TryParse(input, out id))
            {
                IO.WriteLineError("You must enter a number!");
                return;
            }
            
            int removed = _formats.RemoveWhere(x => x.Index == id); //[[12]]
            if (removed == 0)
            {
                IO.WriteLineError("No formats with the specified ID found!");
            }
            else
            {
                IO.WriteLineSuccess("Format removed successfully.");
            }
        }

        private static void ShowExistingFormats()
        {
            if(_formats.Count == 0)
            {
                IO.WriteLineWarning("No formats found.");
                return;
            }

            foreach(var format in _formats.OrderBy(x => x.Index))
                Console.WriteLine($"{format.Index}: {format.Format}");
        }

        private static void AddNewFormat()
        {
            string format = IO.ReadLine("Enter a format: ");
            if(AddNewFormat(format))
                IO.WriteLineSuccess("Format added successfully.");
        }

        private static bool AddNewFormat(string format)
        {
            format = Regex.Replace(format, @"\s+", "");

            if (format.Length < 3 || !_formatRegex.IsMatch(format))
            {
                IO.WriteLineError("Format '{0}' is invalid!", format);
                return false;
            }

            if(!_formats.Add(new ExpressionFormat(format)))
            {
                IO.WriteLineError("Format '{0}' already exists!", format);
                return false;
            }
            return true;
        }

        #region Helper methods
        private static bool GetDesiredResult(out int lower, out int upper)
        {
            lower = upper = -1;
            string input = IO.ReadLine("Desired result (one number for exact result, two numbers for a range): ");
            var args = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
            {
                IO.WriteLineError("You must enter at least one number!");
                return false;
            }
            else if (args.Length == 1)
            {
                int result;
                if (!int.TryParse(args[0], out result))
                {
                    IO.WriteLineError("You must enter a number!");
                    return false;
                }
                lower = upper = result;
            }
            else if (args.Length == 2)
            {
                if (!int.TryParse(args[0], out lower) || !int.TryParse(args[1], out upper))
                {
                    IO.WriteLineError("You must enter a number!");
                    return false;
                }
                if (upper < lower)
                    MathHelper.Swap(ref lower, ref upper);
            }
            else
            {
                IO.WriteLineError("Too many numbers provided!");
                return false;
            }
            return true;
        }

        private static bool GetNumberOfOperands(out int lower, out int upper)
        {
            lower = upper = -1;
            string input = IO.ReadLine("Desired number of operands in each expression (one number for exact number of operands, two numbers for a range): ");
            var args = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
            {
                IO.WriteLineError("You must enter at least one number!");
                return false;
            }
            else if (args.Length == 1)
            {
                int result;
                if (!int.TryParse(args[0], out result))
                {
                    IO.WriteLineError("You must enter a number!");
                    return false;
                }
                if (result < 2)
                {
                    IO.WriteLineError("There must be at least two operands!");
                    return false;
                }
                lower = upper = result;
            }
            else if (args.Length == 2)
            {
                if (!int.TryParse(args[0], out lower) || !int.TryParse(args[1], out upper))
                {
                    IO.WriteLineError("You must enter a number!");
                    return false;
                }
                if (upper < lower)
                    MathHelper.Swap(ref lower, ref upper);

                if (lower < 2 || upper < 2)
                {
                    IO.WriteLineError("There must be at least two operands!");
                    return false;
                }
            }
            else
            {
                IO.WriteLineError("Too many numbers provided!");
                return false;
            }
            return true;
        }

        private static bool GetNumberOfExpressionsToGenerate(out int number)
        {
            string input = IO.ReadLine("Number of expressions to generate: ");
            if (!int.TryParse(input, out number))
            {
                IO.WriteLineError("You must enter a number!");
                return false;
            }
            return true;
        }

        private static bool GetInputFile(out string inputFileName)
        {
            inputFileName = IO.ReadLine("Input file: ");
            if(inputFileName == string.Empty)
            {
                IO.WriteLineError("No input file provided.");
                return false;
            }

            try
            {
                new FileInfo(inputFileName);
            }
            catch (ArgumentException)
            {
                IO.WriteLineError("Invalid file name!");
                return false;
            }
            catch (Exception e)
            {
                IO.WriteLineError(e.Message);
                return false;
            }
            return true;
        }

        private static bool GetOutputFile(out string outFileName)
        {
            outFileName = IO.ReadLine("Output file (leave blank if you want console output): ");
            if (outFileName != "")
            {
                try
                {
                    new FileInfo(outFileName);
                }
                catch (ArgumentException)
                {
                    IO.WriteLineError("Invalid file name!");
                    return false;
                }
                catch (Exception e)
                {
                    IO.WriteLineError(e.Message);
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
