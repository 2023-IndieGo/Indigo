using System.Collections;
using System.Collections.Generic;

public partial class MapManager
{
    public struct TileRow
    {
        public List<Tile> rowTile;
    }

    public List<TileRow> tiles;

    public List<TileRow> Tiles { get => tiles; set => tiles = value; }

    public void Init()
    {
        UnityEngine.Debug.LogWarning("MapManager Init");

        Tiles = new List<TileRow>();

        for(int i = 0; i < 5; i++)
        {
            Tile tempTile = new Tile(i.ToString());
            TileRow tempTileRow = new TileRow();
            tempTileRow.rowTile = new List<Tile>();
            tempTileRow.rowTile.Add(tempTile);
            Tiles.Add(tempTileRow);
        }

        Debug();
    }

    public Tile GetTile(int x, int y)
    {
        return Tiles[x].rowTile[y];
    }

    public Chip MakeChip()
    {
        return new Chip();
    }

    public void Debug()
    {
        int index = 0;
        foreach(var rowTile in tiles)
        {
            string debug = "rowTile" + index;
            foreach(var tile in rowTile.rowTile)
            {
                debug += " " + tile.ID;

                foreach (var chip in tile.Chips)
                {
                    debug += " " + "C";
                }
            }

            UnityEngine.Debug.LogWarning(debug);
            index++;
        }
    }
}
