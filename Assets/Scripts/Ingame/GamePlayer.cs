using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class GamePlayer
{
    #region Field / Properties / Event

    private int _default_DrawCard_Attack;
    /// <summary>
    /// 공격 턴 시 뽑는 카드의 수
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
    /// 공격턴 시 뽑는 카드의 수가 변화할 경우 발생하는 이벤트
    /// </summary>
    public event OnValueChange<int> On_default_DrawCard_Attack_Value_Change;


    private int _default_DrawCard_Defence;
    /// <summary>
    /// 방어 턴 시 뽑는 카드의 수
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
    /// 방어턴 시 뽑는 카드의 수가 변화할 경우 발생하는 이벤트
    /// </summary>
    public event OnValueChange<int> On_default_DrawCard_Defence_Value_Change;


    private int _current_Health;
    /// <summary>
    /// 현재 체력
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
    /// 현재체력 변화시 발생하는 이벤트
    /// </summary>
    public event OnValueChange<int> On_Current_Health_Value_Change;


    private int _max_Health;
    /// <summary>
    /// 기본 체력
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
    /// 기본체력 변화시 발생하는 이벤트
    /// </summary>
    public event OnValueChange<int> On_Max_Health_Value_Change;


    private int _current_Shield;
    /// <summary>
    /// 현재 방어막
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
    /// 현재 방어막 변화시 발생하는 이벤트
    /// </summary>
    public event OnValueChange<int> On_Current_Shield_Value_Change;

    /****주석처리 : 최대방어막****
    private int _max_Shield;
    /// <summary>
    /// 최대 방어막
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
    /// 최대 방어막 변화시 발생하는 이벤트
    /// </summary>
    public event OnValueChange<int> On_Max_Shield_Value_Change;
    */


    private TurnType _current_TurnType;
    /// <summary>
    /// 현재 플레이어의 턴타입
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
    /// 플레이어의 턴타입이 바뀌면 발동하는 이벤트 메서드
    /// </summary>
    public event OnValueChange<TurnType> On_TurnType_Change;


    #endregion



    #region Events

    /// <summary>
    /// 플레이어의 체력이 0이 되면 발생하는 이벤트
    /// !주의! 게임끝이 아님!
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
