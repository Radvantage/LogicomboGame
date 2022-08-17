using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Barricade.cs
 * Description: Passive barrier unit which builds up for players to march their units forward against, complete line moves the wall forward.
 */
namespace Logicombo.Units
{
    class Barricade : BaseUnit
    {
        public override char OBJ_ICON
        {
            get
            {
                return 'X';
            }
        }

        public Barricade(int xPos, int yPos, int pLevel)
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
            Color = ConsoleColor.Yellow;
            ResetBody();
        }
    }
}