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
                for (int i = 0; i < default_DrawCard_Count; i++)
                {
                    deckData.DrawRandomCard_ToHand();
                }
            }
        );
    }
    #endregion


    #region Public Methods
    #endregion


    #region Private/Protected Methods
    #endregion
}
