using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : BaseModel
{
    #region Field
    public CardType type;
    public UnitWhere unitWhere;
    public string name;

    public Sprite AttackImage;
    public string Attack_explain;
    protected int attackValue;
    public int AttackValue
    {
        get => attackValue;
        set
        {
            int before = attackValue;
            attackValue = value;
            if(value != before)
            {
                OnAttackValue_Change?.Invoke(before, value);
            }
        }
    }
    public OnValueChange<int> OnAttackValue_Change;


    public Sprite DefenceImage;
    public string Defence_explain;
    protected int defenceValue;
    public int DefenceValue
    {
        get => defenceValue;
        set
        {
            int before = defenceValue;
            defenceValue = value;
            if (value != before)
            {
                OnDefenceValue_Change?.Invoke(before, value);
            }
        }
    }
    public OnValueChange<int> OnDefenceValue_Change;



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


    /// <summary>
    /// 공격효과 (파티클 아님)
    /// </summary>
    /// <param name="target"></param>
    public virtual void AttackEffect(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(이)가 {target}을 때리며 공격효과를 발생시킵니다.");
    }

    public virtual void AttackFail(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(이)가 {target}을 때리는데 실패하며 공격실패효과를 발생시킵니다.");
    }
    /// <summary>
    /// 방어효과 (파티클 아님)
    /// </summary>
    /// <param name="target"></param>
    public virtual void DefenceEffect(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(이)가 {target}으로부터 방어에 성공하며 방어효과를 발생시킵니다.");
    }

    public virtual void DefenceFail(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(이)가 {target}을 때리며 방어실패효과를 발생시킵니다.");
    }

    #endregion


    #region Private/Protected Methods
    #endregion
}
