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
/// ���� ��ȭ�� ��� ����Ǵ� ��������Ʈ �޼���
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
