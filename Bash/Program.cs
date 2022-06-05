using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml;
using Microsoft.SqlServer.Server;

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
            string[] list = new string[1024];
            Dictionary<string, string> variables = new Dictionary<string, string>();


            do
            {
                Console.Write("> ");
                line = Console.ReadLine();
                args = line.Split();
                run(args, flag, result, list, variables);
            } while (status != 0);
        }

        public static void run(string[] args, bool flag, int result, string[] list, Dictionary<string, string> variables)
        {for (int i = 0; i < args.Length; i++)
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
                                    result = switchOneCondition(args,0, list, variables);
                                    if (args[0] != "false")
                                    {
                                        result = switchOneCondition(args, 2, list, variables);
                                    }
                                }

                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "cat" || args[2] == "echo" || args[2] == "wc"))
                                {
                                    result = switchOneCondition(args, 0, list, variables);
                                    if (args[0] != "false")
                                    {
                                        switchTwoCondition(args, result, 2, variables, list, flag);
                                    }
                                }

                                break;

                            case "||":
                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "pwd" || args[2] == "true" || args[2] == "false"))
                                {
                                    result = switchOneCondition(args, 0, list, variables);
                                    if (args[0] == "false")
                                    {
                                        result = switchOneCondition(args, 2, list, variables);
                                    }
                                }

                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "cat" || args[2] == "echo" || args[2] == "wc"))
                                {
                                    result = switchOneCondition(args, 0, list, variables);
                                    if (args[0] == "false")
                                    {
                                        switchTwoCondition(args, result, 2, variables, list, flag);
                                    }
                                }

                                break;

                            case ";":
                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "pwd" || args[2] == "true" || args[2] == "false"))
                                {
                                    result = switchOneCondition(args, 0, list, variables);
                                    result = switchOneCondition(args, 2, list, variables);
                                }

                                if ((args[0] == "pwd" || args[0] == "true" || args[0] == "false") &&
                                    (args[2] == "cat" || args[2] == "echo" || args[2] == "wc"))
                                {
                                    result = switchOneCondition(args, 0, list, variables);
                                    switchTwoCondition(args, result, 2, variables, list, flag);
                                }

                                break;
                        }

                        switch (args[2])
                        {
                            case "&&":
                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "pwd" || args[3] == "true" || args[3] == "false"))
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    if (result == 0)
                                    {
                                        result = switchOneCondition(args, 3, list, variables);
                                    }
                                }

                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "cat" || args[3] == "echo" || args[3] == "wc"))
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    if (result == 0)
                                    {
                                        switchTwoCondition(args, result, 3, variables, list, flag);
                                    }
                                }

                                break;

                            case "||":
                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "pwd" || args[3] == "true" || args[3] == "false"))
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    if (result != 0)
                                    {
                                        result = switchOneCondition(args, 3, list, variables);
                                    }
                                }

                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "cat" || args[3] == "echo" || args[3] == "wc"))
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    if (result != 0)
                                    {
                                        switchTwoCondition(args, result, 3, variables, list, flag);
                                    }
                                }

                                break;

                            case ";":
                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "pwd" || args[3] == "true" || args[3] == "false"))
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    result = switchOneCondition(args, 3, list, variables);
                                }

                                if ((args[0] == "cat" || args[0] == "echo" || args[0] == "wc") &&
                                    (args[3] == "cat" || args[3] == "echo" || args[3] == "wc"))
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    switchTwoCondition(args, result, 3, variables, list, flag);
                                }

                                break;
                        }

                        switch (args[4])
                        {
                            case "&&":
                                if (args[5] == "pwd" || args[5] == "true" || args[5] == "false")
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    if (result == 0)
                                    {
                                        result = switchOneCondition(args, 5, list, variables);
                                    }
                                }
                                
                                if (args[5] == "echo" || args[5] == "cat" || args[5] == "wc")
                                {
                                    switchTwoCondition(args, result, 0, variables, list, flag);
                                    if (result == 0)
                                    {
                                        result = switchTwoCondition(args, result, 5, variables, list, flag);
                                    }
                                }

                                break;
                        }
                        
                    }
                }

                if (flag == false)
                {
                    if (args.Length == 1)
                    {
                        result = switchOneCondition(args, 0, list, variables);
                    }
                    else
                    {
                        result = switchTwoCondition(args, result, 0, variables, list, flag);
                    }
                }
        }
        

        public static int switchOneCondition(string[] args, int position, string[] list, Dictionary<string, string> variables)
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

            if (args[0][0] == '$')
            {
                localVariables(list, args, variables);
            }
            return 0;
            
        }

        public static int switchTwoCondition(string[] args, int result1, int position, Dictionary<string, string> variables, string[] list, bool flag)
        {
            int result = 0;
            switch (args[position])
            {
                case "script":
                    script(args[position+1], result, list, flag, variables);
                    Console.WriteLine(111);
                    break;
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
                    else if (args[position+1][0] == '$')
                    {
                        string name = getNameWithoutDollar(args[position + 1]);
                        try
                        {
                            Console.WriteLine(variables[name]);
                        }
                        catch (KeyNotFoundException e)
                        {
                            Console.WriteLine($"Warning: {e.Message}");
                            result = 1;
                            return result;
                        }
                        
                        return result;
                    }
                    else if (args[position + 2] == ">>")
                    {
                        arrowRightEcho(args, position, true);
                        return result;
                    }
                    else if (args[position + 2][0] == '>')
                    {
                        arrowRightEcho(args, position, false);
                        return result;
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

        public static string pwd()
        {
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine(path);
            return path;
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

        public static Dictionary<string, string> localVariables(string[] list, string[] args, Dictionary<string, string> variables)
        {
            string elem = "";
            string name = "";
            bool flag = true;
            for (int i = 1; i < args[0].Length - 1; i++)
            {
                if (args[0][i] != '=' && flag == true)
                {
                    name = name.Insert(name.Length, args[0][i].ToString());
                }
                else
                {
                    flag = false;
                }

                if (flag == false)
                {
                    elem = elem.Insert(elem.Length, args[0][i + 1].ToString());
                }
                
            }
            variables.Add(name, elem);
            return variables;
        }

        public static string getNameWithoutDollar(string arg)
        {
            string name = "";
            for (int i = 1; i < arg.Length; i++)
            {
                if (arg[i] != '=')
                {
                    name = name.Insert(name.Length, arg[i].ToString());
                }
                else
                {
                    break;
                }
            }

            return name;
        }
        
        public static string getName(string arg)
        {
            string name = "";
            for (int i = 0; i < arg.Length; i++)
            {
                if (arg[i] != '=')
                {
                    name = name.Insert(name.Length, arg[i].ToString());
                }
                else
                {
                    break;
                }
            }

            return name;
        }

        public static void arrowRight(string[] args, int position, bool flag, string name)
        {
            string halfPath = getName(args[position + 3]);
            using (StreamWriter writer = new StreamWriter(halfPath, flag))
            {
                writer.WriteLine(name);
                Console.WriteLine(name);
            }
       }

        public static void arrowRightEcho(string[] args, int position, bool flag)
        {
            string name = getName(args[position + 1]);
            arrowRight(args, position, flag, name);
        }

        public static void script(string path, int result, string[] list, bool flag, Dictionary<string, string> variables)
        {
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string text = reader.ReadLine();
                Console.WriteLine(text);
                string[] arg = text.Split();
                run(arg, flag, result, list, variables);
            }
        }
    }
}