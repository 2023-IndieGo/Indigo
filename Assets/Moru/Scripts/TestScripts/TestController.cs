using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;
using Sirenix.OdinInspector;


public class TestController : MonoBehaviour
{

    public TestMapViewer tileViewer;
    private TestTile lastTile;
    public TestTile tile;
    public Unit SeletedUnit;

    void Update()
    {
        if (lastTile != tile)
        {
            lastTile = tile;
        }
        // ���콺 ��ư�� ������ ���� ����ĳ���� ����
        if (Input.GetMouseButtonDown(0))
        {

            // ���콺 ��ġ�� �������� ���� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ������ �����ϰ�, �浹�� ��ü ������ hit ������ ����
            if (Physics.Raycast(ray, out hit))
            {
                // �浹�� ��ü�� ���� ���
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

        //�����׽�Ʈ
    }


}
