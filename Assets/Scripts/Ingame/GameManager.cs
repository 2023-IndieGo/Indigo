using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;


/// <summary>
/// 인게임상태에서만 새로 생성 후 초기화(Init())되도록
/// 인게임이 종료되면 GameManager
/// null로 바꾸고 이벤트를 모두 날려야함.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    #region Field
    [TitleGroup("Field")]

    [ShowInInspector, LabelText("isHost")]
    public bool isConnectedClientMaster = PhotonNetwork.IsMasterClient;
    [ShowInInspector, LabelText("Server_Authority")]
    public Server_authority_Type server_Authority { get; private set; }


    [ShowInInspector, LabelText("게임 상태")]
    private GameState _cur_GameState;
    /// <summary>
    /// 현재 게임의 상태입니다.
    /// </summary>
    public GameState cur_GameState
    {
        get => _cur_GameState;
    }


    /// <summary>
    /// 준비단계의 진행되고 있는 현재시간입니다.
    /// </summary>
    [LabelText("턴 종료까지의 시간")]
    public float prepare_CurTime;

    /// <summary>
    /// 준비단계 진입 시 각 턴마다의 주어지는 시간입니다.
    /// </summary>
    [ShowInInspector, LabelText("주어진 준비시간"), ReadOnly]
    public float _prepare_maxTime => default_PrepareTime * prepareTime_IncreaseRate * current_Turn;

    private float _default_PrepareTime;
    /// <summary>
    /// 기본 준비단계 시간입니다.
    /// </summary>
    public float default_PrepareTime = 30;

    private float _prepareTime_IncreaseRate = 1.1f;
    /// <summary>
    /// 매 턴마다의 준비시간 증가비율입니다.
    /// </summary>
    public float prepareTime_IncreaseRate;

    [ShowInInspector, LabelText("현재 턴 수")]
    private uint _current_Turn = 0;
    /// <summary>
    /// 현재 턴이 몇 턴인지 알려주는 필드변수입니다.
    /// </summary>
    public uint current_Turn => _current_Turn;

    [ShowInInspector, LabelText("플레이어 데이터")]
    /// <summary>
    /// 현재 게임의 플레이어 데이터 배열입니다. [0] = 나, [1] = 상대방
    /// </summary>
    public GamePlayer[] players = new GamePlayer[2];
    #endregion


    #region Reference
    [TitleGroup("Reference"),  SerializeField]
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

    [SerializeField]
    private BattleConnecter _batteConnecter;
    public BattleConnecter battleConnecter
    {
        get
        {
            if (_batteConnecter == null)
            { _batteConnecter = GameObject.FindObjectOfType<BattleConnecter>(); }
            if (_batteConnecter == null)
            {
                GameObject obj = new GameObject("BattleController");
                _batteConnecter = obj.AddComponent<BattleConnecter>();
            }
            return _batteConnecter;
        }
    }

    public EventTrigger events { get; private set; }

    #endregion


    #region Events


    #endregion


    #region Constructor

    #endregion


    #region Public Methods

    public void Destroy()
    {

    }
    /// <summary>
    /// 게임의 상태를 변경합니다.
    /// 변경된 상태에 따른 이벤트가 실행됩니다.
    /// </summary>
    /// <param name="targetState"></param>
    public void SetGameState(GameState targetState)
    {
        events.about_GameManager.SetGameState(targetState);
        _cur_GameState = targetState;
    }





    /// <summary>
    /// 게임상태 업데이트문
    /// </summary>
    public void Update()
    {
        events.about_GameManager.Update();
    }
    #endregion


    #region Private/Protected Methods
    protected override void Awake()
    {
        base.Awake();
        //자체변수 초기화
        //이벤트 트리거 클래스 초기화
        events = new EventTrigger();

        OnPrepareEventAdd();    //이벤트 등록


        //게임플레이 준비 사전테스트
        //게임을 먼저 세팅하는 단계 (필요한 데이터 받아오기 단계)
        SetGameState(GameState.Setting_Game);
        //자기 플레이어의 데이터를 가져옵니다.
        GetPlayerData();
    }


    private void GetPlayerData()
    {
        //GamePlayer를 새로 생성하여 
        //자신 플레이어 데이터를 기반으로 GamePlayer 초기화 및
        //players[0]에 할당
        GamePlayer pl = new GamePlayer();
        pl.Init();
        battleConnecter.Init(pl);
        //로컬 플레이어의 데이터를 게임매니저의 플레이어 0번에 등록
        players[0] = pl;
        int insertAdress = isConnectedClientMaster ? 0 : 1;
        GamePlayer secPl = battleConnecter.players[insertAdress];


        players[0].current_TurnType = TurnType.Attack_Turn;

        
        //상대 플레이어 데이터를 가져오기를 시도.
        //내부 비동기 코루틴으로 상대 플레이어 정보를 지속적으로 가져오기를 시도,
        StartCoroutine(TryGetOtherPlayerInfo());
        IEnumerator TryGetOtherPlayerInfo()
        {
            float maxWaitTime = 5;
            float currentTime = 0;
            //일정 시간이 지날때까지, 혹은 플레이어 정보가 null이 아닐때까지
            while (currentTime >= maxWaitTime || secPl == null)
            {
                //상대플레이어 데이터 받아오기 시도 => 포톤넷워크
                secPl = battleConnecter.players[insertAdress];
                currentTime += Time.deltaTime;
                yield return null;
            }
            //성공 시 
            //players[1]에 할당
            if (secPl != null)
            {
                players[insertAdress] = secPl;
                secPl.current_TurnType = TurnType.Defence_Turn;

                //서버에 대한 호스트/클라 여부를 게임매니저 변수
                server_Authority = isConnectedClientMaster ? Server_authority_Type.Host : Server_authority_Type.Client;
                //에 할당
                //게임 스테이트 이제 스타트로 변경
                SetGameState(GameState.Start);
            }
            //접속실패
            else
            {
                //실패 시 서버 접속이 실패됨 (네트워킹 매니저에게 추후 이전될 동작)
                Debug.Log($"GamePlayerData를 얻어오지 못했습니다.");
                //자동으로 로비로 돌아가기
            }
            
        }

    }
    #endregion

    #region Private/Protected Method

    private void OnPrepareEventAdd()
    {
        events.about_GameManager.AddEventOnState(GameState.Prepare,
            //Start
            () =>
            {
                _current_Turn++;
                prepare_CurTime = _prepare_maxTime;
            },
            //Update
            () => 
            {
                prepare_CurTime -= Time.deltaTime;
                if(prepare_CurTime <= 0)
                {
                    SetGameState(GameState.Battle);
                }
            },
            //Exit
            () =>
            {
                prepare_CurTime = 0;
            }
            );
    }

    #endregion
}
