using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Aura
{
    public string name;
    public string explain;
    private GamePlayer _player;
    public GamePlayer player => player;
    /// <summary>
    /// 아우라 이벤트가 플레이어에게 더해질 때 버프핸들러 이벤트를 통해 호출됩니다.
    /// </summary>
    public virtual void Init(GamePlayer _player)
    {
        this._player = _player;
    }

    /// <summary>
    /// 아우라 이벤트가 플레이어로부터 사라질 때 버프 핸들러 이벤트를 통해 호출됩니다.
    /// 사라질때의 이벤트핸들은 아우라 하위클래스 내부에서 이벤트에 의해 호출되도록 합니다.
    /// </summary>
    public virtual void OnEnded()
    {

    }
}

public class Buff : Aura
{
    public override void Init(GamePlayer _player)
    {
        base.Init(_player);
        GameManager.instance.events.about_Aura.GetPlayerBuff(_player, this);
        Debug.Log($"{_player}에게 버프 :{this.name} 을(를) 적용");
    }
}

public class Debuff : Aura
{
    public override void Init(GamePlayer _player)
    {
        base.Init(_player);
        GameManager.instance.events.about_Aura.GetPlayerDebuff(_player, this);
        Debug.Log($"{_player}에게 디버프 :{this.name} 을(를) 적용");
    }
}
