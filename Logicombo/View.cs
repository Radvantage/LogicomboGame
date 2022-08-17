using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: View.cs
 * Description: View component of Logicombo MVC, draw BattleGrid assuming frame-by-frame updates to gameplay. Facilitate interaction and live changes to associated units/collections with ASCII-based graphics.
 */
namespace Logicombo
{
    class View
    {
        private BattleGrid gridMap;

        //Scrolling Title Elements, Text & Color... Iterators.
        private static List<char> scrollTitle = new List<char> { ' ', ' ', 'L', ' ', 'O', ' ', 'G', ' ', 'I', ' ', 'C', ' ', 'O', ' ', 'M', ' ', 'B', ' ', 'O', ' ', ' ' };
        private ConsoleColor[] scrollColorsF = new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Blue };
        private ConsoleColor[] scrollColorsB = new ConsoleColor[] { ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.DarkGray };
        private int scrollI = scrollTitle.Count - 1;
        private int colorI = 0;

        //Constructor, accepts reference to current BattleGrid instance to display by applying properties and methods of said class.
        public View(BattleGrid map)
        {
            gridMap = map;
            scrollTitle.AddRange(map.Name.ToCharArray());
        }

        //Draw method, displays formatted, visual representation of BattleGrid instance with Unit String formats. Animates title, applies color to relevant foreground/background ASCII-graphic.
        public void Draw()
        {
            //Update per frame, redraw from origin without visible cursor to prevent flickering.
            CursorVisible = false;
            SetCursorPosition(0, 0);

            //Draw title with scrolling effect dependent on thread update frequency, shift position/colors per frame using array iterators.
            Write("(___(");
            for (int c = 0; c < gridMap.Width*5; c++)
            {
                if (scrollTitle[(scrollI + c) % scrollTitle.Count] != ' ')
                {
                    ForegroundColor = scrollColorsF[colorI];
                    Write(scrollTitle[(scrollI + c)%scrollTitle.Count]);
                    ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    ForegroundColor = scrollColorsB[colorI];
                    Write("~");
                    ForegroundColor = ConsoleColor.White;
                }
            }
            scrollI++;
            colorI++;
            if (colorI > scrollColorsF.Length - 1)
            {
                colorI = 0;
            }
            WriteLine(")___)");

            //Separate between each row for additional visual clarity. Repeat dashed line across width when processing grid graphics.
            for (int b = 0; b < gridMap.Width + 2; b++)
            {
                Write("-----");
            }

            //Computer "Wickling" queue indication sideline icon.
            WriteLine();
            ForegroundColor = ConsoleColor.DarkRed;
            Write("++");
            ForegroundColor = ConsoleColor.Red;
            Write("W");
            ForegroundColor = ConsoleColor.DarkRed;
            Write("++");
            ForegroundColor = ConsoleColor.White;

            //Draw next Computer unit "Wickling" by peeking at grid's computer player queue. Draw empty enemy territory otherwise.
            int comColumn = gridMap.PeekCom().Position.Item1;
            for (int a = 0; a < gridMap.Width; a++)
            {
                Write("[");
                if (a == comColumn)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = gridMap.PeekCom().Color;
                    Write(gridMap.PeekCom().ToString());
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkRed;
                    Write("///");
                }
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Black;
                Write("]");
            }
            ForegroundColor = ConsoleColor.DarkRed;
            Write("++");
            ForegroundColor = ConsoleColor.Red;
            Write("W");
            ForegroundColor = ConsoleColor.DarkRed;
            Write("++");
            ForegroundColor = ConsoleColor.White;
            WriteLine();

            //River separator graphic before proper grid, draw crossing bridges for valid entry points so player can predict potential areas where enemies might be queued against their defenses.
            for (int b = 0; b < gridMap.Width + 2; b++)
            {
                Write("-----");
            }
            WriteLine();
            Write("CROSS");
            for (int a = 0; a < gridMap.Width; a++)
            {
                Write("[");
                if (!gridMap.ComXPositions.Contains(a))
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    Write("~~~");
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkRed;
                    Write("///");
                }
                ForegroundColor = ConsoleColor.White;
                Write("]");
            }
            Write("CROSS");
            WriteLine();
            for (int a = 0; a < gridMap.Width + 2; a++)
            {
                Write("-----");
            }
            WriteLine();

            //Draw grid, check for row that is the goal, draw units or empty battlefield territory otherwise according to respective color associations. Use solid backgrounds for well-defined units.
            for (int j = 0; j < gridMap.Height; j++)
            {
                if (j == gridMap.Goal)
                {
                    ForegroundColor = ConsoleColor.DarkYellow;
                    Write("##");
                    ForegroundColor = ConsoleColor.Yellow;
                    Write("G");
                    ForegroundColor = ConsoleColor.DarkYellow;
                    Write("##");
                    ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Write("=" + (gridMap.Height - j).ToString("000") + "=");
                }
                
                for (int i = 0; i < gridMap.Width; i++)
                {
                    Units.BaseUnit u = gridMap.GetUnitAt(i, j);
                    Write("[");

                    if (u == null)
                    {
                        ForegroundColor = ConsoleColor.DarkGreen;
                        Write("...");
                    }
                    else if (u is Units.Foliage) {
                        ForegroundColor = u.Color;
                        Write(u.ToString());
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Black;
                        BackgroundColor = u.Color;
                        Write(u.ToString());
                    }

                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                    Write("]");
                }
                if (j == gridMap.Goal)
                {
                    //Goal sidelines along the row so player knows how far to progress their wall.
                    ForegroundColor = ConsoleColor.DarkYellow;
                    Write("##");
                    ForegroundColor = ConsoleColor.Yellow;
                    Write("G");
                    ForegroundColor = ConsoleColor.DarkYellow;
                    Write("##");
                    ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    //Number sidelines for grid size indication.
                    Write("=" + (gridMap.Height - j).ToString("000") + "=");
                }
                WriteLine();
                for (int a = 0; a < gridMap.Width + 2; a++)
                {
                    Write("-----");
                }
                WriteLine();

            }
        }
    }
}