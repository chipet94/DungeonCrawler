using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler
{
    abstract class Map
    {
        private int _Height;
        private int _Width;
        private Point _SpawnPoint;
        private int[] _MapData;
        private Tile[,] _Tiles;
        public Map()
        {
        }
        public Tile[,] Tiles { get => _Tiles; }
        public Point SpawnPoint { get => _SpawnPoint; }
        public int[] GetMapData { get => _MapData; }
        public int Height { get => _Height; }
        public int Width { get => _Width; }
        /// <summary>
        /// Redraws all tiles that has been changed. 
        /// </summary>
        public void Refresh()
        {
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    // to avoid the screen "flash" that normaly occur when you redraw all, only draw the ones that has been changed. 
                    if (_Tiles[x,y].HasChanged)
                    {
                        _Tiles[x, y].Draw();
                    }     
                }
            }
        }
        // Creates a 2d tile array from a 1d int array. used to make maps. 
        public static Tile[,] MakeTiles<T>(int[] dataIn, int mapHeight, int mapWidth)
        {
            Tile[,] dataOut = new Tile[mapHeight, mapWidth];
            for (int x = 0; x < mapHeight; x++)
            {
                for (int y = 0; y < mapWidth; y++)
                {          
                    dataOut[x, y] = Tile.GetTileFromInt(dataIn[x * mapWidth + y], new Point(x, y));
                }
            }
            return dataOut;
        }
        private void Load()
        {
            _Tiles = MakeTiles<Tile>(_MapData, Height, Width);
            LoadObjects();
        }
        public abstract void LoadObjects();
        public class Simple : Map
        {              
            public Simple()
            {
                _Height = 20;
                _Width = 20;
                _SpawnPoint = new Point(1, 1);
                _MapData = new int[]
                {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1,
                0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1,
                1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
                Load();          
            }
            public override void LoadObjects()
            {
                // instead of giving special tiles ids and placing them on the map, i desided to place them manually to have more controll over them.
                //All keys, doors, traps and portals. 
                _Tiles[2, 18].ContainedTileObject = new Tile.Portal(_Tiles[2, 18].Location, new Point(6, 3));

                _Tiles[9, 7].ContainedTileObject = new Tile.Door(Tiles[9, 7].Location, ColorScheme.Blue(), 10);
                _Tiles[3, 8].ContainedTileObject = new Tile.Key(Tiles[3, 8].Location, 10, ColorScheme.Blue(), "Blue Key");

                _Tiles[18, 12].ContainedTileObject = new Tile.Door(Tiles[18, 12].Location, ColorScheme.Red(), 11);
                _Tiles[14, 14].ContainedTileObject = new Tile.Key(Tiles[14, 14].Location, 11, ColorScheme.Red(), "Red Key");

                _Tiles[4, 18].ContainedTileObject = new Tile.Door(Tiles[4, 18].Location, ColorScheme.Yellow(), 12);
                _Tiles[14, 5].ContainedTileObject = new Tile.Key(Tiles[14, 5].Location, 12, ColorScheme.Yellow(), "Golden Key");

                _Tiles[3, 3].ContainedTileObject = new Tile.Trap(Tiles[3, 3].Location);
                _Tiles[4, 12].ContainedTileObject = new Tile.Trap(Tiles[4, 12].Location);
                _Tiles[14, 4].ContainedTileObject = new Tile.Trap(Tiles[14, 4].Location);
                _Tiles[9, 18].ContainedTileObject = new Tile.Trap(Tiles[9, 18].Location);

                _Tiles[6, 0].ContainedTileObject = new Tile.Goal(_Tiles[6, 0].Location);
            }
        }
    }
}
