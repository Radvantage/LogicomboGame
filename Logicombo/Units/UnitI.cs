using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: UnitI.cs
 * Description: Unit interface for determining contents and structure of in-game objects.
 */
namespace Logicombo.Units
{
    interface UnitI
    {
        //Body property - Representation of unit to communicate type, power, party.
        char[] Body
        {
            get;
            set;
        }

        //Color property - Unique identification color to draw in view render.
        ConsoleColor Color
        {
            get;
            set;
        }

        //Power property - Quantity of individual unit.
        int Power
        {
            get;
            set;
        }

        //Position tuple property - Coordinates of unit on grid.
        (int, int) Position
        {
            get;
            set;
        }
    }
}