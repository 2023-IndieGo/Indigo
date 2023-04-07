using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ΰ��ӻ��¿����� ���� ���� �� �ʱ�ȭ(Init())�ǵ���
/// �ΰ����� ����Ǹ� GameManager
/// null�� �ٲٰ� �̺�Ʈ�� ��� ��������.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    #region Field
    private GameState _cur_GameState;
    /// <summary>
    /// ���� ������ �����Դϴ�.
    /// </summary>
    public GameState cur_GameState
    {
        get => cur_GameState;
    }

    public Server_authority_Type server_Authority { get; private set; }

    /// <summary>
    /// �ΰ������� �ƴ��� ��Ÿ���� �Ҹ� ����:
    /// true : �ΰ��� ___/___
    /// false : �ΰ����� �ƴ�
    /// </summary>
    public bool isGaming = false;

    private float _prepare_CurTime;
    /// <summary>
    /// �غ�ܰ��� ����ǰ� �ִ� ����ð��Դϴ�.
    /// </summary>
    public float prepare_CurTime;

    /// <summary>
    /// �غ�ܰ� ���� �� �� �ϸ����� �־����� �ð��Դϴ�.
    /// </summary>
    public float _prepare_maxTime => default_PrepareTime * prepareTime_IncreaseRate * current_Turn;

    private float _default_PrepareTime;
    /// <summary>
    /// �⺻ �غ�ܰ� �ð��Դϴ�.
    /// </summary>
    public float default_PrepareTime;

    private float _prepareTime_IncreaseRate = 1.1f;
    /// <summary>
    /// �� �ϸ����� �غ�ð� ���������Դϴ�.
    /// </summary>
    public float prepareTime_IncreaseRate;

    private uint _current_Turn = 1;
    /// <summary>
    /// ���� ���� �� ������ �˷��ִ� �ʵ庯���Դϴ�.
    /// </summary>
    public uint current_Turn => _current_Turn;

    /// <summary>
    /// ���� ������ �÷��̾� ������ �迭�Դϴ�. [0] = ��, [1] = ����
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
    /// ������ ���¸� �����մϴ�.
    /// ����� ���¿� ���� �̺�Ʈ�� ����˴ϴ�.
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
    /// �ش� ���ӽ�����Ʈ�� �Ű����� �޼��带 ����մϴ�.
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
    /// �ش� ���ӽ�����Ʈ�� �Ű����� �޼��带 �����մϴ�.
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
    /// ���ӻ��� ������Ʈ��
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
        //�̺�Ʈ �ʱ�ȭ
        EventInit();
        //������ ���� �����ϴ� �ܰ� (�ʿ��� ������ �޾ƿ��� �ܰ�)
        SetGameState(GameState.Setting_Game);
        //�ڱ� �÷��̾��� �����͸� �����ɴϴ�.
        GetPlayerData();
    }

    /// <summary>
    /// ���ӸŴ��� Awake �ܰ迡�� �ڽ��� �ʱ�ȭ ����
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
        //GamePlayer�� ���� �����Ͽ� 
        //�ڽ� �÷��̾� �����͸� ������� GamePlayer �ʱ�ȭ ��
        //players[0]�� �Ҵ�
        //GamePlayer�� �߰��� ���� �����Ͽ�
        //��� �÷��̾� �����͸� �������⸦ �õ�.
        //���� �񵿱� �ڷ�ƾ���� ��� �÷��̾� ������ ���������� �������⸦ �õ�,
        StartCoroutine(TryGetOtherPlayerInfo());
        IEnumerator TryGetOtherPlayerInfo()
        {
            //�����ð�����, �ִ���ð����� or ����÷��̾� �����͸� �޾ƿ� �� ����
            while (true)
            {
                //����÷��̾� ������ �޾ƿ��� �õ� (�̰� �����۾�����)
                yield return null;
            }
            //���� �� 
            //players[1]�� �Ҵ�
            //���� �� ���� ������ ���е� (��Ʈ��ŷ �Ŵ������� ���� ������ ����)
            //������ ���� ȣ��Ʈ/Ŭ�� ���θ� ���ӸŴ��� ����
            server_Authority = Server_authority_Type.Host;
            //�� �Ҵ�
            //���� ������Ʈ ���� ��ŸƮ�� ����
            SetGameState(GameState.Start);
        }

    }
    #endregion


}
