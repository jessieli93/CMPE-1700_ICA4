using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ICA4
{
    class Program
    {
        static void Main(string[] args)
        {
            bool decrypt = false;  //Am I encrypting or decrypting?
            uint n = 0; //What is the amount I am (de)rotating?
            bool fileIO = false; //Am I encrypting/decrypting a file?
            string text = ""; //What is the string I am [en|de]crypting?
            string fileName = ""; //What is the file I am [en|de]crypting?
            
            //Check args
            string line;
            int n1=0;
            //Confirm I have between 3 and 4 args
            if (args.Count() < 3 || args.Count() > 4)
                PrintError("Invalid number of arguments (" + args.Count() + ")", "Expected either 3 or 4 arguments.", true, true, -1);

            //First arg should be n
            try
            {
                n = uint.Parse(args[0]);
            }
            catch (Exception e)
            {
                PrintError("Invalid rotation number (" + args[0] + ").", e.Message, true, true, -2);
            }

            //Next arg should be either -e or -d
            switch (args[1])
            {
                case "-e": decrypt = false; break;
                case "-d": decrypt = true; break;
                default:
                    PrintError("Unexpected encryption/decryption flag (" + args[1] + ").", "Expected -e or -d.", false, true, -3);
                    break;
            }

            //Next arg should be either -f or the string to (de)rotate.
            if (args[2] == "-f")
            {
                if (args.Count() < 4)
                    PrintError("Expected filename after -f argument.", "", false, true, -4);
                fileIO = true;
                fileName = args[3];
            }
            else
            {
                if (args.Count() > 3)
                    PrintError("Unexpected arguments after string, starting with \"" + args[3] + "\". Perhaps you need to put the string in quotes?"
                        , "", false, true, -5);
                fileIO = false;
                text = args[2];
            }

            //REPLACE THIS SECTION WITH YOUR OWN CODE.  YOU'LL WANT A FEW ADDITIONAL METHODS, TOO.
            if (decrypt == false)
            {
                 n1= Convert.ToInt32(n);
            }
            else
            {
                n1 = Convert.ToInt32(n) * -1;
            }
            if (!fileIO)
            {
                Console.WriteLine("Here is where I would " + (decrypt ? "decrypt" : "encrypt") +
                    " the string \"" + text + "\" using rot " + n + ".");
                Console.WriteLine();
                string[] codes = new string[text.Length];
                codes = shift(text, n1);
                for (int i = 0; i < text.Length; i++)
                {
                    Console.Write(codes[i]);
                }
                Console.WriteLine();

                
            }
            else
            {
                string[] fi = new string[1];
                string sentence = null;
                if (File.Exists(@fileName))
                {

                    Console.WriteLine();
                    Console.WriteLine("Here is where I would " + (decrypt ? "decrypt" : "encrypt") +
                    " the file \"" + fileName + "\" using rot " + n + ".");
                    Console.WriteLine();
                    System.IO.StreamReader file = new System.IO.StreamReader(@fileName.ToString());
                    Console.Write("Here is what the sentence and the " + (decrypt ? "decrypt" : "encrypt") +
                        " sentence(s) would be: ");
                    Console.WriteLine();

                    fi[0] = file.ReadToEnd();
                    string s = fi[0];
                    file.Close();
                    fi = shift(fi[0], n1);
                    
                    foreach (string i in fi)
                    {
                        Console.Write(i);
                        sentence += i;
                    }
                    File.WriteAllText(fileName, sentence);
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("The filename : " + fileName + " does not exist.");
                Console.WriteLine();
            }

        }
        static string[] shift(string value, int shift)
        {
            int sh1 = Convert.ToInt32(shift);
            int sh2 = Convert.ToInt32(shift);
            if (sh1 > 26)
            {
                sh1 = (sh1 % 26);
            }
            if (sh2 > 10)
            {
                sh2 = sh2 % 10;
            }
            char[] buffer = value.ToCharArray();
            string[] result = new string[value.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                // Letter.
                char letter = buffer[i];
                string nu;
                int nu1;


                if (char.IsLetter(buffer[i]))
                {


                    // Add shift to all.
                    letter = (char)(letter + sh1);
                    //Console.WriteLine(letter + " " + shift);

                    // Subtract 26 on overflow.
                    // Add 26 on underflow.
                    if (letter > 'z')
                    {
                        letter = (char)(letter - 26);
                        //Console.WriteLine("The letter is " + letter);
                    }
                    else if (letter < 'a')
                    {
                        letter = (char)(letter + 26);
                        //Console.WriteLine("The letter is " + letter);
                    }

                    result[i] = (letter.ToString());
                }
                if (char.IsDigit(buffer[i]))
                {
                    nu = Convert.ToString(buffer[i]);
                    nu1 = Convert.ToInt32(nu);
                    // Console.WriteLine(buffer[i]);
                    //Console.WriteLine("the nu is " + nu1);
                    nu1 = (nu1 + sh2);
                    //onsole.WriteLine("The number is " + nu1);
                    if (nu1 > 9)
                    {
                        nu1 = (int)(nu1 - 10);
                    }
                    else if (nu1 < 0)
                    {
                        nu1 = (int)(nu1 + 10);
                    }
                    //Console.WriteLine(nu1);
                    result[i] = Convert.ToString(nu1);

                }
                if (char.IsPunctuation(buffer[i]))
                {
                    string n = Convert.ToString(buffer[i]);
                    result[i] = n;
                }
                if (char.IsUpper(buffer[i]))
                {

                    string s = buffer[i].ToString();
                    s = s.ToLower();
                    char l = Convert.ToChar(s);
                    //Console.WriteLine(l);
                    l = (char)(l + sh1);
                    //Console.WriteLine("The letter is " + letter);

                    //Subtract 26 on overflow.
                    // Add 26 on underflow.
                    if (l > 'z')
                    {
                        l = (char)(l - 26);
                    }
                    else if (l < 'a')
                    {
                        l = (char)(l + 26);
                    }

                    result[i] = (l.ToString().ToUpper());
                }
                if (char.IsWhiteSpace(buffer[i]))
                {
                    result[i] = " ";
                }
            }
            return result;
        }

    
    public static void PrintError(string Err = "Unknown Failure", string Dbg = "",
                                    bool printUsage = true, bool exit = false, int exitVal = 1)
        {

            //1 Print out error message
            ConsoleColor currBackColor = Console.BackgroundColor;
            ConsoleColor currForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Error: " + Err);
            if (Dbg.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Error.WriteLine(Dbg);
            }
            if (printUsage)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                PrintUsage();
            }
            Console.ForegroundColor = currForeColor;
            Console.BackgroundColor = currBackColor;

            if (exit)
                Environment.Exit(exitVal); // By convention, exit with a value != 0 for error
        }

        //General usage message
        public static void PrintUsage()
        {
            Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " <n> <-e | -d> [ <str> | -f <filename>] \n" +
                   @"Performs rot-n encryption and decryption.
<n> specifies amount to rotate or de-rotate by.
-e specifies to encrypt (rotate by n)
-d specifies to decrypt (rotate by -n)
<str> is the string to rotate by, unless -f is instead specified
-f <filename> means perform the operation on the text file specified instead of a string.");
        }
    }
}
