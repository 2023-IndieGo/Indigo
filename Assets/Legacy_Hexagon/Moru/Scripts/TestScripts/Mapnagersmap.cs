using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lagacy_Hexagon.Moru;
using Sirenix.OdinInspector;


namespace Lagacy_Hexagon
{
    namespace Moru
    {

        public class TestMapViewer : MonoBehaviour
        {
            public Tile myTile;
            private void OnEnable()
            {

            }

            public void Init(Tile _tile)
            {
                myTile = _tile;
                myTile.curPosition = transform.position;
            }
        }

        //[System.Serializable]
        //public class Mapnagersmap
        //{
        //    public static int x;
        //    public static int y;
        //    private static Tile[,] grids;

        //    public Mapnagersmap(int x, int y)
        //    {
        //        Mapnagersmap.x = x;
        //        Mapnagersmap.y = y;
        //        grids = new Tile[x, y];
        //        for (int _x = 0; _x < Mapnagersmap.x; _x++)
        //        {
        //            for (int _y = 0; _y < Mapnagersmap.y; _y++)
        //            {
        //                grids[_x, _y] = new Tile(_x, _y);
        //            }
        //        }
        //    }

        //    public static Tile GetTile(int x, int y)
        //    {
        //        return grids[x, y];
        //    }
        //}

        [System.Serializable]
        public class Tile
        {
            [ShowInInspector, TitleGroup("Field Condition")]
            private Stack<Unit> units = new Stack<Unit>();
            public Stack<Unit> Units => units;

            [ShowInInspector]
            private ResourcesFiled miningField;
            public ResourcesFiled MiningField { get => miningField; set => miningField = value; }

            [ShowInInspector]
            private Dictionary<eRGB, Stack<Res>> resList = new Dictionary<eRGB, Stack<Res>>();
            public Dictionary<eRGB, Stack<Res>> ResList => resList;

            [TitleGroup("Adress")]
            public int adressX;

            public int adressY;

            public Vector3 curPosition;

            public Tile(int x, int y)
            {
                adressX = x;
                adressY = y;
                //메모리 초기화
                if (resList == null)
                {
                    resList = new Dictionary<eRGB, Stack<Res>>();
                }
                for (int i = 0; i < (int)eRGB.Black; i++)
                {
                    resList.Add((eRGB)i, new Stack<Res>());
                }

            }

            /// <summary>
            /// 해당타일로부터 매개변수값의 타일을 받아옵니다.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public Res PopRes(eRGB type)
            {
                if (resList.ContainsKey(type))
                {
                    var stackList = resList[type];
                    if (stackList.TryPop(out Res result))
                    {
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
                else return null;
            }

            /// <summary>
            /// 해당타일의 자원현황에 자원을 쌓습니다.
            /// </summary>
            /// <param name="res"></param>
            public void AddRes(Res res)
            {
                if (resList.ContainsKey(res.colorType))
                {
                    var stackList = resList[res.colorType];
                    stackList.Push(res);
                }
            }

            public Unit PopUnit()
            {
                if (units.TryPop(out var result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }

            public void PushUnit(Unit unit)
            {
                units.Push(unit);
            }
        }
    }
}