using System.Collections.Generic;

namespace DungeonCrawler
{
    abstract class Map
    {
        private int _Height;
        private int _Width;
        private Point _SpawnPoint;
        private char[,] _MapData;
        private Tile[,] _Tiles;
        private List<Tile> _ObjectTiles;

        public Tile[,] Tiles { get => _Tiles; }
        public Point SpawnPoint { get => _SpawnPoint; }
        public char[,] GetMapData { get => _MapData; }
        public List<Tile> GameObjects { get => _ObjectTiles; }
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
            //Objects after to make sure they're above. 
            for (int i = 0; i < _ObjectTiles.Count; i++)
            {
                if (_ObjectTiles[i].HasChanged)
                {
                    _ObjectTiles[i].Draw();
                }
            }
        }
        //Translates char[,] to Tile[,]
        public static Tile[,] MakeTiles<T>(char[,] dataIn, int mapHeight, int mapWidth)
        {
            Tile[,] dataOut = new Tile[mapHeight, mapWidth];
            for (int x = 0; x < mapHeight; x++)
            {
                for (int y = 0; y < mapWidth; y++)
                {          
                    dataOut[x, y] = Tile.GetTileFromChar(dataIn[x,y], new Point(x, y));
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

        //The game map. You can use this as a template for making new maps. 
        public class Simple : Map
        {
            public Simple()
            {
                _Height = 20;
                _Width = 20;
                _SpawnPoint = new Point(1, 1);
                _MapData = new char[,]
                {
                    { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#', },
                    { '#','.','.','.','.','.','.','.','.','.','.','.','.','.','.','.','#','.','.','#', },
                    { '#','.','.','.','.','.','.','#','#','.','.','.','.','.','.','.','#','.','.','#', },
                    { '#','.','.','.','.','.','.','#','.','.','.','.','.','.','#','.','#','.','.','#', },
                    { '#','#','#','#','#','#','#','#','#','#','#','#','.','.','#','.','#','#','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','.','.','#','.','.','.','.','#', },
                    { '.','.','.','.','.','.','#','.','.','.','.','.','.','.','#','.','.','.','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','.','.','#','.','.','.','.','#', },
                    { '#','#','#','#','#','#','#','.','.','.','.','.','.','.','#','.','.','.','.','#', },
                    { '#','.','.','.','.','.','#','.','#','#','#','#','#','#','#','#','#','.','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','#','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','.','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','.','.','.','.','.','.','#','.','.','.','.','.','.','#', },
                    { '#','.','.','.','.','.','.','.','.','.','.','.','.','.','.','.','.','.','.','#', },
                    { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#', },
                };
                Load();        
            
            }
            public override void LoadObjects()
            {
                _ObjectTiles = new List<Tile>();

                // instead of giving special tiles ids and placing them on the map, i desided to place them manually to have more controll over them.
                //All keys, doors, traps and portals. 
                _ObjectTiles.Add(new Tile.Portal(new Point(2, 18), new Point(6, 3)));

                _ObjectTiles.Add(new Tile.Door(new Point(9,7), ColorScheme.Blue(), 10));
                _ObjectTiles.Add(new Tile.Key(new Point(3, 8), 10, ColorScheme.Blue(), "Blue Key"));

                _ObjectTiles.Add(new Tile.Door(new Point(18, 12), ColorScheme.Red(), 11));
                _ObjectTiles.Add(new Tile.Key(new Point(14, 14), 11, ColorScheme.Red(), "Red Key"));

                //Can also use an existing tile location instead of creating a new point. 
                _ObjectTiles.Add(new Tile.Door(Tiles[4, 18].Location, ColorScheme.Yellow(), 12));
                _ObjectTiles.Add(new Tile.Key(Tiles[14, 5].Location, 12, ColorScheme.Yellow(), "Golden Key"));

                _ObjectTiles.Add(new Tile.Trap(Tiles[3, 3].Location));
                _ObjectTiles.Add(new Tile.Trap(Tiles[4, 12].Location));
                _ObjectTiles.Add(new Tile.Trap(Tiles[14, 4].Location));
                _ObjectTiles.Add(new Tile.Trap(Tiles[9, 18].Location));

                _ObjectTiles.Add(new Tile.Goal(_Tiles[6, 0].Location));
            }
        }
    }
}
