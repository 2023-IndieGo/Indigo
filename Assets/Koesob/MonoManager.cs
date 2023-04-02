using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonoManager : MonoBehaviour
{
    private static MonoManager managers = null;
    public static MonoManager Managers
    { get { return managers; } }

    private GameManager game;
    private MapManager map;
    [SerializeField] private TestUIManager ui;

    public GameManager Game
    { get { return game; } }
    public MapManager Map
    { get { return map; } }
    public TestUIManager UI
    { get { return ui; } }

    [SerializeField] private TextMeshProUGUI state;
    [SerializeField] private Button Button_StartTurn;
    [SerializeField] private Button Button_EndTurn;
    [SerializeField] private Button Button_EndGame;

    private void Awake()
    {
        if(managers == null)
        {
            managers = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        game = new GameManager();
        map = new MapManager();
        game.Init();
        map.Init();
        ui.Init();

        Button_StartTurn?.onClick.RemoveAllListeners();
        Button_StartTurn?.onClick.AddListener(game.StartTurn);
        Button_EndTurn?.onClick.RemoveAllListeners();
        Button_EndTurn?.onClick.AddListener(game.EndTurn);
        Button_EndGame?.onClick.RemoveAllListeners();
        Button_EndGame?.onClick.AddListener(game.EndGame);

        game.StartGame();
    }

    public void ChangeState(string _state)
    {
        state.text = _state;
    }
}
