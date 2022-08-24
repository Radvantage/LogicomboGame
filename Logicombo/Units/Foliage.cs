using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Foliage.cs
 * Description: Passive barrier unit which redirects Com units from their respective lane based on tree (foliage) position.
 */
namespace Logicombo.Units
{
    class Foliage : BaseUnit
    {
        public override char OBJ_ICON
        {
            get
            {
                return '%';
            }
        }

        public Foliage(int xPos, int yPos)
        {
            Position = (xPos, yPos);
            Power = 1;
            ResetBody();
        }

        public override void ResetBody()
        {
            Body = new char[3] { '.', OBJ_ICON, '.' };
            Color = ConsoleColor.Green;
        }
    }
}