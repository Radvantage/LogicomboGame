using Microsoft.VisualBasic;
using System;
using System.Threading;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Program.cs
 * Description: Execute game intialization and control loop.
 */
namespace Logicombo
{
    class Program
    {
        //Initialize Game Attributes per Session
        private static int gX = 11;
        private static int gY = 6;
        private static int goal = 3;
        private static int difficulty = 1;
        private static int foliage = 3;
        private static bool gameSet = false;
        private static char inputC;

        public static Random r = new Random();

        private static void WaitForKey()
        {
            Console.Write("\n>>Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        //Helper method - extract first character from string input or empty
        private static char ValidCharInput()
        {
            string s = Console.ReadLine().ToString();
            if (s.Length > 0)
            {
                return s[0];
            }
            else
            {
                return ' ';
            }
        }

        //Helper method - Intro Section
        private static void WelcomeScreen()
        {
            Console.Clear();
            //Game Initialization
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#=================================#");
            Console.WriteLine("#------------LOGICOMBO------------#");
            Console.WriteLine("#<<<<Program of Logic & Combat>>>>#");
            Console.WriteLine("#=====Developed by K Hanninen=====#");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Beep(80, 200);

            WaitForKey();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#=================================#");
            Console.WriteLine("#-----------GAME CONFIG-----------#");
            Console.WriteLine("#=================================#\n");
        }

        //Configuration helper - Game Battle-Grid Size Selector & Validator
        private static void ConfigureSize()
        {
            bool sizeValid = false;
            while (!sizeValid)
            {
                //Inform selection until user submits valid dimensions tag
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(">>BATTLE-GRID SIZE");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(">>[c]ustom = (??x??)");
                Console.WriteLine(">>[s]mall  = (05x08)");
                Console.WriteLine(">>[m]edium = (07x10)");
                Console.WriteLine(">>[l]arge  = (09x12)");

                Console.Write("<<Input: ");

                //Data Validation Variables (Reading Input + Custom Dimensions)
                bool validX = false;
                bool validY = false;

                inputC = ValidCharInput();

                switch (inputC)
                {
                    case ('s'):
                        sizeValid = true;
                        gX = 5;
                        gY = 8;
                        goal = 3;
                        break;
                    case ('m'):
                        sizeValid = true;
                        gX = 7;
                        gY = 10;
                        goal = 4;
                        break;
                    case ('l'):
                        sizeValid = true;
                        gX = 9;
                        gY = 12;
                        goal = 5;
                        break;
                    case ('c'): //verify custom dimensions for game space
                        Console.WriteLine("\n>>CUSTOM BATTLE-GRID SIZE");
                        while (!validX)
                        {
                            Console.Write("<<Grid Width (4-24): ");
                            string xString = Console.ReadLine();
                            int temp;

                            if (int.TryParse(xString, out _))
                            {
                                temp = Convert.ToInt32(xString);
                            }
                            else
                            {
                                temp = 999;
                            }

                            if (temp >= 4 && temp <= 24)
                            {
                                gX = temp;
                                validX = true;
                            }
                            else
                            {
                                //Notify error - input width denied
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n>>Grid width '" + temp + "' is invalid or out-of-bounds. Try again.");
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.Beep(500, 250);
                            }
                        }
                        while (!validY)
                        {
                            Console.Write("<<Grid Length (4-24): ");
                            string yString = Console.ReadLine();
                            int temp;

                            if (int.TryParse(yString, out _))
                            {
                                temp = Convert.ToInt32(yString);
                            }
                            else
                            {
                                temp = 999;
                            }

                            if (temp >= 4 && temp <= 24)
                            {
                                gY = temp;
                                goal = temp / 2;
                                validY = true;
                            }
                            else
                            {
                                //Notify error - input length denied
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n>>Grid length '" + temp + "' is invalid or out-of-bounds. Try again.");
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.Beep(500, 250);
                            }
                        }
                        sizeValid = true;
                        break;
                    default:
                        //Notify error - wrong tag
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(">>Selection '" + inputC + "' invalid (c/s/m/l). Try again.\n");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Beep(500, 250);
                        break;
                }

                ConfirmSelection(ref sizeValid, (gX.ToString("D2") + "x" + gY.ToString("D2") + " GRID"));
            }
        }

        //Configuration helper - Game Difficulty Mode Selector & Validator
        private static void ConfigureMode()
        {
            bool modeValid = false;
            while (!modeValid)
            {
                //Inform selection until user submits valid mode tag
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(">>DIFFICULTY MODE");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(">>[n]ormal");
                Console.WriteLine(">>[e]xpert");
                Console.WriteLine(">>[w]arlord");

                Console.Write("<<Input: ");

                inputC = ValidCharInput();

                string modeName = "?";
                switch (inputC)
                {
                    case ('n'):
                        difficulty = 3;
                        foliage = 1;
                        modeValid = true;
                        modeName = "NORMAL";
                        break;
                    case ('e'):
                        difficulty = 2;
                        foliage = 2;
                        modeValid = true;
                        modeName = "EXPERT";
                        break;
                    case ('w'):
                        difficulty = 1;
                        foliage = 3;
                        modeValid = true;
                        modeName = "WARLORD";
                        break;
                    default:
                        //Notify error - wrong tag
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(">>Selection '" + inputC + "' invalid (n/e/w). Try again.\n");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Beep(500, 250);
                        break;
                }

                ConfirmSelection(ref modeValid, modeName + " MODE");
            }
        }

        //Confirmation helper method - use in configure methods to verify selection before proceeding
        private static void ConfirmSelection(ref bool valid, string name)
        {
            //Confirm selected parameters, set valid flag to false when denied
            bool confirmation = false;
            while (valid && !confirmation)
            {
                Console.WriteLine("\n>>Game is set to '" + name + "' ... Are you sure? (y/n)");
                Console.Write("<<Input: ");
                inputC = ValidCharInput();

                switch (inputC)
                {
                    case 'y':
                        confirmation = true;
                        break;
                    case 'n':
                        confirmation = true;
                        valid = false;
                        Console.WriteLine();
                        break;
                    default:
                        //Notify error - cannot confirm
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(">>Selection '" + inputC + "' invalid. Try again.\n");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Beep(500, 250);
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            //Accept game init settings until validated and confirmed
            while (!gameSet)
            {
                WelcomeScreen();

                ConfigureSize();

                Console.WriteLine();

                ConfigureMode();

                WaitForKey();
                gameSet = true;
            }

            BattleGrid bg = new BattleGrid("2022", gX, gY, goal, difficulty, foliage);
            bg.EnqueueCom();
            View v = new View(bg);

            Console.Clear();
            Console.Beep(200, 500);

            int turnCounter = 0;
            while (true)
            {
                v.Draw();

                Thread.Sleep(450);
                turnCounter++;

                if (turnCounter%2 == 0)
                {
                    Console.Beep(200, 50);
                    bg.MarchCom();
                    bg.DequeueCom();
                    bg.EnqueueCom();
                }
                
                
            }
            Console.ReadKey();
        }
    }
}