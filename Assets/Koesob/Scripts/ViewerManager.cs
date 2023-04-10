using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewerManager : MonoBehaviour
{
    private static ViewerManager instance = null;

    [SerializeField] private GameObject pointerObject = null;
    [SerializeField] private PlayingCardsArea playingCardArea = null;
    [SerializeField] private GameObject nodeCardArea = null;
    [SerializeField] public GameObject canvas = null;

    public UnityAction<Vector2> CheckDrop;

    public GameObject PointerObject { get => pointerObject; set => pointerObject = value; }
    public static ViewerManager Instance { get => instance; private set => instance = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            this.gameObject.name = typeof(ViewerManager).ToString() + " (Singleton)";

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}
