﻿using System;

namespace DungeonCrawler
{
    abstract class Tile : GameObject, ITile
    {
        string _Text; // this is the "symbols" used for a tile. to make it display properly do not make the string longer than Globals.TILESIZE_WIDTH.
        State _State;
        TileType _Type;
        ConsoleColor _Color;
        ConsoleColor _TextColor;
        ColorScheme _ColorScheme;
        bool _Changed;

        public bool HasChanged { get => _Changed; set => _Changed = value; }
        public State CurrentState { get => _State; }
        public TileType Type { get => _Type; }

        public Tile(Point location)
        {
            Location = location;
            _Changed = true;
        }
        public virtual void Draw()
        {
                Point.TranslateLocation(Location).SetCursor();
                Console.ForegroundColor = _TextColor;
                Console.BackgroundColor = _Color;
                Console.Write(_Text);
                Console.ResetColor();
                _Changed = false;
           
        }
        public void UpdateState(State newState)
        {
            _State = newState;
            _Changed = true;
        }
        /// <summary>
        /// Translates a char to a Tile.
        /// </summary>
        public static Tile GetTileFromChar(char tileId, Point pos)
        {
            switch (tileId)
            {
                case '.':
                    return new Floor(pos);
                case '#':
                    return new Wall(pos);
                default:
                    throw new Exception("Invalid Tile ID");
            }
        }

        public bool Equals(TileType type)
        {
            if (this.Type == type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        class Wall : Tile
        {
            public Wall(Point location) : base(location)
            {
                _ColorScheme = ColorScheme.Gray();
                _State = State.NotVisible;
                base._Text = "_|";
                _TextColor = ConsoleColor.Black;
                base._Type = TileType.Wall;
            }
            public override void Draw()
            {
                if (_State == State.Visible)
                {
                    _Color = _ColorScheme.Visible;
                }
                if (_State == State.NotVisible)
                {
                    _Color = _ColorScheme.NotVisible;
                }
                base.Draw();
            }
        }

        class Floor : Tile
        {
            public Floor(Point location) : base(location)
            {
                _ColorScheme = ColorScheme.White();
                base._Text = "~~";
                base._Type = TileType.Floor;

                _State = State.Hidden;
            }
            public override void Draw()
            {
                if (_State == State.Visible)
                {
                    _Color = _ColorScheme.Visible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.NotVisible)
                {
                    _Color = _ColorScheme.NotVisible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.Hidden)
                {
                    _Color = _ColorScheme.Hidden;
                    _TextColor = ConsoleColor.Black;
                }
                base.Draw();
            }
        }

        public class Door : Tile
        {
            private int KeyID;
            public enum DoorState
            {
                Locked,
                UnLocked
            }
            private DoorState _DoorState;
            public DoorState GetDoorState { get => _DoorState; }
            public Door(Point location,ColorScheme colorScheme, int keyID) : base(location)
            {
                KeyID = keyID;
                _ColorScheme = colorScheme;
                base._Type = TileType.Door;
                _DoorState = DoorState.Locked;
                _State = State.Hidden;
                _Text = "||";
            }
            public override void Draw()
            {
                _TextColor = ConsoleColor.Black;
                if (_State == State.Visible)
                {
                    if (_DoorState == DoorState.Locked)
                    {
                        _Color = _ColorScheme.Locked;
                    }
                    else
                    {
                        _Color = _ColorScheme.Unlocked;
                        
                    }
                }
                if (_State == State.NotVisible)
                {
                    if (_DoorState == DoorState.Locked)
                    {
                        _Color = _ColorScheme.NotVisible_locked;
                    }
                    else
                    {
                        _Color = _ColorScheme.NotVisible_Unlocked;
                    }
                }
                if (_State == State.Hidden)
                {
                    _Color = _ColorScheme.Hidden;
                }
                base.Draw();
            }
            public bool CheckIfPlayerHasKey(Player player)
            {
                for (int i = 0; i < player.keys.Count; i++)
                {
                    if (player.keys[i].ID == KeyID)
                    {
                        _DoorState = DoorState.UnLocked;
                        player.RemoveKey(player.keys[i]);
                        return true;
                    }
                }
                return false;
            }
        }
        public class Key : Tile, IKey
        {
            private string _Name;
            private int _ID;
            public Key(Point location, int id, ColorScheme colorScheme, string name) : base(location)
            {
                _Type = TileType.Key;
                _State = State.Hidden;
                _ColorScheme = colorScheme;
                _Text = "8=";
                _Name = name;
                _ID = id;
            }
            public int ID { get => _ID; set => _ID = value; }
            public string Name => _Name;
            public override void Draw()
            {
                if (_State == State.Visible)
                {
                    _Color = _ColorScheme.Visible;
                }
                if (_State == State.NotVisible)
                {
                    _Color = _ColorScheme.NotVisible;
                }
                if (_State == State.Hidden)
                {
                    _Color = _ColorScheme.Hidden;
                }
                base.Draw();
            }
        }
        public class Trap : Tile
        {
            public Trap(Point location) : base(location)
            {
                _Text = "--";
                _ColorScheme = ColorScheme.White();
                _Type = TileType.Trap;
                _State = State.Hidden;
            }
            public override void Draw()
            {
                if (_State == State.Visible)
                {
                    _Color = _ColorScheme.Visible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.NotVisible)
                {
                    _Color = _ColorScheme.NotVisible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.Hidden)
                {
                    _Color = _ColorScheme.Hidden;
                    _TextColor = ConsoleColor.Black;
                }
                base.Draw();
            }
            public void TriggerSound()
            {
                Console.Beep();
            }
        }
        public class Portal: Tile
        {
            Point _Destination;
            public Portal(Point location, Point destination) : base(location)
            {
                _Text = "()";
                _Destination = destination;
                _ColorScheme = ColorScheme.Yellow();
                _Type = TileType.Portal;
                _State = State.Hidden;
            }
            public override void Draw()
            {
                if (_State == State.Visible)
                {
                    _Color = _ColorScheme.Visible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.NotVisible)
                {
                    _Color = _ColorScheme.NotVisible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.Hidden)
                {
                    _Color = _ColorScheme.Hidden;
                    _TextColor = ConsoleColor.Black;
                }
                base.Draw();
            }
            public void TeleportPlayer(Player player)
            {
                player.Location = _Destination;
            }
        }
        public class Goal : Tile
        {
            public Goal(Point location) : base(location)
            {
                _Text = "((";
                _ColorScheme = ColorScheme.White();
                _Type = TileType.Goal;
                _State = State.Hidden;
            }
            public override void Draw()
            {
                if (_State == State.Visible)
                {
                    _Color = _ColorScheme.Visible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.NotVisible)
                {
                    _Color = _ColorScheme.NotVisible;
                    _TextColor = ConsoleColor.DarkGray;
                }
                if (_State == State.Hidden)
                {
                    _Color = _ColorScheme.Hidden;
                    _TextColor = ConsoleColor.Black;
                }
                base.Draw();
            }
        }
    }
}
