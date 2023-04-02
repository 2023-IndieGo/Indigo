using System.Collections;
using System.Collections.Generic;
using Moru;

public partial class MapManager
{
    public struct TileRow
    {
        public List<Tile> rowTile;
    }

    public List<TileRow> tiles;

    public List<TileRow> Tiles { get => tiles; set => tiles = value; }

    public int x;
    public int y;

    /// <summary>
    /// 맵을 x, y사이즈로 생성합니다.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Init(int x, int y)
    {
        UnityEngine.Debug.LogWarning("MapManager Init");

        Tiles = new List<TileRow>();

        //타일생성 구현
        //MapManager.x = x;
        //MapManager.y = y;
        //grids = new Tile[x, y];
        //for (int _x = 0; _x < Mapnagersmap.x; _x++)
        //{
        //    for (int _y = 0; _y < Mapnagersmap.y; _y++)
        //    {
        //        grids[_x, _y] = new Tile(_x, _y);
        //    }
        //}

    }

    public Tile GetTile(int x, int y)
    {
        return Tiles[x].rowTile[y];
    }

    public Chip MakeChip()
    {
        return new Chip();
    }

}
