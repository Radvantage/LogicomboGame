using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: BattleGrid.cs
 * Description: Per-stage battlefield grid - contains data for player/computer units, objects.
 */
namespace Logicombo
{
    class BattleGrid
    {
        public int Width
        {
            get;
        }
        public int Height
        {
            get;
        }

        public String Name
        {
            get;
        }

        //Array grid to contain in-game units.
        private Units.BaseUnit[,] unitDataLayer;

        //Queue for preparing the entrance of computer and player-team units onto the grid, respectively.
        private Queue<Units.Com> comQueue;
        private Queue<Units.Human> humQueue;

        public List<int> ComXPositions;

        private int difficultyLevel;
        
        //Goal Property for the player-winning condition, a row for the player to reach with their units.
        private int _goalRow;
        public int Goal
        {
            get
            {
                return _goalRow;
            }
        }

        //Constructor for grid, set dimensions and player's goal row. Difficulty parameters: 3, 2, 1 corresponds to wall power level. Foliage parameters: 3 - 100%, 2 - 50%, 1 - 0% on entry points
        public BattleGrid(String levelName, int dimenX, int dimenY, int row, int difficulty, int foliage)
        {
            if (dimenX >= 3 && dimenY >= 3)
            {
                Name = levelName;
                Width = dimenX;
                Height = dimenY;
                
                if (row < dimenY)
                {
                    _goalRow = row;
                }
                else
                {
                    throw new IndexOutOfRangeException("Given row for goal completion is invalid - value is greater than or equal to game grid height!");
                }

                if (difficulty > 3 || difficulty < 1)
                {
                    throw new ArgumentOutOfRangeException("Given grid difficulty value is invalid - value is greater than 3 or less than 1, check parameter definitions!");
                }
                difficultyLevel = difficulty;

                comQueue = new Queue<Units.Com>(Width);
                humQueue = new Queue<Units.Human>(Width);
                ComXPositions = new List<int>();

                unitDataLayer = new Units.BaseUnit[Width, Height];

                if (foliage > 3 || foliage < 1)
                {
                    throw new ArgumentOutOfRangeException("Given foliage density value is invalid - value is greater than 3 or less than 1, check parameter definitions!");
                }

                for (int i = 0; i < Width; i++)
                {
                    unitDataLayer[i, Height - 1] = new Units.Wall(i, Height - 1, difficultyLevel);

                    //Distribute possible entry point bridges for queueing new Computer enemies. Simultaneous manage foliage random distribution, east/west push based on respective east/west column.
                    if (i%difficultyLevel == 0)
                    {
                        //Establishing attributes to push to foliage candidates during generation. Initialize before conditionals to avoid repetitious code.
                        int foliageY = Program.r.Next(Goal);
                        bool eastPush;
                        if (i + 1 < Width / 2)
                        {
                            eastPush = false;
                        }
                        else
                        {
                            eastPush = true;
                        }

                        //Distribute entry points and foliage on grid based on respective value definitions.
                        if (difficultyLevel > 1)
                        {
                            if (i > 0 && i < Width)
                            {
                                ComXPositions.Add(i);
                            }

                            if (foliage == 3)
                            {
                                unitDataLayer[i, foliageY] = new Units.Foliage(i, foliageY, eastPush);
                            }
                            else if (foliage == 2)
                            {
                                if (Program.r.Next(2) == 1)
                                {
                                    unitDataLayer[i, foliageY] = new Units.Foliage(i, foliageY, eastPush);
                                }
                            }
                        }
                        else
                        {
                            if ((i != 0 && i != Width - 1) && i != (Width/2))
                            {
                                if (i > 0 && i < Width)
                                {
                                    ComXPositions.Add(i);
                                }
                                
                                if (foliage == 3)
                                {
                                    unitDataLayer[i, foliageY] = new Units.Foliage(i, foliageY, eastPush);
                                }
                                else if (foliage == 2)
                                {
                                    if (Program.r.Next(2) == 1)
                                    {
                                        unitDataLayer[i, foliageY] = new Units.Foliage(i, foliageY, eastPush);
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            else
            {
                throw new ArgumentOutOfRangeException("Given values for game grid dimensions are invalid - X and/or Y coordinate(s) are less than 3!");
            }
        }

        //Enqueue items to respective positions on the battlefield - validate coordinates exist within grid boundaries.
        public void EnqueueCom()
        {
            int position = Program.r.Next(ComXPositions.Count);
            Units.Com c = new Units.Com(ComXPositions[position], 4 - difficultyLevel);
            comQueue.Enqueue(c);
        }
        public void EnqueueHum(Units.Human h)
        {
            if (h.Position.Item1 < unitDataLayer.GetLength(0) && h.Position.Item2 < unitDataLayer.GetLength(1))
            {
                humQueue.Enqueue(h);
            }
            else
            {
                throw new IndexOutOfRangeException("Given .Position value of enqueued Human unit instance is invalid - X and/or Y coordinate(s) exist beyond game grid dimensions!");
            }
        }

        public virtual Units.Com PeekCom()
        {
            return comQueue.Peek();
        }
        public virtual Units.Human PeekHum()
        {
            return humQueue.Peek();
        }

        public Units.Com DequeueCom()
        {
            return comQueue.Dequeue();
        }
        public Units.Human DequeueHum()
        {
            return humQueue.Dequeue();
        }

        public Units.BaseUnit GetUnitAt(int xPos, int yPos)
        {
            return unitDataLayer[xPos, yPos];
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}