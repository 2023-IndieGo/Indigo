using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : BaseModel
{
    #region Field
    public GamePlayer owner;
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
    /// ?????? ???????? ??????????.
    /// ???????? ?????? ????
    /// </summary>
    public virtual void OnDrawing()
    {
        Debug.Log($"?????? {this.name}?? ??????????.");

    }

    /// <summary>
    /// ?????? ?????????? ??????????.
    /// ???????? ?????? ????
    /// </summary>
    public virtual void OnThrowAway()
    {
        Debug.Log($"{this.name}?? ??????????.");
    }

    /// <summary>
    /// ???? ?????? ?????????? ???? ????????.
    /// ???????? ?????? ????
    /// </summary>
    public virtual void OnFieldDraw(Field.Zone adress)
    {
        Debug.Log($"{this.name}?? ?????? ????????.");
    }


    /// <summary>
    /// ???????? (?????? ????)
    /// </summary>
    /// <param name="target"></param>
    public virtual void AttackSuccess(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(??)?? {target}?? ?????? ?????????? ????????????.");
    }

    public virtual void AttackFail(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(??)?? {target}?? ???????? ???????? ?????????????? ????????????.");
    }
    /// <summary>
    /// ???????? (?????? ????)
    /// </summary>
    /// <param name="target"></param>
    public virtual void DefenceEffect(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(??)?? {target}???????? ?????? ???????? ?????????? ????????????.");
    }

    public virtual void DefenceFail(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(??)?? {target}?? ?????? ?????????????? ????????????.");
    }

    #endregion


    #region Private/Protected Methods
    #endregion
}
