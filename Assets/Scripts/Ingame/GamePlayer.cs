using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public partial class GamePlayer
{
    #region Field / Properties / Event


    private int _default_DrawCard_Count;
    /// <summary>
    /// ��� �� �� �̴� ī���� ��
    /// </summary>
    public int default_DrawCard_Count
    {
        get => _default_DrawCard_Count; set
        {
            int before = _default_DrawCard_Count;
            _default_DrawCard_Count = value;
            if (value != before)
            {
                On_default_DrawCard_Count_Value_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// ����� �� �̴� ī���� ���� ��ȭ�� ��� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public OnValueChange<int> On_default_DrawCard_Count_Value_Change;


    private int _current_Health;
    /// <summary>
    /// ���� ü��
    /// </summary>
    public int current_Health
    {
        get => _current_Health; set
        {
            int before = _current_Health;
            _current_Health = value;
            if (value != before)
            {
                On_Current_Health_Value_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// ����ü�� ��ȭ�� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public OnValueChange<int> On_Current_Health_Value_Change;


    private int _max_Health = 30;
    /// <summary>
    /// �⺻ ü��
    /// </summary>
    public int max_Health
    {
        get => _max_Health; set
        {
            int before = _max_Health;
            _max_Health = value;
            if (value != before)
            {
                On_Max_Health_Value_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// �⺻ü�� ��ȭ�� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public OnValueChange<int> On_Max_Health_Value_Change;


    private int _current_Shield;
    /// <summary>
    /// ���� ��
    /// </summary>
    public int current_Shield
    {
        get => _current_Shield; set
        {
            int before = _current_Shield;
            _current_Shield = value;
            if (value != before)
            {
                On_Current_Shield_Value_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// ���� �� ��ȭ�� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public OnValueChange<int> On_Current_Shield_Value_Change;

    /****�ּ�ó�� : �ִ��****
    private int _max_Shield;
    /// <summary>
    /// �ִ� ��
    /// </summary>
    public int max_Shield
    {
        get => _max_Shield; set
        {
            int before = _max_Shield;
            _max_Shield = value;
            if (value != before)
            {
                On_Max_Shield_Value_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// �ִ� �� ��ȭ�� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event OnValueChange<int> On_Max_Shield_Value_Change;
    */

    [SerializeField, LabelText("�� Ÿ��")]
    private TurnType _current_TurnType;
    /// <summary>
    /// ���� �÷��̾��� ��Ÿ��
    /// </summary>
    public TurnType current_TurnType
    {
        get => _current_TurnType;
        set
        {
            TurnType before = _current_TurnType;
            _current_TurnType = value;
            if (before != value)
            {
                On_TurnType_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// �÷��̾��� ��Ÿ���� �ٲ�� �ߵ��ϴ� �̺�Ʈ �޼���
    /// </summary>
    public OnValueChange<TurnType> On_TurnType_Change;

    private CardType _lastOpenCardType;
    /// <summary>
    /// �÷��̾ ���� �ֱٿ� �� ī���� Ÿ��
    /// </summary>
    public CardType lastOpenCardType
    {
        get => _lastOpenCardType;
        set
        {
            CardType before = _lastOpenCardType;
            _lastOpenCardType = value;
            if(before != value)
            {
                On_OpenCard_LastOpenCardValueChange?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// �÷��̾��� ��Ÿ���� �ٲ�� �ߵ��ϴ� �̺�Ʈ �޼���
    /// </summary>
    public OnValueChange<CardType> On_OpenCard_LastOpenCardValueChange;

    #endregion


    #region Constructor
    public GamePlayer()
    {
        //�÷��̾� �ʵ庯�� �ʱ�ȭ
        default_DrawCard_Count = 5;
        current_Health = max_Health;
        current_Shield = 0;

        //�÷��̾� ���� �ʱ�ȭ
        //���� �κ񿡼� PlayerData�� �޾ƿ� �ε��ϵ���
        deckData = new Deck(this);
        hand = new Hand(this);
        trash = new TrashCan(this);
        field = new Field(this);
        character = new Character(this);

        BuffList = new Dictionary<Buff, List<Buff>>();
        DebuffList = new Dictionary<Debuff, List<Debuff>>();

        //�ʱ�ȭ �Ǹ� �ʿ䵥���Ͱ� �ڵ��ʱ�ȭ

        Debug.Log($"�÷��̾� ���� ����");

        
    }
    public void Init()
    {
        //������� �̺�Ʈ �߰�
        GameManager.instance.events.about_GameManager.AddEventOnState(GameState.Prepare,
            //start
            () =>
            {
                //�غ�ܰ� ���� �� ī�带 �����κ��� �����ϴ�.
                for (int i = 0; i < default_DrawCard_Count; i++)
                {
                    deckData.DrawRandomCard_ToHand();
                }
            },
            null,
            OnFirstTurn_PrepareMethod
        );
    }
    #endregion


    #region Public Methods
    #endregion


    #region Private/Protected Methods
    /// <summary>
    /// ù�Ͽ�, �÷��̾ ī�带 ���� �ʴ� ��쿡 ���ؼ� ����ó���� �մϴ�.
    /// ������ ������ ī�尡 ��ġ�ǵ��� �����մϴ�.
    /// </summary>
    private void OnFirstTurn_PrepareMethod()
    {
        //ù���� ��Ʋ��� ���� ��
        //�ʵ� 0��°�� ����ִٸ� ������ī�带 �ʵ忡 ���ϴ�.
        if (GameManager.instance.current_Turn == 0)
        {
            //ù���� ù��° ������ �ʵ尡 ���������
            int adress = this.current_TurnType == TurnType.Attack_Turn ? 0 : 1;
            if(this.field.zones[0][adress].currentCard == null)
            {
                //�ڵ忡�� ������ ī�带 �ش��ʵ忡 ���ϴ�.
                int random = Random.Range(0, this.hand.cards.Count);
                this.field.zones[0][adress].LocatedCard(this.hand.cards[random]);
            }
        }
        //�޼��� ����� �����մϴ�.
        GameManager.instance.events.about_GameManager.DeleteEventOnState(GameState.Prepare, null, null, OnFirstTurn_PrepareMethod);
    }
    #endregion
}
