using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : BaseModel
{
    #region Field
    public CardType type;
    public UnitWhere unitWhere;
    public string name;
    public string Attack_explain;
    public string Defence_explain;



    #endregion


    #region Properties
    #endregion


    #region Events
    #endregion


    #region Constructor
    #endregion


    #region Public Methods
    public override void Init()
    {
        base.Init();
    }

    public virtual void OnDrawing()
    {
        Debug.Log($"������ {this.name}�� �̾ҽ��ϴ�.");
    }

    public virtual void OnThrowAway()
    {
        Debug.Log($"{this.name}�� ���Ƚ��ϴ�.");
    }

    /// <summary>
    /// �ʵ��� ������ ���� �Ű������� �� ���� �ֽ��ϴ�.
    /// </summary>
    public virtual void OnFieldDraw()
    {
        Debug.Log($"{this.name}�� �ʵ忡 �½��ϴ�.");
    }


    #endregion


    #region Private/Protected Methods
    #endregion
}
