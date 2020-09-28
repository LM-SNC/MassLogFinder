namespace MassLogFinder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="method">The method<see cref="String"/>.</param>
        /// <param name="path">The path<see cref="String"/>.</param>
        public void getFiles(String method, String path)
        {
            Console.WriteLine("You choose: " + method + "\n");

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            List<String> fileNameList = new List<String>();

            foreach (var file in directoryInfo.GetFiles()) //проходим по файлам
            {
                //получаем расширение файла и проверяем подходит ли оно нам 
                //File.Move(file.FullName, Path.ChangeExtension(file.FullName, ".txt"));

                if (Path.GetExtension(file.FullName) == ".log" || Path.GetExtension(file.FullName) == ".txt")
                {
                    fileNameList.Add((file.Name));
                }
            }

            int count = 0;
            foreach (string name in fileNameList) //проходим по файлам
            {
                count++;
                Console.WriteLine(count + "." + " " + name);

            }

            string desiredString;
            Console.WriteLine("\n" + "Send your desired string:" + "\n");
            desiredString = Console.ReadLine();


            for (int i = 0; i < fileNameList.Count; i++)
            {
                StreamReader f = new StreamReader(path + "/" + fileNameList[i]);
                int line = 0;
                bool foundBool = false;

                while (!f.EndOfStream)
                {
                    line++;
                    string s = f.ReadLine();
                    //Console.WriteLine(s);

                    if (s.Contains(desiredString))
                    {
                        Console.WriteLine("\n" + "\n" + "[" + fileNameList[Convert.ToInt32(i)] + "]" + "\n");
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

        /// <summary>
        /// The getallFiles.
        /// </summary>
        /// <param name="disk">The disk<see cref="String"/>.</param>
        public void getallFiles(String disk)
        {
            /*for (int i = 0; i < 601; i++)
            {
                float prc = ((float)i / 600) * 100;
                if ((int)prc % 10 == 0)
                {
                    Console.WriteLine((int)prc + "%");
                    Console.Clear();
                    Thread.Sleep(100);
                }
            }
            */

            string desiredString;
            Console.WriteLine("\n" + "Send your desired string:" + "\n");
            desiredString = Console.ReadLine();

            string[] subdirs = Directory.GetDirectories(disk);
            int mainDirectory = 0;
            int count = 0;
            string[] allfiles = null;
            List<String> fileNameList = new List<String>();
            foreach (var fileMain in subdirs) //проходим по файлам
            {
                count = 0;
                try
                {
                    allfiles = Directory.GetFiles(fileMain, "*.*", SearchOption.AllDirectories);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Access error: " + fileMain);
                    Console.ReadLine();
                    continue;
                }
                foreach (var file in allfiles) //проходим по файлам
                {
                    if (Path.GetExtension(file) == ".log" || Path.GetExtension(file) == ".txt")
                    {

                        StreamReader g = new StreamReader(file);
                        while (!g.EndOfStream)
                        {
                            //Console.Clear();
                            string s = g.ReadLine();
                            //Console.WriteLine(s);
                            Console.WriteLine(mainDirectory + "/" + subdirs.Length + "-- " + count + "/" + allfiles.Length + "--size: " + file.Length);

                            if (s.Contains(desiredString))
                            {
                                fileNameList.Add((file));
                            }
                        }
                        g.Close();
                        //Console.WriteLine(file);
                    }
                    count++;

                }
                mainDirectory++;
            }
            foreach (var file in fileNameList)
            {
                StreamReader f = new StreamReader(file);
                int line = 0;
                bool foundBool = false;

                while (!f.EndOfStream)
                {
                    line++;
                    string s = f.ReadLine();
                    //Console.WriteLine(s);

                    if (s.Contains(desiredString))
                    {
                        Console.WriteLine("\n" + "\n" + "[" + file + "]" + "\n");
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

        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        internal static void Main(string[] args)
        {
            //Program program = new Program();
            //program.getallFiles();


            Program program = new Program();
            List<String> diskList = new List<String>();

            while (true)
            {
                Console.WriteLine("Select your search area:" + "\n" + "1. Program directory" + "\n" + "2. All files" + "\n" + "3. Custom path" + "\n");

                String choose = Console.ReadLine();
                if (Regex.IsMatch(choose, @"^[0-9]+$"))
                {
                    int conChoose = Convert.ToInt32(choose);

                    if (conChoose == 1)
                    {
                        program.getFiles("Program directory", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));


                    }
                    else if (conChoose == 2)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("You choose: All Files" + "\n" + "\n");
                            Console.WriteLine("Select your disk:");
                            string[] Drives = Environment.GetLogicalDrives();
                            int count = 0;
                            foreach (string s in Drives)
                            {
                                count++;
                                Console.WriteLine(count + ". " + s);
                                diskList.Add(s);
                            }
                            Console.WriteLine("\n");

                            choose = Console.ReadLine();
                            if (Regex.IsMatch(choose, @"^[0-9]+$"))
                            {
                                conChoose = Convert.ToInt32(choose);
                                if (conChoose < 1 || conChoose > count)
                                {
                                    Console.WriteLine("The argument must be at least 1 and at most " + count + "\n");
                                    Console.ReadLine();
                                    Console.Clear();
                                    continue;
                                }
                                program.getallFiles(diskList[conChoose - 1]);
                            }
                            else
                            {
                                Console.WriteLine("Only numbers must be used as an argument!");
                                Console.ReadLine();
                                Console.Clear();
                                continue;
                            }
                        }
                    }
                    else if (conChoose == 3)
                    {
                        string customPath;
                        while (true)
                        {
                            Console.WriteLine("Send your path: ");
                            customPath = Console.ReadLine();

                            try
                            {
                                DirectoryInfo dir = new DirectoryInfo(customPath);

                                if (!dir.Exists)
                                {
                                    Console.WriteLine("Invalid path!" + "\n");
                                    continue;
                                }
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Invalid path!" + "\n");
                                continue;
                            }
                            break;
                        }
                        program.getFiles("Custom path", customPath);
                    }
                }
                else
                {
                    Console.WriteLine("Only numbers must be used as an argument!");
                    continue;
                }

                Console.WriteLine("\n" + "\n" + "Send enter for continue");
                Console.ReadLine();
                Console.Clear();
                continue;
            }
        }
    }
}
