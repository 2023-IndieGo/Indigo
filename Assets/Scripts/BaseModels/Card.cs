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
        Debug.Log($"덱에서 {this.name}을 뽑았습니다.");
    }

    public virtual void OnThrowAway()
    {
        Debug.Log($"{this.name}을 버렸습니다.");
    }

    /// <summary>
    /// 필드의 구성에 따라 매개변수가 들어갈 수도 있습니다.
    /// </summary>
    public virtual void OnFieldDraw()
    {
        Debug.Log($"{this.name}을 필드에 냈습니다.");
    }


    #endregion


    #region Private/Protected Methods
    #endregion
}
