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
    /// 카드를 뽑으면서 호출됩니다.
    /// 자체적인 효과만 허용
    /// </summary>
    public virtual void OnDrawing()
    {
        Debug.Log($"덱에서 {this.name}을 뽑았습니다.");

    }

    /// <summary>
    /// 카드가 버려지면서 호출됩니다.
    /// 자체적인 효과만 허용
    /// </summary>
    public virtual void OnThrowAway()
    {
        Debug.Log($"{this.name}을 버렸습니다.");
    }

    /// <summary>
    /// 해당 카드가 매개변수값 존에 내집니다.
    /// 자체적인 효과만 허용
    /// </summary>
    public virtual void OnFieldDraw(Field.Zone adress)
    {
        GameManager.instance.events.about_Field.OnLocatedCard_At_Zone?.Invoke(this, adress);
        Debug.Log($"{this.name}을 필드에 냈습니다.");
    }


    #endregion


    #region Private/Protected Methods
    #endregion
}
