using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Bash
{
    internal class Program
    {

        const int size = 1024;

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
            bool flag;





            do
            {
                
                Console.Write("> ");
                line = Console.ReadLine();
                args = line.Split();
                /*for (int i = 0; i < args.Length; i++)
                {
                    Console.Write(args[i], "\t");
                }*/

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "&&" || args[i] == "||" || args[i] == ";")
                    {
                        flag = true;
                        switch (args[1])
                        {
                            case "&&":
                                switch (args[0])
                                {
                                    case "pwd":
                                        pwd();
                                        result = 0;
                                        break;
                                    case "true":
                                        result = 0;
                                        break;
                                    case "false":
                                        result = 1;
                                        break;
                                }

                                if (args[0] != "false")
                                {
                                    switch (args[2])
                                    {
                                        case "cat":
                                            cat(args[3], result);
                                            result = cat(args[3], result);
                                            break;
                                        case "echo":
                                            if (args[3] == "$?")
                                            {
                                                prevResult(result);
                                                break;
                                            }
                                            else
                                            {
                                                echo(args);
                                                result = 0;
                                                break;
                                            }
                                        case "wc":
                                            wc(args[3]);
                                            break;
                                    }
                                }
                                break;
                            case "||":
                                    
                                
                        }
                    }
            }
                if (args.Length == 1)
                {
                    switch (line)
                    {
                        case "pwd":
                            pwd();
                            result = 0;
                            break;
                        case "true":
                            result = 0;
                            break;
                        case "false":
                            result = 1;
                            break;
                        /*case "$?":
                            prevResult(result);
                            break;*/
                    }
                }
                else
                {
                    switch (args[0])
                    {
                        case "cat":
                            cat(args[1], result);
                            result = cat(args[1], result);
                            break;
                        case "echo":
                            if (args[1] == "$?")
                            {
                                prevResult(result);
                                break;
                            }
                            else
                            {
                                echo(args);
                                result = 0;
                                break;
                            }
                        case "wc":
                            wc(args[1]);
                            break;
                            
                    }
                }

                //Console.WriteLine(line);
                //Console.WriteLine(args.Length);
                //status = execute(args);
            } while (status != 0);
        }



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

        public static void connectors()
        {
            
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