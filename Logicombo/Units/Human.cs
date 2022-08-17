using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: Human.cs
 * Description: Base Human object, player's controllable "Hornwswog" unit types inherit this Class.
 */
namespace Logicombo.Units
{
    abstract class Human : BaseUnit
    {
        //Enumeration for indicating unit's cardinal face. Describe char equivalents for display.
        public enum Faces
        {
            WEST = '<',
            EAST = '>',
            NORTH = '^'
        }

        //Property for direction, uses faces enumeration.
        private Faces _direction;
        public Faces Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        //Override body reset - player's units have a unique representation on the battlefield.
        public override void ResetBody()
        {
            Body = new char[] { Convert.ToString(Power)[0], (char)Direction, OBJ_ICON };
        }
    }
}