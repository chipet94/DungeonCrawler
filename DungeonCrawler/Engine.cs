using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler
{
    class Engine
    {
        public Map[] Maps;
        Map _SelectedMap;
        Player _Player;
        bool _Playing;
        public Engine()
        {
            Maps = new Map[] { new Map.Simple() };
            _SelectedMap = Maps[0];
            _Player = new Player(_SelectedMap.SpawnPoint);
        }
        
        public void GameLoop()
        {
            _Playing = true;
            _SelectedMap.Refresh();
            while (_Playing)
            {
                
                _Player.CheckTiles(_SelectedMap);
                _SelectedMap.Refresh();
                _Player.Draw();


                GetInput();
            }
        }
        public void GetInput()
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    TryMovePlayer(Direction.Up);
                    break;
                case ConsoleKey.DownArrow:
                    TryMovePlayer(Direction.Down);
                    break;
                case ConsoleKey.LeftArrow:
                    TryMovePlayer(Direction.Left);
                    break;
                case ConsoleKey.RightArrow:
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
                //_SelectedMap.Tiles[player.Location.X, player.Location.Y].Draw();
                _SelectedMap.Tiles[_Player.Location_X, _Player.Location_Y].HasChanged = true;
                _Player.Move(direction);
                _Player.CheckTiles(_SelectedMap);
                CheckTile();
            }
        }
        bool CanMove(int mapX, int mapY)
        {

            if (_SelectedMap.Tiles[mapX, mapY].Type != Tile.TileType.Wall)
            {
                if (_SelectedMap.Tiles[mapX, mapY].ContainedTileObject != null && _SelectedMap.Tiles[mapX, mapY].ContainedTileObject.Type != Tile.TileType.Door)
                {
                    return true;
                }
                else
                {
                    if (_SelectedMap.Tiles[mapX, mapY].ContainedTileObject != null && _SelectedMap.Tiles[mapX, mapY].ContainedTileObject.Type == Tile.TileType.Door)
                    {
                        Tile.Door door = (Tile.Door)_SelectedMap.Tiles[mapX, mapY].ContainedTileObject;
                        if (door.GetDoorState == Tile.Door.DoorState.Locked)
                        {
                            return door.CheckIfPlayerHasKey(_Player);
                        }
                    }
                    return true;
                }             
            }
            else
            {
                return false;
            }
        }
        private void CheckTile()
        {
            var x = _Player.Location_X;
            var y = _Player.Location_Y;
            if (_SelectedMap.Tiles[x, y].ContainedTileObject != null)
            {
                switch (_SelectedMap.Tiles[x, y].ContainedTileObject.Type)
                {
                    case Tile.TileType.Goal:
                        _Playing = false;
                        return;
                    case Tile.TileType.Trap:
                        var trap = (Tile.Trap)_SelectedMap.Tiles[x, y].ContainedTileObject;
                        trap.TriggerSound();
                        _Player.StepOnTrap();
                        break;
                    case Tile.TileType.Portal:
                        var portal = (Tile.Portal)_SelectedMap.Tiles[x, y].ContainedTileObject;
                        portal.TeleportPlayer(_Player);
                        break;
                    case Tile.TileType.Key:
                        _Player.PickupKey((Tile.Key)_SelectedMap.Tiles[x, y].ContainedTileObject);
                        _SelectedMap.Tiles[x, y].ContainedTileObject = null;
                        break;
                    default:
                        break;
                }
            }
        }
        public void CheckPath()
        {
            if (true)
            {

            }
        }
    }
}
