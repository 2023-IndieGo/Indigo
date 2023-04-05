using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lagacy_Hexagon.Moru;
using Sirenix.OdinInspector;

namespace Lagacy_Hexagon
{
    public class TestController : MonoBehaviour
    {

        public TestMapViewer tileViewer;
        private Tile lastTile;
        public Tile tile;
        public Unit SeletedUnit;

        void Update()
        {
            if (lastTile != tile)
            {
                lastTile = tile;
            }
            // 마우스 버튼이 눌렸을 때만 레이캐스팅 수행
            if (Input.GetMouseButtonDown(0))
            {

                // 마우스 위치를 기준으로 레이 생성
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 레이캐스팅을 수행하고, 충돌한 객체 정보를 hit 변수에 저장
                if (Physics.Raycast(ray, out hit))
                {
                    // 충돌한 객체의 정보 출력
                    Debug.Log("Hit object: " + hit.transform.name);
                    var tileData = hit.collider.gameObject.GetComponentInParent<TestMapViewer>();
                    if (tileData != null)
                    {
                        tileViewer = tileData;
                        tile = tileViewer.myTile;
                    }
                    if (tile != null)
                    {
                        if (tile.MiningField == null && tile.Units.Count == 0 && SeletedUnit == null)
                        {
                            //tile.MiningField = new ResourcesFiled(tile);
                        }

                        else if (tile.MiningField == null && tile.Units.Count == 0 && SeletedUnit != null)
                        {
                            SeletedUnit.OnMove(tile);
                            SeletedUnit = null;
                        }

                        else if (tile.Units.Count > 0)
                        {
                            SeletedUnit = tile.Units.Pop();
                        }
                    }
                }

            }

            //무브테스트
        }


    }
}