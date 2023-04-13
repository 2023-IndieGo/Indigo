using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//복붙해서 쓰면
#region Field
#endregion


#region Properties
#endregion


#region Events
#endregion


#region Constructor
#endregion


#region Public Methods
#endregion


#region Private/Protected Methods
#endregion
//스크립트가 깔끔해져요 *^^*



/// <summary>
/// 값이 변화할 경우 실행되는 델리게이트 메서드
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="before"></param>
/// <param name="after"></param>
public delegate void OnValueChange<T>(T before, T after);
public delegate void Del_NoRet_1_Params<T>(T value);
public delegate void Del_NoRet_2_Params<T, U>(T first, U second);

public delegate void Del_NoRet_NoParams();

public enum Server_authority_Type
{
    Host,
    Client
}

public enum AnimationType
{
    /// <summary>
    /// 애니메이션
    /// </summary>
    Animation,
    /// <summary>
    /// 이펙트
    /// </summary>
    Particle,
    /// <summary>
    /// 애니메이션, 이펙트 둘다
    /// </summary>
    Both,
    /// <summary>
    /// 재생X (대기)
    /// </summary>
    None
}

public enum TurnType 
{ 
    Attack_Turn,
    Defence_Turn 
}

public enum GameState
{
    Start = 0,
    Prepare,
    Battle,
    End,
    Setting_Game = 100
}

public enum CardType
{
    Scissors,
    Rock,
    Paper,
}
public enum CardBattleResult
{
    Success,
    Fail
}

public enum UnitWhere
{
    Deck,
    Hand,
    Trash,
    Field
}

public enum ZoneType
{
    Attack,
    Defence
}
public class Define : MonoBehaviour
{
}
