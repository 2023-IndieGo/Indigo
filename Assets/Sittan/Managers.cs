using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { return s_instance; } }

    #region Manager

    [SerializeField] TestManagerA _testManagerA = new TestManagerA();
    [SerializeField] MapManager map = new MapManager();
    [SerializeField] ResourceManager resource = new ResourceManager();


    public static TestManagerA TestA { get { return Instance._testManagerA; } }
    public static MapManager Map { get { return Instance.map; } }
    public static ResourceManager Resource { get { return Instance.resource; } }
    #endregion




    private void Awake()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();



            TestA.Init();
            map.Init(10,10);
            resource.Init();
        }
    }

    public static void Clear()
    {
        TestA.Clear();
    }
}
