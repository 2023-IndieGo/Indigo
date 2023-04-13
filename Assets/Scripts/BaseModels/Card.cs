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


    /// <summary>
    /// ����ȿ�� (��ƼŬ �ƴ�)
    /// </summary>
    /// <param name="target"></param>
    public virtual void AttackEffect(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(��)�� {target}�� ������ ����ȿ���� �߻���ŵ�ϴ�.");
    }

    public virtual void AttackFail(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(��)�� {target}�� �����µ� �����ϸ� ���ݽ���ȿ���� �߻���ŵ�ϴ�.");
    }
    /// <summary>
    /// ���ȿ�� (��ƼŬ �ƴ�)
    /// </summary>
    /// <param name="target"></param>
    public virtual void DefenceEffect(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(��)�� {target}���κ��� �� �����ϸ� ���ȿ���� �߻���ŵ�ϴ�.");
    }

    public virtual void DefenceFail(GamePlayer target, GamePlayer who)
    {
        Debug.Log($"{this.name}(��)�� {target}�� ������ ������ȿ���� �߻���ŵ�ϴ�.");
    }

    #endregion


    #region Private/Protected Methods
    #endregion
}
