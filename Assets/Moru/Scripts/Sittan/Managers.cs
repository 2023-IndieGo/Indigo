using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { return s_instance; } }

    #region Manager

    [SerializeField] TestManagerA _testManagerA = new TestManagerA();


    public static TestManagerA TestA { get { return Instance._testManagerA; } }

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
        }
    }

    public static void Clear()
    {
        TestA.Clear();
    }
}
