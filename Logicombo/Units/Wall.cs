using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Wall.cs
 * Description: Passive barrier unit which protects against Com units for the Player.
 */
namespace Logicombo.Units
{
    class Wall : BaseUnit
    {
        public override char OBJ_ICON
        {
            get
            {
                return '#';
            }
        }

        public Wall(int xPos, int yPos, int pLevel)
        {
            Position = (xPos, yPos);
            if (pLevel <= 3 && pLevel > 0)
            {
                Power = pLevel;
            }
            else
            {
                Power = 3;
            }
            ResetBody();
        }

        public override void ResetBody()
        {
            Body = new char[3] { '-', '-', '-' };
            for (int i = 0; i < Power; i++)
            {
                Body[i] = OBJ_ICON;
            }
            switch (Power)
            {
                case 1:
                    Color = ConsoleColor.DarkGray;
                    break;
                case 2:
                    Color = ConsoleColor.Gray;
                    break;
                case 3:
                    Color = ConsoleColor.White;
                    break;
            }
        }
    }
}