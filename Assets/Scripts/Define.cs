using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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


/// <summary>
/// 값이 변화할 경우 실행되는 델리게이트 메서드
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="before"></param>
/// <param name="after"></param>
public delegate void OnValueChange<T>(T before, T after);

public delegate void Del_NoRet_NoParams();


public enum TurnType { Attack_Turn, Defence_Turn }
public class Define : MonoBehaviour
{
}
