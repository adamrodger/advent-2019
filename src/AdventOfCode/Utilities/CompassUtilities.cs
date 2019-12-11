using System;

namespace AdventOfCode.Utilities
{
    /// <summary>
    /// Compass bearing
    /// </summary>
    public enum Bearing { North, South, East, West };

    /// <summary>
    /// Turn direction
    /// </summary>
    public enum TurnDirection { Left = 0, Right = 1 };

    /// <summary>
    /// Extensions methods to do with moving around a grid
    /// </summary>
    public static class DirectionExtensions
    {
        /// <summary>
        /// Turn in the given direction
        /// </summary>
        /// <param name="bearing">Current bearing</param>
        /// <param name="turn">Turn direction</param>
        /// <returns>New bearing</returns>
        public static Bearing Turn(this Bearing bearing, TurnDirection turn)
        {
            switch (bearing)
            {
                case Bearing.North:
                    return turn == TurnDirection.Left ? Bearing.West : Bearing.East;
                case Bearing.South:
                    return turn == TurnDirection.Left ? Bearing.East : Bearing.West;
                case Bearing.East:
                    return turn == TurnDirection.Left ? Bearing.North : Bearing.South;
                case Bearing.West:
                    return turn == TurnDirection.Left ? Bearing.South : Bearing.North;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Move from one position to another in the given direction and number of steps
        /// </summary>
        /// <param name="position">Starting position</param>
        /// <param name="bearing">Move direction</param>
        /// <param name="steps">Move steps</param>
        /// <returns>New position</returns>
        public static (int x, int y) Move(this (int x, int y) position, Bearing bearing, int steps = 1)
        {
            switch (bearing)
            {
                case Bearing.North:
                    return (position.x, position.y - steps);
                case Bearing.South:
                    return (position.x, position.y + steps);
                case Bearing.East:
                    return (position.x + steps, position.y);
                case Bearing.West:
                    return (position.x - steps, position.y);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
