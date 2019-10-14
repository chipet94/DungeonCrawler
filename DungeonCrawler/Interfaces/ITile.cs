namespace DungeonCrawler
{
    interface ITile
    {
        TileType Type { get; }
        bool Equals(TileType type);
    }
}
