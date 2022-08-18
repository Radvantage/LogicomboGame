using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Com.cs
 * Description: Computer "Wickling" units, hostiles of varying vital power.
 */
namespace Logicombo.Units
{
    class Com : BaseUnit
    {
        public override char OBJ_ICON
        {
            get
            {
                return 'W';
            }
        }

        //Combine one power into self from another unit behind
        public void Combine()
        {
            Power++;
            ResetBody();
        }

        //Com enemy unit constructor - X must exist within width of Battlegrid
        public Com(int xPos, int pLevel)
        {
            Position = (xPos, 0);
            if (pLevel <= 3 && pLevel > 0) { 
                Power = pLevel;
            }
            else
            {
                Power = 3;
            }
            Color = ConsoleColor.Red;
            ResetBody();
        }
    }
}