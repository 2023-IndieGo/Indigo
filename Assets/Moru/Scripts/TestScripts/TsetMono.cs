using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru
{
    public class TsetMono : MonoBehaviour
    {
        public int x, y;
        TestMap map;


        void Start()
        {
            map = new TestMap(50, 50);
        }

        private void Update()
        {
            var tile = TestTileUtility.GetNeighborTile(TestMap.GetTile(x, y));
            
            Debug.Log($"{DebugUtillity.ArrayToString(tile)}");
        }
    }
}