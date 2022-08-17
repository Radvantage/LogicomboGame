using System;
using System.Collections.Generic;
using System.Text;

/*
 * Project: Logicombo - A Game of Logic and Combat
 * Author: Kalin Robert Hanninen, 2021
 * File: BaseUnit.cs
 * Description: Base abstract class for defining behaviors, properties, and visuals of on-screen units.
 */
namespace Logicombo.Units
{
    abstract class BaseUnit : UnitI
    {
        //Default implementation for base properties.
        private char[] _body = new char[3];
        public char[] Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }

        //Identification color Property.
        private ConsoleColor _color;
        public ConsoleColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        //Power vitals value Property.
        private int _power;
        public int Power
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
            }
        }

        //Position grid coordinate Property.
        private (int, int) _position;
        public (int, int) Position
        {
            get
            {
                return _position;
            }
            set
            {
                //Validate coordinates against negative X/Y values - default to (0, 0).
                if (value.Item1 >= 0 && value.Item2 >= 0)
                {
                    _position = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Given .Position value of unit instance is invalid - X and/or Y coordinate(s) are negative!");
                }
            }
        }

        //Reset body representation depending on power level - default '@' OBJ_ICON constant.
        public virtual char OBJ_ICON
        {
            get
            {
                return '.';
            }
        }
        public virtual void ResetBody()
        {
            Body = new char[3] { '-', '-', '-' };
            for (int i = 0; i < Power; i++)
            {
                Body[i] = OBJ_ICON;
            }
        }

        //Receiving damage from attacking enemies, reduce power until 0.
        public void Damage()
        {
            if (Power > 0)
            {
                Power--;
            }
            ResetBody();
        }

        //Express all units as a combination of 3 characters on the grid - the body property, override object ToString().
        public override string ToString()
        {
            return new string(Body);
        }
    }
}