﻿using System;
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
                                    try
                                    {
                                        string result = Lib.Expression.GetRPNExpression(param[0]);
                                        Console.WriteLine($"[Calc] RPN Expression: " + (result == String.Empty ? "Empty" : result) + "\n");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("[Calc] Invalid expression!");
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
                                if (param_count == 2)
                                {
                                    if (param[0] == "-r")
                                    {
                                        try
                                        {
                                            double result = Lib.Expression.EvaluateRPN(param[1]);
                                            Console.WriteLine($"[Calc] RPN Expression result: {result}\n");
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("[Calc] Invalid expression!\n");
                                        }
                                    }
                                    else
                                    {
                                        if (param[1] != "true" && param[1] != "false") break;
                                        switch (param[0])
                                        {
                                            case "--exprpn":
                                                {
                                                    if (Lib.Expression.Samples.SwitchExpectedRPN(Boolean.Parse(param[1])))
                                                        Console.WriteLine("[Calc] Expected RPN expressions will be shown in samples implementation.\n");
                                                    else
                                                        Console.WriteLine("[Calc] Expected RPN expressions will be hidden in samples implementation.\n");
                                                    break;
                                                }
                                            case "--expres":
                                                {
                                                    if (Lib.Expression.Samples.SwitchExpectedResult(Boolean.Parse(param[1])))
                                                        Console.WriteLine("[Calc] Expected expression results will be shown in samples implementation.\n");
                                                    else
                                                        Console.WriteLine("[Calc] Expected expression results will be hidden in samples implementation.\n");
                                                    break;
                                                }
                                            case "--actrpn":
                                                {
                                                    if (Lib.Expression.Samples.SwitchActualRPN(Boolean.Parse(param[1])))
                                                        Console.WriteLine("[Calc] Actual RPN expressions will be shown in samples implementation.\n");
                                                    else
                                                        Console.WriteLine("[Calc] Actual RPN expressions will be hidden in samples implementation.\n");
                                                    break;
                                                }
                                        }
                                    }
                                    break;
                                }
                                else if (param_count == 1)
                                {
                                    if (param[0] == "-t")
                                    {
                                        Lib.Expression.Samples.ImplementSamples();
                                        break;
                                    }

                                    try
                                    {
                                        double result = Lib.Expression.Parse(param[0]);
                                        Console.WriteLine($"[Calc] Expression result: {result}\n");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("[Calc] Invalid expression!\n");
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
                static public class Samples
                {
                    static private bool _show_expected_rpn = false;
                    static private bool _show_expected_result = true;
                    static private bool _show_actual_rpn = false;
                    static public bool SwitchExpectedRPN(bool state)
                    {
                        _show_expected_rpn = state;
                        return _show_expected_rpn;
                    }
                    static public bool SwitchExpectedResult(bool state)
                    {
                        _show_expected_result = state;
                        return _show_expected_result;
                    }
                    static public bool SwitchActualRPN(bool state)
                    {
                        _show_actual_rpn = state;
                        return _show_actual_rpn;
                    }
                    private struct Sample
                    {
                        public string Expression { get; }
                        public double ExpectedResult { get; }
                        public string ExpectedRPN { get; }
                        public Sample(string Expression, string ExpectedRPN = "", double ExpectedResult = Double.NaN)
                        {
                            this.Expression = Expression;
                            this.ExpectedResult = ExpectedResult;
                            this.ExpectedRPN = ExpectedRPN;
                        }
                    }
                    static private Sample[] _samples = new Sample[]
                    {
                        new Sample("(16^(1/4))^(5/(64/(2^7)))", "16 1 4 / ^ 5 64 2 7 ^ / / ^", 1024),
                        new Sample("((3*15)-(7+7*4))/(1,25*8^(1/3))", "3 15 * 7 7 4 * + - 1,25 8 1 3 / ^ * /", 4),
                        new Sample("5*(-3)", "5 3 $ *", -15),
                        new Sample("-10*7", "10 $ 7 *", -70),
                        new Sample("2^(-1)", "2 1 $ ^", 0.5),
                        new Sample("-5*(5^2/11*(-3)-1)^2", "5 $ 5 2 ^ 11 / 3 $ * 1 - 2 ^ *", -305.6198347),
                        new Sample("sqrt(40-(-9))+(-2/5*(-(-5))+423,233^2/15^3,33333)", "40 9 $ - # 11 22 3 / ^ 22,222 / 44,3321 $ - 6 1 3 / ^ + +", 26.520817722),
                        new Sample("sqrt(40-(-9)) + (11^(22/3)/22,222)-(-44,3321)+6^(1/3)", "", 1950331.26665),
                        new Sample("sqrt(sqr(23,33))-4/sqr(sqrt(sqr((-16)/(-4))))", "", 23.08),
                        new Sample("sqrt(85^2/(21^(-3))-11*11/(-55-(-55+33*123/443)))", "85 2 ^ 21 3 $ ^ / 11 11 * 55 $ 55 $ 33 123 * 443 / + - / - #", 8179.89842272),
                        new Sample("sqr(sin(-0,5)) + sqr(cos(-0,5))", "0,5 $ S & 0,5 $ C & +", 1),
                        new Sample("sin(-0,5)^2 + cos(-0,5)^2", "0,5 $ S 2 ^ 0,5 $ C 2 ^ +", 1),
                        new Sample("1/sin(cos(sqrt(1-(-2,34))*(1,3+5,85^2)-2,71^(15/0,9455))/3,76534)", "1 1 2,34 $ - # 1,3 5,85 2 ^ + * 2,71 15 0,9455 / ^ - C 3,76534 / S /", 3.8099727)
                    };
                    static public void ImplementSamples()
                    {
                        int spaces_from_left = 30;
                        foreach (var item in _samples)
                        {
                            try
                            {
                                Console.WriteLine("[Calc]" + "Current expression: ".PadLeft(spaces_from_left) + item.Expression);
                                if (_show_expected_rpn) Console.WriteLine("[Calc]" + "Expected RPN expression: ".PadLeft(spaces_from_left) + item.ExpectedRPN);
                                if (_show_expected_result) Console.WriteLine("[Calc]" + "Expected expression result: ".PadLeft(spaces_from_left) + item.ExpectedResult);
                                double result = Parse(item.Expression);
                                string resultRPN = GetRPNExpression(item.Expression);
                                if (_show_actual_rpn) Console.WriteLine("[Calc]" + "Actual RPN expression: ".PadLeft(spaces_from_left) + resultRPN);
                                Console.WriteLine("[Calc]" + "Actual expression result: ".PadLeft(spaces_from_left) + result);
                                Console.WriteLine();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("[Calc] Invalid expression!\n");
                            }
                        }
                    }
                }
                static private class Enums
                {
                    public enum UnaryOperators
                    {
                        Minus = '$',
                        SquareRoot = '#',
                        Square = '&',
                        Sin = 'S',
                        Cos = 'C',
                        Tan = 't',
                        Ctg = 'c',
                        Ln = 'l',
                        Lg = 'g',
                        Log = 'd'
                    }
                    public enum ConstantOperators
                    {
                        Pi = 'p',
                        E = 'e'
                    }
                    public enum BinaryOperators
                    {
                        Plus = '+',
                        Minus = '-',
                        Mul = '*',
                        Div = '/',
                        Pow = '^',
                        LeftBracket = '(',
                        RightBracket = ')'
                    }
                    public enum Delimeters
                    {
                        Space = ' ',
                        Equal = '='
                    }
                }
                static private bool IsDelimeter(char c)
                {
                    if (Enum.IsDefined(typeof(Enums.Delimeters), (int)c)) return true;
                    return false;
                }
                static private bool IsBinaryOperator(char c)
                {
                    if (Enum.IsDefined(typeof(Enums.BinaryOperators), (int)c)) return true;
                    return false;
                }
                static private bool IsUnaryOperator(char c)
                {
                    if (Enum.IsDefined(typeof(Enums.UnaryOperators), (int)c)) return true;
                    return false;
                }
                static private bool IsConstantOperator(char c)
                {
                    if (Enum.IsDefined(typeof(Enums.ConstantOperators), (int)c)) return true;
                    return false;
                }
                static private byte GetBinaryOperatorPriority(char s)
                {
                    switch (s)
                    {
                        case (char)Enums.BinaryOperators.LeftBracket: return 0;
                        case (char)Enums.BinaryOperators.RightBracket: return 1;
                        case (char)Enums.BinaryOperators.Plus: return 2;
                        case (char)Enums.BinaryOperators.Minus: return 3;
                        case (char)Enums.BinaryOperators.Mul: return 4;
                        case (char)Enums.BinaryOperators.Div: return 4;
                        case (char)Enums.BinaryOperators.Pow: return 5;
                        default: return 6;
                    }
                }
                static public string GetRPNExpression(string input)
                {
                    string output = string.Empty;
                    Stack<char> operStack = new Stack<char>();

                    for (int i = 0; i < input.Length; i++)
                    {
                        if (IsDelimeter(input[i]))
                            continue;

                        if (Char.IsDigit(input[i]))
                        {
                            while (!IsDelimeter(input[i]) && !IsBinaryOperator(input[i]))
                            {
                                output += input[i];
                                i++;

                                if (i == input.Length) break;
                            }

                            output += " ";
                            i--;
                        }
                        if (Char.IsLetter(input[i]))
                        {
                            string function = String.Empty;
                            while (Char.IsLetter(input[i]))
                            {
                                function += input[i];
                                if (i + 1 == input.Length) break;
                                i++;
                            }

                            switch (function)
                            {
                                case "sqrt": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.SquareRoot); break; }
                                case "sqr": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Square); break; }
                                case "sin": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Sin); break; }
                                case "cos": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Cos); break; }
                                case "tan": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Tan); break; }
                                case "ctg": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Ctg); break; }
                                case "ln": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Ln); break; }
                                case "lg": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Lg); break; }
                                case "log": { if (input[i] != '(') throw new ArgumentException(); operStack.Push((char)Enums.UnaryOperators.Log); break; }
                                case "pi": { operStack.Push((char)Enums.ConstantOperators.Pi); break; }
                                case "e": { operStack.Push((char)Enums.ConstantOperators.E); break; }
                                default: { throw new ArgumentException(); }
                            }
                        }
                        if (IsBinaryOperator(input[i]))
                        {
                            if (input[i] == (char)Enums.BinaryOperators.LeftBracket)
                                operStack.Push(input[i]);
                            else if (input[i] == (char)Enums.BinaryOperators.RightBracket)
                            {
                                char s = operStack.Pop();

                                while (s != (char)Enums.BinaryOperators.LeftBracket)
                                {
                                    output += s.ToString() + ' ';
                                    s = operStack.Pop();
                                }
                            }
                            else
                            {
                                if (operStack.Count > 0)
                                    if (GetBinaryOperatorPriority(input[i]) <= GetBinaryOperatorPriority(operStack.Peek()))
                                        output += operStack.Pop().ToString() + " ";

                                if (input[i] == (char)Enums.BinaryOperators.Minus)
                                {
                                    var j = i;
                                    while (j > 0 && !Char.IsDigit(input[j]) && !(input[j] == (char)Enums.BinaryOperators.LeftBracket))
                                    {
                                        j--;
                                    }
                                    if (input[j] == (char)Enums.BinaryOperators.LeftBracket || j == 0) operStack.Push((char)Enums.UnaryOperators.Minus);
                                    else operStack.Push((char)Enums.BinaryOperators.Minus);
                                }
                                else
                                {
                                    operStack.Push(char.Parse(input[i].ToString()));
                                }
                            }
                        }
                    }

                    while (operStack.Count > 0)
                        output += operStack.Pop() + " ";

                    return output;
                }
                static public double EvaluateRPN(string input)
                {
                    double result = 0;
                    Stack<double> numbers = new Stack<double>();

                    for (int i = 0; i < input.Length; i++)
                    {
                        if (Char.IsDigit(input[i]))
                        {
                            string a = string.Empty;

                            while (!IsDelimeter(input[i]) && !IsBinaryOperator(input[i]))
                            {
                                a += input[i];
                                i++;
                                if (i == input.Length) break;
                            }
                            numbers.Push(double.Parse(a));
                            i--;
                        }
                        else if (IsConstantOperator(input[i]))
                        {
                            switch (input[i])
                            {
                                case (char)Enums.ConstantOperators.Pi: { numbers.Push(Math.PI); break; }
                                case (char)Enums.ConstantOperators.E: { numbers.Push(Math.E); break; }
                            }
                        }
                        else if (IsUnaryOperator(input[i]))
                        {
                            double a = numbers.Pop();

                            switch (input[i])
                            {
                                case (char)Enums.UnaryOperators.Minus: result = -a; break;
                                case (char)Enums.UnaryOperators.SquareRoot: { if (a < 0) throw new ArgumentOutOfRangeException(); result = Math.Sqrt(a); break; }
                                case (char)Enums.UnaryOperators.Square: { result = a * a; break; }
                                case (char)Enums.UnaryOperators.Sin: { result = Math.Sin(a); break; }
                                case (char)Enums.UnaryOperators.Cos: { result = Math.Cos(a); break; }
                                case (char)Enums.UnaryOperators.Tan: { result = Math.Tan(a); break; }
                                case (char)Enums.UnaryOperators.Ctg: { result = 1 / Math.Tan(a); break; }
                                case (char)Enums.UnaryOperators.Ln: { if (a <= 0) throw new ArgumentException(); result = Math.Log(a); break; }
                                case (char)Enums.UnaryOperators.Lg: { if (a <= 0) throw new ArgumentException(); result = Math.Log10(a); break; }
                                case (char)Enums.UnaryOperators.Log: { if (a <= 0) throw new ArgumentException(); result = Math.Log(a) / Math.Log(2); break; }
                            }
                            if (Double.IsNaN(result)) throw new ArgumentException();
                            numbers.Push(result);
                        }
                        else if (IsBinaryOperator(input[i]))
                        {
                            double a = numbers.Pop();
                            double b = numbers.Pop();

                            switch (input[i])
                            {
                                case (char)Enums.BinaryOperators.Plus: result = b + a; break;
                                case (char)Enums.BinaryOperators.Minus: result = b - a; break;
                                case (char)Enums.BinaryOperators.Mul: result = b * a; break;
                                case (char)Enums.BinaryOperators.Div:
                                    {
                                        if (a == 0) throw new DivideByZeroException();
                                        result = b / a;
                                        break;
                                    }
                                case (char)Enums.BinaryOperators.Pow: result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                            }
                            if (Double.IsNaN(result)) throw new ArgumentException();
                            numbers.Push(result);
                        }
                    }
                    return numbers.Pop();
                }
                static public double Parse(string input)
                {
                    string output = GetRPNExpression(input);
                    double result = EvaluateRPN(output);
                    return result;
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