using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;


/// <summary>
/// �ΰ��ӻ��¿����� ���� ���� �� �ʱ�ȭ(Init())�ǵ���
/// �ΰ����� ����Ǹ� GameManager
/// null�� �ٲٰ� �̺�Ʈ�� ��� ��������.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    #region Field
    [TitleGroup("Field")]

    [ShowInInspector, LabelText("isHost")]
    public bool isConnectedClientMaster = PhotonNetwork.IsMasterClient;
    [ShowInInspector, LabelText("Server_Authority")]
    public Server_authority_Type server_Authority { get; private set; }


    [ShowInInspector, LabelText("���� ����")]
    private GameState _cur_GameState;
    /// <summary>
    /// ���� ������ �����Դϴ�.
    /// </summary>
    public GameState cur_GameState
    {
        get => _cur_GameState;
    }


    /// <summary>
    /// �غ�ܰ��� ����ǰ� �ִ� ����ð��Դϴ�.
    /// </summary>
    [LabelText("�� ��������� �ð�")]
    public float prepare_CurTime;

    /// <summary>
    /// �غ�ܰ� ���� �� �� �ϸ����� �־����� �ð��Դϴ�.
    /// </summary>
    [ShowInInspector, LabelText("�־��� �غ�ð�"), ReadOnly]
    public float _prepare_maxTime => default_PrepareTime * prepareTime_IncreaseRate * current_Turn;

    private float _default_PrepareTime;
    /// <summary>
    /// �⺻ �غ�ܰ� �ð��Դϴ�.
    /// </summary>
    public float default_PrepareTime = 30;

    private float _prepareTime_IncreaseRate = 1.1f;
    /// <summary>
    /// �� �ϸ����� �غ�ð� ���������Դϴ�.
    /// </summary>
    public float prepareTime_IncreaseRate;

    [ShowInInspector, LabelText("���� �� ��")]
    private uint _current_Turn = 0;
    /// <summary>
    /// ���� ���� �� ������ �˷��ִ� �ʵ庯���Դϴ�.
    /// </summary>
    public uint current_Turn => _current_Turn;

    [ShowInInspector, LabelText("�÷��̾� ������")]
    /// <summary>
    /// ���� ������ �÷��̾� ������ �迭�Դϴ�. [0] = ��, [1] = ����
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
    /// ������ ���¸� �����մϴ�.
    /// ����� ���¿� ���� �̺�Ʈ�� ����˴ϴ�.
    /// </summary>
    /// <param name="targetState"></param>
    public void SetGameState(GameState targetState)
    {
        events.about_GameManager.SetGameState(targetState);
        _cur_GameState = targetState;
    }





    /// <summary>
    /// ���ӻ��� ������Ʈ��
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
        //��ü���� �ʱ�ȭ
        //�̺�Ʈ Ʈ���� Ŭ���� �ʱ�ȭ
        events = new EventTrigger();

        OnPrepareEventAdd();    //�̺�Ʈ ���


        //�����÷��� �غ� �����׽�Ʈ
        //������ ���� �����ϴ� �ܰ� (�ʿ��� ������ �޾ƿ��� �ܰ�)
        SetGameState(GameState.Setting_Game);
        //�ڱ� �÷��̾��� �����͸� �����ɴϴ�.
        GetPlayerData();
    }


    private void GetPlayerData()
    {
        //GamePlayer�� ���� �����Ͽ� 
        //�ڽ� �÷��̾� �����͸� ������� GamePlayer �ʱ�ȭ ��
        //players[0]�� �Ҵ�
        GamePlayer pl = new GamePlayer();
        pl.Init();
        battleConnecter.Init(pl);
        //���� �÷��̾��� �����͸� ���ӸŴ����� �÷��̾� 0���� ���
        players[0] = pl;
        int insertAdress = isConnectedClientMaster ? 0 : 1;
        GamePlayer secPl = battleConnecter.players[insertAdress];


        players[0].current_TurnType = TurnType.Attack_Turn;

        
        //��� �÷��̾� �����͸� �������⸦ �õ�.
        //���� �񵿱� �ڷ�ƾ���� ��� �÷��̾� ������ ���������� �������⸦ �õ�,
        StartCoroutine(TryGetOtherPlayerInfo());
        IEnumerator TryGetOtherPlayerInfo()
        {
            float maxWaitTime = 5;
            float currentTime = 0;
            //���� �ð��� ����������, Ȥ�� �÷��̾� ������ null�� �ƴҶ�����
            while (currentTime >= maxWaitTime || secPl == null)
            {
                //����÷��̾� ������ �޾ƿ��� �õ� => ����ݿ�ũ
                secPl = battleConnecter.players[insertAdress];
                currentTime += Time.deltaTime;
                yield return null;
            }
            //���� �� 
            //players[1]�� �Ҵ�
            if (secPl != null)
            {
                players[insertAdress] = secPl;
                secPl.current_TurnType = TurnType.Defence_Turn;

                //������ ���� ȣ��Ʈ/Ŭ�� ���θ� ���ӸŴ��� ����
                server_Authority = isConnectedClientMaster ? Server_authority_Type.Host : Server_authority_Type.Client;
                //�� �Ҵ�
                //���� ������Ʈ ���� ��ŸƮ�� ����
                SetGameState(GameState.Start);
            }
            //���ӽ���
            else
            {
                //���� �� ���� ������ ���е� (��Ʈ��ŷ �Ŵ������� ���� ������ ����)
                Debug.Log($"GamePlayerData�� ������ ���߽��ϴ�.");
                //�ڵ����� �κ�� ���ư���
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
