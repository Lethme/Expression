using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Collections;
using Collections;

namespace Expression
{
    class Program
    {
        static public class Lib
        {
            static private class Command
            {
                /// <summary>
                /// Returns string parameters collection from ParamStr string
                /// </summary>
                /// <param name="ParamStr">Строка параметров</param>
                /// <param name="Pattern">Паттерн, по которому из строки выбираются параметры</param>
                static public MatchCollection ParamsCollection(string ParamStr, string Pattern = "([\"].+?[\"]|[^ ]+)+")
                {
                    var regex = new Regex(Pattern, RegexOptions.Singleline);
                    var Params = regex.Matches(ParamStr);
                    return Params;
                }
                /// <summary>
                /// Returns string parameter with ParamNumber index from ParamStr string
                /// </summary>
                /// <param name="ParamStr">Parameters string</param>
                /// <param name="ParamNumber">String parameter index in ParamStr</param>
                static public string GetParam(string ParamStr, int ParamNumber, string Pattern = "([\"].+?[\"]|[^ ]+)+")
                {
                    var Params = ParamsCollection(ParamStr, Pattern);
                    if (Params.Count - 1 < ParamNumber) return string.Empty;
                    else return Params[ParamNumber].ToString();
                }
                /// <summary>
                /// Reads a non-empty string form Console
                /// </summary>
                /// <param name="marker">Specifies a text label which appears on line before reading user input</param>
                static public string Enter(string marker = ">")
                {
                    string tempCommand;
                    do
                    {
                        Console.Write($"{marker} "); tempCommand = Console.ReadLine();
                    } while (tempCommand == String.Empty);
                    return tempCommand;
                }
            }
            static private class Output
            {
                static public void ClearLine(int Line)
                {
                    Console.MoveBufferArea(0, Line, Console.BufferWidth, 1, Console.BufferWidth, Line, ' ', Console.ForegroundColor, Console.BackgroundColor);
                    return;
                }
                static public bool Confirmation(string Line = "")
                {
                    char symb;
                    Console.CursorVisible = false;
                    do
                    {
                        Console.CursorLeft = 0;
                        Console.Write($"{Line} y/n: ");
                        symb = Console.ReadKey().KeyChar;
                    } while (symb != 'y' && symb != 'n');

                    Console.CursorVisible = true;

                    switch (symb)
                    {
                        case 'y': return true;
                        case 'n': return false;
                    }

                    return false;
                }
            }
            static public class Shell
            {
                static private string command;
                static private string command_base;
                static private int param_count;
                static private string[] param;
                static public string Command { get { return command; } }
                static public string CommandBase { get { return command_base; } }
                static public string[] Param { get { return param; } }
                static public int ParamCount { get { return param_count; } }
                static public void Ref()
                {
                    Console.WriteLine("Use 'help' to see all the available commands in current shell!");
                    Console.WriteLine("Use '-?' argument with any command to see an additional information!\n");
                    return;
                }
                static private void GetParams()
                {
                    command_base = Lib.Command.GetParam(command, 0);

                    var tempCollection = Lib.Command.ParamsCollection(command);
                    param_count = tempCollection.Count - 1;

                    param = new string[tempCollection.Count - 1];
                    for (int i = 1; i < tempCollection.Count; i++)
                    {
                        param[i - 1] = tempCollection[i].ToString().Trim('"');
                    }
                }
                static private void SetCommand(string Command)
                {
                    if (Command != String.Empty)
                    {
                        command = Command;
                        GetParams();
                    }
                }
                static public void ExecuteString(string Command)
                {
                    if (Command != String.Empty)
                    {
                        SetCommand(Command);
                        Execute();
                    }
                }
                static public void ExecuteString(string[] CommandArr)
                {
                    foreach (var Command in CommandArr)
                    {
                        if (Command != String.Empty)
                        {
                            SetCommand(Command);
                            Execute();
                        }
                    }
                }
                static public void Enter()
                {
                    command = Lib.Command.Enter();
                    GetParams();
                }
                static private void faq()
                {
                    int stream_size = 30;
                    Console.WriteLine("calc \"expression\"".PadLeft(stream_size) + " - Evaluate string math expression");
                    Console.WriteLine("rpn \"expression\"".PadLeft(stream_size) + " - Convert classic math expression to Reverse Polish Notation");
                    Console.WriteLine();
                    Console.WriteLine("help".PadLeft(stream_size) + " - Show all the available commands");
                    Console.WriteLine("cls".PadLeft(stream_size) + " - Clear the console output");
                    Console.WriteLine("exit".PadLeft(stream_size) + " - Exit");
                    Console.WriteLine();
                }
                static public void Execute()
                {
                    switch (command_base)
                    {
                        case "rpn":
                            {
                                if (param_count == 1)
                                {
                                    if (param[0] == "-t")
                                    {
                                        foreach (var item in Lib.Expression.Stack.Samples)
                                        {
                                            try
                                            {
                                                string result = Lib.Expression.Stack.GetExpression(item);
                                                Console.WriteLine($"[Stack] Current Expression: {item}");
                                                Console.WriteLine($"[Stack] RPN Expression: " + (result == String.Empty ? "Empty" : result) + "\n");
                                            }
                                            catch (Exception)
                                            {
                                                Console.WriteLine("[Stack] Invalid expression!\n");
                                            }
                                        }
                                        break;
                                    }

                                    try
                                    {
                                        string result = Lib.Expression.Stack.GetExpression(param[0]);
                                        Console.WriteLine($"[Stack] RPN Expression: " + (result == String.Empty ? "Empty" : result) + "\n");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("[Stack] Invalid expression!");
                                    }
                                }
                                else if (param_count == 0)
                                {
                                    Console.WriteLine("Expression should be stated!");
                                    Console.WriteLine("Better use brackets '\"' for typing your expression!\n");
                                }
                                else
                                {
                                    Console.WriteLine("Too much arguments!\n");
                                }
                                break;
                            }
                        case "calc":
                            {
                                if (param_count == 1)
                                {
                                    if (param[0] == "-t")
                                    {
                                        foreach (var item in Lib.Expression.Stack.Samples)
                                        {
                                            try
                                            {
                                                double result = Lib.Expression.Stack.Parse(item);
                                                Console.WriteLine($"[Stack] Current Expression: {item}");
                                                Console.WriteLine($"[Stack] Expression result: {result}\n");
                                            }
                                            catch (Exception)
                                            {
                                                Console.WriteLine("[Stack] Invalid expression!\n");
                                            }
                                        }
                                        break;
                                    }

                                    try
                                    {
                                        double result = Lib.Expression.Stack.Parse(param[0]);
                                        Console.WriteLine($"[Stack] Expression result: {result}\n");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("[Stack] Invalid expression!\n");
                                    }
                                }
                                else if (param_count == 0)
                                {
                                    Console.WriteLine("Expression should be stated!");
                                    Console.WriteLine("Better use brackets '\"' for typing your expression!\n");
                                }
                                else
                                {
                                    Console.WriteLine("Too much arguments!\n");
                                }
                                break;
                            }
                        case "help":
                            {
                                Lib.Shell.faq();
                                return;
                            }
                        case "cls":
                            {
                                Console.Clear();
                                Lib.Shell.Ref();
                                return;
                            }
                        case "exit":
                            {
                                if (Lib.Output.Confirmation("Are you sure you want to exit?"))
                                {
                                    Console.Clear();
                                    System.Environment.Exit(0);
                                }
                                else
                                {
                                    Console.WriteLine('\n');
                                    return;
                                }
                                return;
                            }
                        default:
                            {
                                Console.WriteLine("Wrong command!\n");
                                return;
                            }
                    }
                }
            }
            static public class Expression
            {
                static public class Regex
                {
                    public static bool TryParse(string str)
                    {
                        try
                        {
                            Parse(str);
                            return true;
                        }
                        catch (FormatException)
                        {
                            return false;
                        }
                    }

                    public static double Parse(string str)
                    {
                        // Парсинг скобок
                        Match matchSk = System.Text.RegularExpressions.Regex.Match(str, string.Format(@"\(({0})\)", @"[1234567890\.\+\-\*\/^%]*"));
                        if (matchSk.Groups.Count > 1)
                        {
                            string inner = matchSk.Groups[0].Value.Substring(1, matchSk.Groups[0].Value.Trim().Length - 2);
                            string left = str.Substring(0, matchSk.Index);
                            string right = str.Substring(matchSk.Index + matchSk.Length);
                            return Parse(left + Parse(inner) + right);
                        }

                        // Парсинг действий
                        Match matchMulOp = System.Text.RegularExpressions.Regex.Match(str, string.Format(@"({0})\s?({1})\s?({0})\s?", RegexNum, RegexMulOp));
                        Match matchAddOp = System.Text.RegularExpressions.Regex.Match(str, string.Format(@"({0})\s?({1})\s?({2})\s?", RegexNum, RegexAddOp, RegexNum));
                        var match = (matchMulOp.Groups.Count > 1) ? matchMulOp : (matchAddOp.Groups.Count > 1) ? matchAddOp : null;
                        if (match != null)
                        {
                            string left = str.Substring(0, match.Index);
                            string right = str.Substring(match.Index + match.Length);
                            string val = ParseAct(match).ToString(CultureInfo.InvariantCulture);
                            return Parse(string.Format("{0}{1}{2}", left, val, right));
                        }

                        // Парсинг числа
                        try
                        {
                            return double.Parse(str, CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            throw new FormatException(string.Format("Неверная входная строка '{0}'", str));
                        }
                    }

                    private const string RegexNum = @"[-]?\d+\.?\d*";
                    private const string RegexMulOp = @"[\*\/^%]";
                    private const string RegexAddOp = @"[\+\-]";

                    private static double ParseAct(Match match)
                    {
                        double a = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                        double b = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                        switch (match.Groups[2].Value)
                        {
                            case "+":
                                return a + b;

                            case "-":
                                return a - b;

                            case "*":
                                return a * b;

                            case "/":
                                return a / b;

                            case "^":
                                return Math.Pow(a, b);

                            case "%":
                                return a % b;

                            default:
                                throw new FormatException(string.Format("Неверная входная строка '{0}'", match.Value));
                        }
                    }
                }
                static public class Stack
                {
                    static private string[] _samples = new string[] 
                    {
                        "(16^(1/4))^(5/(64/(2^7)))",
                        "((3*15)-(7+7*4))/(1,25*8^(1/3))"
                    };
                    static public string[] Samples => _samples;
                    static private bool IsDelimeter(char c)
                    {
                        if ((" =".IndexOf(c) != -1)) return true;
                        return false;
                    }
                    static private bool IsOperator(char с)
                    {
                        if (("+-/*^()".IndexOf(с) != -1)) return true;
                        return false;
                    }
                    static private byte GetPriority(char s)
                    {
                        switch (s)
                        {
                            case '(': return 0;
                            case ')': return 1;
                            case '+': return 2;
                            case '-': return 3;
                            case '*': return 4;
                            case '/': return 4;
                            case '^': return 5;
                            default: return 6;
                        }
                    }
                    static public double Parse(string input)
                    {
                        string output = GetExpression(input);
                        double result = Counting(output);
                        return result;
                    }
                    static public string GetExpression(string input)
                    {
                        string output = string.Empty;
                        Stack<char> operStack = new Stack<char>();

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (IsDelimeter(input[i]))
                                continue;

                            if (Char.IsDigit(input[i]))
                            {
                                while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                                {
                                    output += input[i];
                                    i++;

                                    if (i == input.Length) break;
                                }

                                output += " ";
                                i--;
                            }

                            if (IsOperator(input[i]))
                            {
                                if (input[i] == '(')
                                    operStack.Push(input[i]);
                                else if (input[i] == ')')
                                {
                                    char s = operStack.Pop();

                                    while (s != '(')
                                    {
                                        output += s.ToString() + ' ';
                                        s = operStack.Pop();
                                    }
                                }
                                else
                                {
                                    if (operStack.Count > 0)
                                        if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                            output += operStack.Pop().ToString() + " ";

                                    operStack.Push(char.Parse(input[i].ToString()));
                                }
                            }
                        }

                        while (operStack.Count > 0)
                            output += operStack.Pop() + " ";

                        return output;
                    }
                    static private double Counting(string input)
                    {
                        double result = 0;
                        Stack<double> temp = new Stack<double>();

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (Char.IsDigit(input[i]))
                            {
                                string a = string.Empty;

                                while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                                {
                                    a += input[i];
                                    i++;
                                    if (i == input.Length) break;
                                }
                                temp.Push(double.Parse(a));
                                i--;
                            }
                            else if (IsOperator(input[i]))
                            {
                                double a = temp.Pop();
                                double b = temp.Pop();

                                switch (input[i])
                                {
                                    case '+': result = b + a; break;
                                    case '-': result = b - a; break;
                                    case '*': result = b * a; break;
                                    case '/': result = b / a; break;
                                    case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                                }
                                temp.Push(result);
                            }
                        }
                        return temp.Peek();
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Lib.Shell.Ref();
            while (true)
            {
                Console.Title = "Console";
                Lib.Shell.Enter();
                Lib.Shell.Execute();
            }
        }
    }
}