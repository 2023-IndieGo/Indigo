using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru
{
    public class TsetMono : SingleToneMono<TsetMono>
    {

        //public float offset = 1.7f;
        //public int x, y;
        //public int _x, _y;
        //public int xSize, ySize;
        //Managers.Map map;
        //public GameObject testTilePrefap;
        //public GameObject testUnitPrefap;
        //public GameObject testFieldPrefap;

        //public static OnGameStateChagne onTurnStart;
        //public static OnGameStateChagne onTurnEnd;

        //void Start()
        //{
        //    map = new Managers.Map(xSize, ySize);
        //    //�� �����
        //    for (int x = 0; x < xSize; x++)
        //    {
        //        for (int y = 0; y < ySize; y++)
        //        {
        //            GameObject newTile = Instantiate(testTilePrefap);
        //            newTile.name = $"tile {x}:{y}";
        //            if (x % 2 != 1)
        //            {
        //                newTile.transform.position = new Vector3(x* offset, 0, y* offset);
        //            }
        //            else
        //            {
        //                newTile.transform.position = new Vector3(x* offset, 0, (y - 0.5f)* offset);
        //            }
        //            newTile.AddComponent<TestMapViewer>().Init(Mapnagersmap.GetTile(x, y));
        //        }
        //    }

        //    //���� �����
        //    int centerX = xSize / 2 -1;
        //    int centerY = ySize / 2 - 1;
        //    var centerTile = Mapnagersmap.GetTile(centerX, centerY);
        //    Queen q = new Queen(centerTile);


        //    //�ڿ����� �����
        //    var tiles = MHexagonUtility.GetNeighborTile(centerTile);
        //    List<Tile> listTile = new List<Tile>();

        //    for (int i = 0; i < tiles.Length; i++)
        //    {
        //        listTile.Add(tiles[i]);
        //    }
        //    foreach (var ntile in tiles)
        //    {
        //        if (ntile != null)
        //        {
        //            var tile = MHexagonUtility.GetNeighborTile(ntile);
        //            foreach (var t in tile)
        //            {
        //                if (t != null)
        //                {
        //                    if (t.MiningField == null && t.Units.Count == 0 && !listTile.Contains(t))
        //                    {
        //                        ResourcesFiled field = new ResourcesFiled(t);
        //                        t.MiningField = field;
        //                        Debug.Log($"{t.adressX} : {t.adressY}");
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}

        //private void Update()
        //{
        //    if (x != _x || y != _y)
        //    {
        //        //x = x > xSize ? xSize - 1 : x;
        //        //x = x < 0 ? 0 : _x;

        //        //y = y > ySize ? ySize - 1 : y;
        //        //y = y < 0 ? 0 : y;

        //        //_x = x;
        //        //_y = y;
        //        var tile = MHexagonUtility.GetNeighborTile(Mapnagersmap.GetTile(x, y));
        //        Debug.Log($"{DebugUtillity.ArrayToString(tile)}");

        //    }
        //}

        //public void TurnStart()
        //{
        //    Debug.Log($"�� ���� �̺�Ʈ �׽�Ʈ");
        //    onTurnStart?.Invoke();
        //}

        //public void TurnEnd()
        //{
        //    Debug.Log($"�� ���� �̺�Ʈ �׽�Ʈ");
        //    onTurnEnd?.Invoke();
        //}

        //public void CreateCreature()
        //{
        //    Debug.Log($"���ο� ���� ���� : ���� ���۵Ǹ� ������ �ڵ����� ���� ���������� ������ ����");
            
        //    int x = Random.Range(0, TsetMono.instance.xSize);
        //    int y = Random.Range(0, TsetMono.instance.ySize);
        //    Tile tile = Mapnagersmap.GetTile(x, y);
        //    Unit unit = new Unit(tile);

        //}
    }
}