

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MassLogFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Your directory: " + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                Console.WriteLine("Choose file:");

                DirectoryInfo directoryInfo = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                List<Bitmap> myList = new List<Bitmap>(); //ваш лист с Bitmap
                List<String> fileNameList = new List<String>();

                int count = 0;
                foreach (var file in directoryInfo.GetFiles()) //проходим по файлам
                {
                    //получаем расширение файла и проверяем подходит ли оно нам 
                    //File.Move(file.FullName, Path.ChangeExtension(file.FullName, ".txt"));

                    if (Path.GetExtension(file.FullName) == ".log" || Path.GetExtension(file.FullName) == ".txt")
                    {
                        myList.Add(new Bitmap(file.FullName)); //если расширение подошло, создаём Bitmap
                        fileNameList.Add((file.Name));
                    }
                }

                foreach (string name in fileNameList) //проходим по файлам
                {
                    Console.WriteLine(count + " " + name);
                    count++;
                }
                Console.WriteLine(count + " all files" + "\n");

                while (true)
                {
                    String choose = Console.ReadLine();
                    if (Regex.IsMatch(choose, @"^[0-9]+$"))
                    {
                        int conChoose = Convert.ToInt32(choose);
                        if (conChoose > myList.Count)
                        {
                            Console.WriteLine("You have exceeded the file limit" + "\n");
                            continue;
                        }

                        if (conChoose.Equals(count))
                        {
                            Console.WriteLine("You choose: all files" + "\n");

                            string desiredString;
                            Console.WriteLine("Send your desired string:" + "\n");
                            desiredString = Console.ReadLine();

                            for (int i = 0; i < myList.Count; i++)
                            {
                                Console.WriteLine("\n" + "\n" + "[" + fileNameList[Convert.ToInt32(i)] + "]" + "\n");
                                StreamReader f = new StreamReader(fileNameList[Convert.ToInt32(i)]);
                                int line = 0;
                                bool foundBool = false;

                                while (!f.EndOfStream)
                                {
                                    line++;
                                    string s = f.ReadLine();
                                    //Console.WriteLine(s);

                                    if (s.Contains(desiredString))
                                    {
                                        Console.WriteLine("Matches found: " + s + "\n" + "line: " + line);
                                        foundBool = true;
                                    }
                                }

                                if (!foundBool)
                                {
                                    Console.WriteLine("Matches not found!");
                                }

                                f.Close();
                            }
                        }
                        else
                        {
                            Console.WriteLine("You choose: " + fileNameList[conChoose] + "\n");

                            string desiredString;
                            Console.WriteLine("Send your desired string:" + "\n");
                            desiredString = Console.ReadLine();

                            Console.WriteLine("\n" + "\n" + "[" + fileNameList[conChoose] + "]" + "\n");
                            StreamReader f = new StreamReader(fileNameList[conChoose]);
                            int line = 0;
                            bool foundBool = false;

                            while (!f.EndOfStream)
                            {
                                line++;
                                string s = f.ReadLine();
                                //Console.WriteLine(s);

                                if (s.Contains(desiredString))
                                {
                                    Console.WriteLine("Matches found: " + s + "\n" + "line: " + line);
                                    foundBool = true;
                                }
                            }

                            if (!foundBool)
                            {
                                Console.WriteLine("Matches not found!");
                            }

                            f.Close();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Only numbers must be used as an argument!");
                        continue;
                    }
                    break;
                }
                Console.WriteLine("\n" + "\n" + "Send enter for continue");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
