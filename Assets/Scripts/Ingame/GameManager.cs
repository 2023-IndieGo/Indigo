using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 인게임상태에서만 새로 생성 후 초기화(Init())되도록
/// 인게임이 종료되면 GameManager
/// null로 바꾸고 이벤트를 모두 날려야함.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    #region Field
    private GameState _cur_GameState;
    /// <summary>
    /// 현재 게임의 상태입니다.
    /// </summary>
    public GameState cur_GameState
    {
        get => cur_GameState;
    }

    public Server_authority_Type server_Authority { get; private set; }

    /// <summary>
    /// 인게임인지 아닌지 나타내는 불린 변수:
    /// true : 인게임 ___/___
    /// false : 인게임이 아님
    /// </summary>
    public bool isGaming = false;

    private float _prepare_CurTime;
    /// <summary>
    /// 준비단계의 진행되고 있는 현재시간입니다.
    /// </summary>
    public float prepare_CurTime;

    /// <summary>
    /// 준비단계 진입 시 각 턴마다의 주어지는 시간입니다.
    /// </summary>
    public float _prepare_maxTime => default_PrepareTime * prepareTime_IncreaseRate * current_Turn;

    private float _default_PrepareTime;
    /// <summary>
    /// 기본 준비단계 시간입니다.
    /// </summary>
    public float default_PrepareTime;

    private float _prepareTime_IncreaseRate = 1.1f;
    /// <summary>
    /// 매 턴마다의 준비시간 증가비율입니다.
    /// </summary>
    public float prepareTime_IncreaseRate;

    private uint _current_Turn = 1;
    /// <summary>
    /// 현재 턴이 몇 턴인지 알려주는 필드변수입니다.
    /// </summary>
    public uint current_Turn => _current_Turn;

    /// <summary>
    /// 현재 게임의 플레이어 데이터 배열입니다. [0] = 나, [1] = 상대방
    /// </summary>
    public GamePlayer[] players = new GamePlayer[2];
    #endregion


    #region Reference
    private BattlePhaseController _battleController;
    public BattlePhaseController battleController
    {
        get
        {
            if (_battleController == null)
            { _battleController = GameObject.FindObjectOfType<BattlePhaseController>(); }
            if (_battleController == null)
            {
                GameObject obj = new GameObject("BattleController");
                _battleController = obj.AddComponent<BattlePhaseController>();
            }
            return _battleController;
        }
    }

    #endregion


    #region Events
    Dictionary<GameState, GameStateEventTrigger> StateEvents;
    class GameStateEventTrigger
    {
        public Del_NoRet_NoParams start;
        public Del_NoRet_NoParams update;
        public Del_NoRet_NoParams exit;

        public GameStateEventTrigger(Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
        {
            this.start += start;
            this.update += update;
            this.exit += exit;
        }
    }

    #endregion


    #region Constructor

    #endregion


    #region Public Methods

    public void Destroy()
    {
        for (int i = 0; i < (int)GameState.End + 1; i++)
        {
            StateEvents[(GameState)i] = null;
        }
    }
    /// <summary>
    /// 게임의 상태를 변경합니다.
    /// 변경된 상태에 따른 이벤트가 실행됩니다.
    /// </summary>
    /// <param name="targetState"></param>
    public void SetGameState(GameState targetState)
    {
        if (StateEvents.TryGetValue(targetState, out var cur_State))
        {
            cur_State.exit?.Invoke();
        }
        if (StateEvents.TryGetValue(targetState, out var next_State))
        {
            next_State.start?.Invoke();
        }
        _cur_GameState = targetState;
    }

    /// <summary>
    /// 해당 게임스테이트에 매개변수 메서드를 등록합니다.
    /// </summary>
    /// <param name="targetStage"></param>
    /// <param name="start"></param>
    /// <param name="update"></param>
    /// <param name="exit"></param>
    public void AddEventOnState(GameState targetState, Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
    {
        if (StateEvents.TryGetValue(targetState, out var value))
        {
            value.start += start;
            value.update += update;
            value.exit += exit;
        }
        else
        {
            StateEvents.Add(targetState, new GameStateEventTrigger(start, update, exit));
        }
    }

    /// <summary>
    /// 해당 게임스테이트에 매개변수 메서드를 제거합니다.
    /// </summary>
    /// <param name="targetState"></param>
    /// <param name="start"></param>
    /// <param name="update"></param>
    /// <param name="exit"></param>
    public void DeleteEventOnState(GameState targetState, Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
    {
        if (StateEvents.TryGetValue(targetState, out var value))
        {
            value.start -= start;
            value.update -= update;
            value.exit -= exit;
        }
    }

    /// <summary>
    /// 게임상태 업데이트문
    /// </summary>
    public void Update()
    {
        if (StateEvents.TryGetValue(cur_GameState, out var cur_State))
        {
            cur_State.update?.Invoke();
        }
    }
    #endregion


    #region Private/Protected Methods
    protected override void Awake()
    {
        base.Awake();
        //이벤트 초기화
        EventInit();
        //게임을 먼저 세팅하는 단계 (필요한 데이터 받아오기 단계)
        SetGameState(GameState.Setting_Game);
        //자기 플레이어의 데이터를 가져옵니다.
        GetPlayerData();
    }

    /// <summary>
    /// 게임매니저 Awake 단계에서 자신의 초기화 진행
    /// </summary>
    private void EventInit()
    {
        StateEvents.Add(GameState.Setting_Game, new GameStateEventTrigger());
        StateEvents.Add(GameState.Start, new GameStateEventTrigger());
        StateEvents.Add(GameState.Prepare, new GameStateEventTrigger());
        StateEvents.Add(GameState.Battle, new GameStateEventTrigger());
        StateEvents.Add(GameState.End, new GameStateEventTrigger());
    }
    private void GetPlayerData()
    {
        //GamePlayer를 새로 생성하여 
        //자신 플레이어 데이터를 기반으로 GamePlayer 초기화 및
        //players[0]에 할당
        //GamePlayer를 추가로 새로 생성하여
        //상대 플레이어 데이터를 가져오기를 시도.
        //내부 비동기 코루틴으로 상대 플레이어 정보를 지속적으로 가져오기를 시도,
        StartCoroutine(TryGetOtherPlayerInfo());
        IEnumerator TryGetOtherPlayerInfo()
        {
            //일정시간마다, 최대대기시간까지 or 상대플레이어 데이터를 받아올 때 까지
            while (true)
            {
                //상대플레이어 데이터 받아오기 시도 (이건 나중작업으로)
                yield return null;
            }
            //성공 시 
            //players[1]에 할당
            //실패 시 서버 접속이 실패됨 (네트워킹 매니저에게 추후 이전될 동작)
            //서버에 대한 호스트/클라 여부를 게임매니저 변수
            server_Authority = Server_authority_Type.Host;
            //에 할당
            //게임 스테이트 이제 스타트로 변경
            SetGameState(GameState.Start);
        }

    }
    #endregion


}
