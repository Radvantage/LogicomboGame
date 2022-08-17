using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Cut.cs
 * Description: Cut sub-class, Human - details construction/behavior of melee/defensive unit type.
 */
namespace Logicombo.Units
{
    class Cut : Human
    {
        public override char OBJ_ICON
        {
            get
            {
                return '!';
            }
        }

        public Cut(int xPos, int yPos, int pLevel, Human.Faces fDirect, ConsoleColor pColor)
        {
            Position = (xPos, yPos);
            Power = pLevel;
            Direction = fDirect;
            Body = new char[] { Convert.ToString(Power)[0], (char)Direction, OBJ_ICON };
        }
    }
}