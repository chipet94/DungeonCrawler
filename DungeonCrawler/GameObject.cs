namespace DungeonCrawler
{
    class GameObject
    {
        private Point _Location;
        public Point Location { get => _Location; set => _Location = value; }
        public int Location_X { get => _Location.X; set => _Location.X = value; }
        public int Location_Y { get => _Location.Y; set => _Location.Y = value; }
    }
}
