using System;

namespace DungeonCrawler
{
    class Engine
    {
        Map[] _Maps;
        Map _SelectedMap;
        Player _Player;
        bool _Playing;
        public Engine()
        {
            MainMenuState();
        }
        private void GameLoop()
        {
            Console.Clear();
            InitGame();
            _Playing = true;
            while (_Playing)
            {  
                _Player.CheckTiles(_SelectedMap);
                _SelectedMap.Refresh();
                _Player.Draw();
                HandleInput();
            }
        }
        private void InitGame()
        {
            _Maps = new Map[] { new Map.Simple() };
            _SelectedMap = _Maps[0];
            _Player = new Player(_SelectedMap.SpawnPoint);
        }
        private void HandleInput()
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                    TryMovePlayer(Direction.Up);
                    break;
                case ConsoleKey.S:
                    TryMovePlayer(Direction.Down);
                    break;
                case ConsoleKey.A:
                    TryMovePlayer(Direction.Left);
                    break;
                case ConsoleKey.D:
                    TryMovePlayer(Direction.Right);
                    break;
                default:
                    break;
            }
        }
        private void TryMovePlayer(Direction direction)
        {
            bool move;
            switch (direction)
            {
                case Direction.Up:
                    move = CanMove(_Player.Location_X - 1, _Player.Location_Y);
                    break;
                case Direction.Down:
                    move = CanMove(_Player.Location_X + 1, _Player.Location_Y);
                    break;
                case Direction.Left:
                    move = CanMove(_Player.Location_X, _Player.Location_Y - 1);
                    break;
                case Direction.Right:
                    move = CanMove(_Player.Location_X, _Player.Location_Y + 1);
                    break;
                default:
                    move = false;
                    break;
            }
            if (move)
            {
                _Player.Move(direction);
                CheckTile();
            }
        }
        bool CanMove(int mapX, int mapY)
        {
            if (!_SelectedMap.Tiles[mapX, mapY].Equals(TileType.Wall))
            {
                for (int i = 0; i < _SelectedMap.GameObjects.Count; i++)
                {
                    var current = _SelectedMap.GameObjects[i];
                    if (current.Location.Compare(mapX,mapY))
                    {
                        if (current.Equals(TileType.Door))
                        {
                            Tile.Door door = (Tile.Door)_SelectedMap.GameObjects[i];
                            if (door.GetDoorState == Tile.Door.DoorState.Locked)
                            {
                                return door.CheckIfPlayerHasKey(_Player);
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CheckTile()
        {
            for (int i = 0; i < _SelectedMap.GameObjects.Count; i++)
            {
                var current = _SelectedMap.GameObjects[i];
                if (current.Location.Compare(_Player.Location))
                {
                    switch (current.Type)
                    {
                        case TileType.Goal:
                            _Playing = false;
                            GamefinishState();
                            return;
                        case TileType.Trap:
                            var trap = (Tile.Trap)current;
                            trap.TriggerSound();
                            _Player.StepOnTrap();
                            break;
                        case TileType.Portal:
                            var portal = (Tile.Portal)current;
                            portal.TeleportPlayer(_Player);
                            break;
                        case TileType.Key:
                            _Player.PickupKey((Tile.Key)current);
                            _SelectedMap.GameObjects.RemoveAt(i);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void MainMenuState()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Simple Dungeon Crawler - Main menu");
                Console.WriteLine("" +
                                    "1. Start Game" +
                                    "\n2. Guide" +
                                    "\n3. Exit");
                Console.Write("\nSelect(number): ");
                var choise = Console.ReadKey();
                switch (choise.Key)
                {
                    case ConsoleKey.D1:
                        GameLoop();
                        return;
                    case ConsoleKey.D2:
                        GuideState();
                        break;
                    case ConsoleKey.D3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Write(" - Not a choise...");
                        Continue();
                        break;
                }             
            }
        }
        private void Continue()
        {
            Console.Write("\nPress Enter to continue....");
            var enter = Console.ReadKey().Key;
            while (enter != ConsoleKey.Enter)
            {
                enter = Console.ReadKey().Key;
            }
        }
        private void GuideState()
        {
            Console.Clear();
            Console.WriteLine("The goal of the game is to find your way out of the cave using as few steps as possible.\n" +
                "The cave is a dangerous place to explore and lots of rooms are blocked by locked doors. To unlock a door you have\n" +
                "to find a key with the same color as the door you want to unlock.\n" +
                "You have to be careful while searching for the exit, some floor tiles are traps, and they are hard to see. \n" +
                "if you step on a trap you will hear a indicator beep and be punished with 10 extra steps.\n" +
                "Use WASD to move around.");
            Continue();
        }
        private void GamefinishState()
        {
            Console.Clear();
            Console.WriteLine($"You made it out in {_Player.Steps} steps!");
            if (_Player.Steps <= 80)
            {
                Console.WriteLine("You're AWESOME! Something tells me you've been here before...");
            }
            else if (_Player.Steps > 80 && _Player.Steps <= 120)
            {
                Console.WriteLine("Good job! but there is a shorter way out... ");
            }
            else
            {
                Console.WriteLine("There are lots improvements to be made... but hey! You survived.");
            }
            Continue();
            MainMenuState();
        }

    }
}
