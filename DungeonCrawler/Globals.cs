using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    class Globals
    {
        public const int GAMEFRAME_X = 5, //X and Y coordinates for the game
                         GAMEFRAME_Y = 0,
                         TILESIZE_HEIGHT = 1, //Tile size
                         TILESIZE_WIDTH = 2; // a width of 2 to make the tiles square. 

        /// <summary>ClearLine clears a row in the console.
        /// <para>Usage: ClearLine(5)</para>
        /// </summary>
        public static void ClearLine(int row) 
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }
    struct ColorScheme
    {
        public ConsoleColor Visible;
        public ConsoleColor Hidden;
        public ConsoleColor NotVisible;
        public ConsoleColor Locked;
        public ConsoleColor Unlocked;
        public ConsoleColor NotVisible_Unlocked;
        public ConsoleColor NotVisible_locked;
        public static ColorScheme Blue()
        {
            return new ColorScheme
            {
                Hidden = ConsoleColor.Black,
                Visible = ConsoleColor.Blue,
                NotVisible = ConsoleColor.DarkBlue,
                Locked = ConsoleColor.Blue,
                Unlocked = ConsoleColor.Green,
                NotVisible_locked = ConsoleColor.DarkBlue,
                NotVisible_Unlocked = ConsoleColor.DarkGreen
            };
        }
        public static ColorScheme Yellow()
        {
            return new ColorScheme
            {
                Hidden = ConsoleColor.Black,
                Visible = ConsoleColor.Yellow,
                NotVisible = ConsoleColor.DarkYellow,
                Locked = ConsoleColor.Yellow,
                Unlocked = ConsoleColor.Green,
                NotVisible_locked = ConsoleColor.DarkYellow,
                NotVisible_Unlocked = ConsoleColor.DarkGreen
            };
        }
        public static ColorScheme Red()
        {
            return new ColorScheme
            {
                Hidden = ConsoleColor.Black,
                Visible = ConsoleColor.Red,
                NotVisible = ConsoleColor.DarkRed,
                Locked = ConsoleColor.Red,
                Unlocked = ConsoleColor.Green,
                NotVisible_locked = ConsoleColor.DarkRed,
                NotVisible_Unlocked = ConsoleColor.DarkGreen
            };
        }
        public static ColorScheme Gray()
        {
            return new ColorScheme
            {
                Hidden = ConsoleColor.Black,
                Visible = ConsoleColor.Gray,
                NotVisible = ConsoleColor.DarkGray,
                Locked = ConsoleColor.DarkGreen,
                Unlocked = ConsoleColor.Green,
                NotVisible_locked = ConsoleColor.DarkBlue,
                NotVisible_Unlocked = ConsoleColor.DarkGreen
            };
        }
        public static ColorScheme White()
        {
            return new ColorScheme
            {
                Hidden = ConsoleColor.Black,
                Visible = ConsoleColor.White,
                NotVisible = ConsoleColor.Gray,
                Locked = ConsoleColor.DarkGreen,
                Unlocked = ConsoleColor.Green,
                NotVisible_locked = ConsoleColor.DarkBlue,
                NotVisible_Unlocked = ConsoleColor.DarkGreen
            };
        }
    }
    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// SetCursor() sets the console cursor to the location of the point.
        /// </summary>
        public void SetCursor()
        {
            Console.SetCursorPosition(Y, X);
        }
        /// <summary>
        /// Translates map coordinates to console coordinates.
        /// </summary>
        public static Point TranslateLocation(int x, int y)
        {
            return new Point((x * Globals.TILESIZE_HEIGHT) + Globals.GAMEFRAME_X, (y * Globals.TILESIZE_WIDTH) + Globals.GAMEFRAME_Y);
        }
        /// <summary>
        /// Translates map coordinates to console coordinates.
        /// </summary>
        public static Point TranslateLocation(Point point)
        {
            return new Point((point.X * Globals.TILESIZE_HEIGHT) + Globals.GAMEFRAME_X, (point.Y * Globals.TILESIZE_WIDTH) + Globals.GAMEFRAME_Y);
        }
    }
}
