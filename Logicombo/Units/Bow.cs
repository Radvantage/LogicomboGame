using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Wiz.cs
 * Description: Bow sub-class, Human - details construction/behavior of ranged/offensive unit type.
 */
namespace Logicombo.Units
{
    class Bow : Human
    {
        public override char OBJ_ICON
        {
            get
            {
                return '}';
            }
        }

        public Bow(int xPos, int yPos, int pLevel, Human.Faces fDirect, ConsoleColor pColor)
        {
            base.Position = (xPos, yPos);
            base.Power = pLevel;
            base.Direction = fDirect;
            base.Body = new char[] { Convert.ToString(Power)[0], (char)Direction, OBJ_ICON };
        }
    }
}