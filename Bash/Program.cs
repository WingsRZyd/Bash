using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Bash
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            start();
        }

        public static void start()
        {
            string line;
            string[] args;
            int status = 1;
            int result = 0;
            bool flag = false;

            
            do
            {
                Console.Write("> ");
                line = Console.ReadLine();
                args = line.Split();

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "&&" || args[i] == "||" || args[i] == ";")
                    {
                        flag = true;
                        switch (args[1])
                        {
                            case "&&":
                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "pwd" || args[2] == "true" || args[2] == "false"))
                                {
                                    result = switchOneCondition(args,0);
                                    if (args[0] != "false")
                                    {
                                        result = switchOneCondition(args, 2);
                                    }
                                }

                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "cat" || args[2] == "echo" || args[2] == "wc"))
                                {
                                    result = switchOneCondition(args, 0);
                                    if (args[0] != "false")
                                    {
                                        switchTwoCondition(args, result, 2);
                                    }
                                }

                                break;

                            case "||":
                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "pwd" || args[2] == "true" || args[2] == "false"))
                                {
                                    result = switchOneCondition(args, 0);
                                    if (args[0] == "false")
                                    {
                                        result = switchOneCondition(args, 2);
                                    }
                                }

                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "cat" || args[2] == "echo" || args[2] == "wc"))
                                {
                                    result = switchOneCondition(args, 0);
                                    if (args[0] == "false")
                                    {
                                        switchTwoCondition(args, result, 2);
                                    }
                                }

                                break;

                            case ";":
                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "pwd" || args[2] == "true" || args[2] == "false"))
                                {
                                    result = switchOneCondition(args, 0);
                                    result = switchOneCondition(args, 2);
                                }

                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "cat" || args[2] == "echo" || args[2] == "wc"))
                                {
                                    result = switchOneCondition(args, 0);
                                    switchTwoCondition(args, result, 2);
                                }

                                break;
                        }

                        switch (args[2])
                        {
                            case "&&":
                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "pwd" || args[3] == "true" || args[3] == "false"))
                                {
                                    switchTwoCondition(args, result, 0);
                                    if (result == 0)
                                    {
                                        result = switchOneCondition(args, 3);
                                    }
                                }

                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "cat" || args[3] == "echo" || args[3] == "wc"))
                                {
                                    switchTwoCondition(args, result, 0);
                                    if (result == 0)
                                    {
                                        switchTwoCondition(args, result, 3);
                                    }
                                }

                                break;

                            case "||":
                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "pwd" || args[3] == "true" || args[3] == "false"))
                                {
                                    switchTwoCondition(args, result, 0);
                                    if (result != 0)
                                    {
                                        result = switchOneCondition(args, 3);
                                    }
                                }

                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "cat" || args[3] == "echo" || args[3] == "wc"))
                                {
                                    switchTwoCondition(args, result, 0);
                                    if (result != 0)
                                    {
                                        switchTwoCondition(args, result, 3);
                                    }
                                }

                                break;

                            case ";":
                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "pwd" || args[3] == "true" || args[3] == "false"))
                                {
                                    switchTwoCondition(args, result, 0);
                                    result = switchOneCondition(args, 3);
                                }

                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "cat" || args[3] == "echo" || args[3] == "wc"))
                                {
                                    switchTwoCondition(args, result, 0);
                                    switchTwoCondition(args, result, 3);
                                }

                                break;
                        }
                    }
                }

                if (flag == false)
                {
                    if (args.Length == 1)
                    {
                        result = switchOneCondition(args, 0);
                    }
                    else
                    {
                        result = switchTwoCondition(args, result, 0);
                    }
                }
            } while (status != 0);
        }

        public static int switchOneCondition(string[] args, int position)
        {
            int result;
            switch (args[position])
            {
                case "pwd":
                    pwd();
                    result = 0;
                    return result;
                    break;
                case "true":
                    result = 0;
                    return result;
                    break;
                case "false":
                    result = 1;
                    return result;
                    break;
            }
            return 0;
            
        }

        public static int switchTwoCondition(string[] args, int result1, int position)
        {
            int result = 0;
            switch (args[position])
            {
                case "cat":
                    result = cat(args[position+1], result);
                    return result;
                    break;
                case "echo":
                    if (args[position+1] == "$?")
                    {
                        prevResult(result1);
                        return result1;
                        break;
                    }
                    else
                    {
                        echo(args);
                        result = 0;
                        return result;
                        break;
                    }
                case "wc":
                    wc(args[position+1]);
                    return result;
                    break;
            }
            return result;}
        
        
        public string[] split_line(string line)
        {
            string[] split_line = line.Split(new char[] {' '});
            return split_line;
        }

        public static void pwd()
        {
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine(path);
        }

        public static int cat(string path, int result)
        {
            try
            {
                StreamReader reader = new StreamReader(path);
                string text = reader.ReadToEnd();
                Console.WriteLine(text);
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine($"Warning: {exception.Message}");
                result = 1;
                return result;
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Warning: {exception.Message}");
                result = 1;
                return result;
            }
            catch (NotSupportedException exception)
            {
                Console.WriteLine($"Warning: {exception.Message}");
                result = 1;
                return result;
            }
            return result;
        }

        public static void echo(string[] args)
        {
            for (int i = 1; i < args.Length - 1; i++)
            {
                Console.Write($"{args[i]} ");
            }

            Console.Write($"{args[args.Length - 1]} \n");
        }

        public static void prevResult(int result)
        {
            Console.WriteLine(result);
        }

        public static void wc(string path)
        {
            int count = File.ReadAllLines(path).Length;
            Console.WriteLine("Count of string:");
            Console.WriteLine(count);
            string s = "" ;
            string[] textMass;
            StreamReader sr = new StreamReader(path);
 
            while (sr.EndOfStream != true)
            {
                s += sr.ReadLine();                
            }
            textMass = s.Split(' ');
            Console.WriteLine("Количество слов:");
            Console.WriteLine(textMass.Length);
 
            sr.Close();
            
            long bat;
            bat = new FileInfo(path).Length;
            Console.WriteLine("count of bytes");
            Console.WriteLine(bat);

        }
    }
}