using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru
{
    public class TsetMono : MonoBehaviour
    {
        public int x, y;
        public int xSize, ySize;
        TestMap map;
        public GameObject testTilePrefap;

        void Start()
        {
            map = new TestMap(xSize, ySize);
            //¸Ê ¸¸µé±â
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    GameObject newTile = Instantiate(testTilePrefap);
                        newTile.name = $"tile {x}:{y}";
                    if(x % 2 != 1)
                    {
                        newTile.transform.position = new Vector3(x, 0,y);
                    }
                    else
                    {
                        newTile.transform.position = new Vector3(x, 0, y - 0.5f);
                    }
                    newTile.AddComponent<TestMapViewer>().Init(TestMap.GetTile(x, y));
                }
            }
        }

        private void Update()
        {
            var tile = TestTileUtility.GetNeighborTile(TestMap.GetTile(x, y));
            
            Debug.Log($"{DebugUtillity.ArrayToString(tile)}");
        }
    }
}