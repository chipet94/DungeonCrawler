using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Player : GameObject
    {
        private ConsoleColor _Color = ConsoleColor.Yellow;
        private string _Text = "PL";
        private int _Steps;
        private List<Tile.Key> _Keys;
        public Player(Point SpawnPoint)
        {
            _Steps = 0;
            _Keys = new List<Tile.Key>();
            Location = SpawnPoint;
        }
        public int Steps { get => _Steps; }
        public List<Tile.Key> keys { get => _Keys; }
        /// <summary>
        /// Adds key to keyring
        /// </summary>
        public void PickupKey(Tile.Key key)
        {
            _Keys.Add(key);
        }
        /// <summary>
        /// Removes key from keyring. 
        /// </summary>
        public void RemoveKey(Tile.Key key)
        {
            _Keys.Remove(key);
        }
        public void StepOnTrap()
        {
            _Steps += 10;
        }
        /// <summary>
        /// Prints info about player.
        /// </summary>
        public void PrintInfo()
        {
            Globals.ClearLine(0);
            Console.SetCursorPosition(0, 0);
            Console.Write($"Current location: X: {Location_X} Y: {Location_Y}");

            Globals.ClearLine(1);
            Console.SetCursorPosition(0, 1);
            Console.Write("Steps: " + _Steps);

            Globals.ClearLine(2);
            Console.SetCursorPosition(0, 2);
            Console.Write("Keyring: ");
            for (int i = 0; i < _Keys.Count; i++)
            {               
                Console.Write(_Keys[i].Name + " ");
            }
        }
        /// <summary>
        /// Draws the Player.
        /// </summary>
        public void Draw()
        {
            Point.TranslateLocation(Location).SetCursor();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = _Color;
            Console.Write(_Text);
            Console.ResetColor();
            PrintInfo();
        }
        /// <summary>
        /// Moves the player in a direction. 
        /// </summary>
        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    Location_X--;
                    break;
                case Direction.Down:
                    Location_X++;
                    break;
                case Direction.Left:
                    Location_Y--;
                    break;
                case Direction.Right:
                    Location_Y++;
                    break;
                default:
                    break;
            }
            _Steps++;
        }
        /// <summary>
        ///sets all visible tiles to not visible and sets all tiles around the player to visible. 
        /// </summary>
        public void CheckTiles(Map map)
        {
            //set all visible tiles to notvisible. 
            for (int x = 0; x < map.Height; x++)
            {
                for (int y = 0; y < map.Width; y++)
                {
                    if (map.Tiles[x,y].CurrentState == State.Visible)
                    {
                        map.Tiles[x, y].UpdateState(State.NotVisible);
                    }
                }
                //objects
                for (int i = 0; i < map.GameObjects.Count; i++)
                {
                    if (map.GameObjects[i].CurrentState == State.Visible)
                    {
                        map.GameObjects[i].UpdateState(State.NotVisible);
                    }
                }
            }
            // set all tiles around the player to visible. 
            var tileArray = map.Tiles;
            for (int x = Location.X -1; x <= Location.X +1; x++)
            {
                for (int y = Location.Y -1; y <= Location.Y +1; y++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        //Map tiles
                        if (x < map.Height && y < map.Width)
                        {
                            if (tileArray[x,y].CurrentState == State.Hidden || tileArray[x, y].CurrentState == State.NotVisible)
                            {
                                tileArray[x, y].UpdateState(State.Visible);
                            }
                            //Objects
                            for (int i = 0; i < map.GameObjects.Count; i++)
                            {
                                if (map.GameObjects[i].Location.Compare(x,y))
                                {
                                    if (map.GameObjects[i].CurrentState == State.Hidden || map.GameObjects[i].CurrentState == State.NotVisible)
                                    {
                                        map.GameObjects[i].UpdateState(State.Visible);
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
