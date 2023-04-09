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

    /// <summary>
    /// ī�带 �����鼭 ȣ��˴ϴ�.
    /// ��ü���� ȿ���� ���
    /// </summary>
    public virtual void OnDrawing()
    {
        Debug.Log($"������ {this.name}�� �̾ҽ��ϴ�.");

    }

    /// <summary>
    /// ī�尡 �������鼭 ȣ��˴ϴ�.
    /// ��ü���� ȿ���� ���
    /// </summary>
    public virtual void OnThrowAway()
    {
        Debug.Log($"{this.name}�� ���Ƚ��ϴ�.");
    }

    /// <summary>
    /// �ش� ī�尡 �Ű������� ���� �����ϴ�.
    /// ��ü���� ȿ���� ���
    /// </summary>
    public virtual void OnFieldDraw(Field.Zone adress)
    {
        GameManager.instance.events.about_Field.OnLocatedCard_At_Zone?.Invoke(this, adress);
        Debug.Log($"{this.name}�� �ʵ忡 �½��ϴ�.");
    }


    #endregion


    #region Private/Protected Methods
    #endregion
}
