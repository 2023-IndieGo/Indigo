using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class GamePlayer
{
    #region Field / Properties / Event

    private int _default_DrawCard_Attack;
    /// <summary>
    /// ���� �� �� �̴� ī���� ��
    /// </summary>
    public int default_DrawCard_Attack { get => _default_DrawCard_Attack; set
        {
            int before = _default_DrawCard_Attack;
            _default_DrawCard_Attack = value;
            if(value != before)
            {
                On_default_DrawCard_Attack_Value_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// ������ �� �̴� ī���� ���� ��ȭ�� ��� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event OnValueChange<int> On_default_DrawCard_Attack_Value_Change;


    private int _default_DrawCard_Defence;
    /// <summary>
    /// ��� �� �� �̴� ī���� ��
    /// </summary>
    public int default_DrawCard_Defence
    {
        get => _default_DrawCard_Defence; set
        {
            int before = _default_DrawCard_Defence;
            _default_DrawCard_Defence = value;
            if (value != before)
            {
                On_default_DrawCard_Defence_Value_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// ����� �� �̴� ī���� ���� ��ȭ�� ��� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event OnValueChange<int> On_default_DrawCard_Defence_Value_Change;


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
            if(current_Health <= 0)
            {
                OnPlayer_Die_Event?.Invoke();
            }
        }
    }
    /// <summary>
    /// ����ü�� ��ȭ�� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event OnValueChange<int> On_Current_Health_Value_Change;


    private int _max_Health;
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
    public event OnValueChange<int> On_Max_Health_Value_Change;


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
    public event OnValueChange<int> On_Current_Shield_Value_Change;

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
            current_TurnType = value;
            if(before != value)
            {
                On_TurnType_Change?.Invoke(before, value);
            }
        }
    }
    /// <summary>
    /// �÷��̾��� ��Ÿ���� �ٲ�� �ߵ��ϴ� �̺�Ʈ �޼���
    /// </summary>
    public event OnValueChange<TurnType> On_TurnType_Change;


    #endregion



    #region Events

    /// <summary>
    /// �÷��̾��� ü���� 0�� �Ǹ� �߻��ϴ� �̺�Ʈ
    /// !����! ���ӳ��� �ƴ�!
    /// </summary>
    public event Del_NoRet_NoParams OnPlayer_Die_Event;

    public event Del_NoRet_NoParams OnPlayer_Card_FieldDrop;

    public event Del_NoRet_1_Params<Card> OnPlayer_DrawCard;

    public event Del_NoRet_1_Params<Card> OnPlayer_ThrowCard;

    

    #endregion


    #region Constructor

    #endregion


    #region Public Methods
    #endregion


    #region Private/Protected Methods
    #endregion
}
