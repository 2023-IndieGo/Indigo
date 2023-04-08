using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class GamePlayer
{
    #region Field / Properties / Event


    private int _default_DrawCard_Count;
    /// <summary>
    /// 방어 턴 시 뽑는 카드의 수
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
    /// 방어턴 시 뽑는 카드의 수가 변화할 경우 발생하는 이벤트
    /// </summary>
    public OnValueChange<int> On_default_DrawCard_Count_Value_Change;


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
        }
    }
    /// <summary>
    /// 현재체력 변화시 발생하는 이벤트
    /// </summary>
    public OnValueChange<int> On_Current_Health_Value_Change;


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
    public OnValueChange<int> On_Max_Health_Value_Change;


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
    public OnValueChange<int> On_Current_Shield_Value_Change;

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
    public OnValueChange<TurnType> On_TurnType_Change;


    #endregion


    #region Constructor

    #endregion


    #region Public Methods
    #endregion


    #region Private/Protected Methods
    #endregion
}
