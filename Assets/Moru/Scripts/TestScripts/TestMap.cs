using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru
{

    public class TestMap
    {
        public static int x;
        public static int y;
        private static TestTile[,] grids;
        public TestMap(int x, int y)
        {
            TestMap.x = x;
            TestMap.y = y;
            grids = new TestTile[x, y];
            for (int _x = 0; _x < TestMap.x; _x++)
            {
                for (int _y = 0; _y < TestMap.y; _y++)
                {
                    grids[_x, _y] = new TestTile(_x, _y);
                }
            }

            
        }

        public static TestTile GetTile(int x, int y)
        {
            return grids[x, y];
        }
    }

    public class TestTile
    {
        private Stack<Unit> units;
        private ResourcesFiled miningField;
        private Stack<Res> resources;
        public int adressX;
        
        public int adressY;
        
        public TestTile(int x, int y)
        {
            adressX = x;
            adressY = y;
        }
    }

    public class TestTileUtility
    {
        /// <summary>
        /// 타일의 주변타일 6개를 받아옵니다.
        /// </summary>
        /// <param name="selectedTile"></param>
        /// <returns></returns>
        public static TestTile[] GetNeighborTile(TestTile selectedTile)
        {
            TestTile[] result = new TestTile[6];
            int x = selectedTile.adressX;
            int y = selectedTile.adressY;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = null;
            }
            if(selectedTile.adressY %2 != 1)
            {

                if(y - 1 >= 0)
                {
                    result[0] = TestMap.GetTile(x, y - 1);
                    
                }
                if(y + 1 <TestMap.y)
                {
                    result[3] = TestMap.GetTile(x, y + 1);
                    if (x - 1 >= 0)
                    {
                        result[5] = TestMap.GetTile(x - 1, y);
                        result[4] = TestMap.GetTile(x - 1, y+1);
                    }
                    if (x + 1 < TestMap.x)
                    {
                        result[1] = TestMap.GetTile(x + 1, y);
                        result[2] = TestMap.GetTile(x + 1, y + 1);
                    }
                }
            }
            else
            {
                if (y - 1 >= 0)
                {
                    result[0] = TestMap.GetTile(x, y - 1);
                    if (x - 1 >= 0)
                    {
                        result[5] = TestMap.GetTile(x - 1, y-1);
                        result[4] = TestMap.GetTile(x - 1, y);
                    }
                    if (x + 1 < TestMap.x)
                    {
                        result[1] = TestMap.GetTile(x + 1, y-1);
                        result[2] = TestMap.GetTile(x + 1, y);
                    }

                }
                if (y + 1 < TestMap.y)
                {
                    result[3] = TestMap.GetTile(x, y + 1);
                }
            }


            return result;
        }
    }
}