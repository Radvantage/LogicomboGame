﻿using Logicombo.Units;
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
        private bool _gameOver = false;
        public bool GameOver
        {
            get
            {
                return _gameOver;
            }
        }
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
                        //Distribute entry points and foliage on grid based on respective value definitions.
                        if (difficultyLevel > 1)
                        {
                            if (i > 0 && i < Width)
                            {
                                ComXPositions.Add(i);

                                GenerateFoliage(i, foliage);
                            }
                        }
                        else
                        {
                            if ((i != 0 && i != Width - 1) && i != (Width/2))
                            {
                                if (i > 0 && i < Width)
                                {
                                    ComXPositions.Add(i);

                                    GenerateFoliage(i, foliage);
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

        //Create foliage along a specified row, given a density parameter
        public void GenerateFoliage(int x, int density)
        {
            int foliageY = Program.r.Next(1, Goal);
            if (Program.r.Next(density) > 0 && unitDataLayer[x, foliageY] == null)
            {
                unitDataLayer[x, foliageY] = new Units.Foliage(x, foliageY);
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

        public void DequeueCom()
        {
            Units.Com c = comQueue.Dequeue();
            Units.BaseUnit temp = unitDataLayer[c.Position.Item1, c.Position.Item2];
            if (temp == null)
            {
                unitDataLayer[c.Position.Item1, c.Position.Item2] = c;
            }
            else
            {
                if (temp is Units.Foliage || temp is Units.Human)
                {
                    //TODO: Decide how to distribute Com units when colliding with foliage
                    unitDataLayer[c.Position.Item1, c.Position.Item2].Damage();
                    unitDataLayer[c.Position.Item1, c.Position.Item2].Damage();
                }
                else
                {
                    CombineCom(c.Position.Item1, c.Position.Item2, temp);
                }
            }
        }

        public void DequeueHum()
        {
            Units.Human h = humQueue.Dequeue();
            
            
        }

        //Helper method - manage game rules for combining like (Com) units; max power is 3
        private void CombineCom(int x, int y, Units.BaseUnit t)
        {
            if (t is Units.Com && t.Power < 3)
            {
                unitDataLayer[x, y].Damage();

                Units.Com ally = (Units.Com)t;
                ally.Combine();
            }
        }

        public void MarchCom()
        {
            for (int i = 0; i < unitDataLayer.GetLength(0); i++)
            {
                for(int j = unitDataLayer.GetLength(1) - 1; j >= 0; j--)
                {
                    if (unitDataLayer[i, j] != null && unitDataLayer[i, j].Power == 0)
                    {
                        //Grid cleanup
                        unitDataLayer[i, j] = null;
                    }

                    if (unitDataLayer[i, j] is Units.Com && j + 1 < unitDataLayer.GetLength(1))
                    {
                        var temp = (unitDataLayer[i, j + 1]);
                        if (temp == null)
                        {
                            //Move forward on open space
                            unitDataLayer[i, j + 1] = GetUnitAt(i, j);
                            unitDataLayer[i, j] = null;

                            if (j + 1 == unitDataLayer.GetLength(1) - 1)
                            {
                                _gameOver = true;
                            }
                        }
                        else if (temp is Units.Foliage)
                        {
                            int lowerBound = 0;
                            int upperBound = 1;

                            //Set traffic bounds whether an open, adjacent space exists
                            if (i > 0 && unitDataLayer[i - 1, j] == null) lowerBound = -1;
                            if (i < unitDataLayer.GetLength(0) && unitDataLayer[i + 1, j] == null) upperBound = 2;
                            
                            int redirectX = Program.r.Next(lowerBound, upperBound);

                            if (Math.Abs(redirectX) > 0)
                            {
                                //Redirect Com unit's path away from thick foliage
                                unitDataLayer[i + redirectX, j] = GetUnitAt(i, j);
                                unitDataLayer[i, j] = null;
                                break;
                            }
                            else
                            {
                                //If no open space, damage foliage
                                temp.Damage();
                            }
                        }
                        else if (temp is Units.Wall || temp is Units.Human || temp is Units.Barricade)
                        {
                            //Damage obstacles to proceed
                            unitDataLayer[i, j].Damage();
                            temp.Damage();
                        }
                        else
                        {
                            //Combine with forces that are further ahead and stuck, if possible
                            CombineCom(i, j, temp);
                        }
                    }
                }
            }
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