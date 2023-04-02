using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject chipPrefab;

    public void Init()
    {

    }

    public void StartGameViewer()
    {
        for(int i = 0; i < MonoManager.Managers.Map.Tiles.Count; i++)
        {
            Instantiate(tilePrefab, new Vector3(i * 3.5f, 0, 0), Quaternion.identity);
        }
    }

    public void UpdateViewer()
    {
        var child = this.gameObject.GetComponentsInChildren<Transform>();


        int x = 0;
        int y = 0;

        foreach(var rowTile in MonoManager.Managers.Map.Tiles)
        {
            foreach(var tile in rowTile.rowTile)
            {
                foreach(var chip in tile.Chips)
                {
                    float randX = Random.Range(0, 1.5f);
                    float randZ = Random.Range(0, 1.5f);
                    Instantiate(chipPrefab, new Vector3(x * 3.5f + randX, y + 5, randZ), Quaternion.identity, this.transform);
                    y++;
                }
            }

            x++;
        }
    }
}
