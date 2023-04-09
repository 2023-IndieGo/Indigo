using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


/// <summary>
/// �ΰ��ӻ��¿����� ���� ���� �� �ʱ�ȭ(Init())�ǵ���
/// �ΰ����� ����Ǹ� GameManager
/// null�� �ٲٰ� �̺�Ʈ�� ��� ��������.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    #region Field

    private bool isConnectedClientMaster = PhotonNetwork.IsMasterClient;

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
        players[0] = isConnectedClientMaster ? pl : null;
        int insertAdress = isConnectedClientMaster ? 1 : 0;
        GamePlayer secPl = isConnectedClientMaster ? players[1] : players[0];
        //��� �÷��̾� �����͸� �������⸦ �õ�.
        //���� �񵿱� �ڷ�ƾ���� ��� �÷��̾� ������ ���������� �������⸦ �õ�,
        StartCoroutine(TryGetOtherPlayerInfo());
        IEnumerator TryGetOtherPlayerInfo()
        {
            float maxWaitTime = 5;
            float currentTime = 0;
            //���� �ð��� ����������, Ȥ�� �÷��̾� ������ null�� �ƴҶ�����
            while (currentTime >= maxWaitTime || secPl != null)
            {

                //����÷��̾� ������ �޾ƿ��� �õ� => ����ݿ�ũ

                currentTime += Time.deltaTime;
                yield return null;
            }
            //���� �� 
            //players[1]�� �Ҵ�
            if (secPl != null)
            {
                players[insertAdress] = secPl;
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
                //�ڵ����� �κ�� ���ư���
            }
            
        }

    }
    #endregion


}
