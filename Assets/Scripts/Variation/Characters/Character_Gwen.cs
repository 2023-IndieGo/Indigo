using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Gwen : Character
{
        public Character_Gwen(GamePlayer player) : base(player)
    {
        var gwenPassive = new GwenPassive();
        passive = gwenPassive;
        passive.Init();

        var gwenUlt = new GwenUltSkill();
        ultimateSkill = gwenUlt;
        ultimateSkill.Init();

        //link
        gwenPassive.OnPassiveCountChange += gwenUlt.OnUltimateSkill;

    }
    [System.Serializable]
    public class GwenPassive : Passive
    {
        private int _passiveCount;
        public int passiveCount
        {
            get => _passiveCount;
            set
            {
                int before = _passiveCount;
                _passiveCount = value;
                if(before != _passiveCount)
                {
                    OnPassiveCountChange?.Invoke(before, value);
                }
            }

        }
        public event OnValueChange<int> OnPassiveCountChange;

                public override void Init()
        {
            base.Init();
            _passiveCount = 0;
            GameManager.instance.events.about_Battle.OnSuccessAttack += PassiveAffect;
        }

        /// <summary>
        /// 가위타입의 공격 성공 시 패시브 스택을 쌓습니다.
        /// </summary>
        /// <param name="mine"></param>
        /// <param name="others"></param>
        private void PassiveAffect(Card mine, Card others)
        {
            if(mine.type == CardType.Scissors)
            {
                SuccessScissorTypeAttack();
            }

        }

        /// <summary>
        /// 그웬의 패시브 효과를 발동시킵니다. 
        /// </summary>
        public void SuccessScissorTypeAttack()
        {
            passiveCount++;
            GameManager.instance.battleConnecter.GetMyPlayer().current_Health += 3;
            GameManager.instance.battleConnecter.GetOtherPlayer().current_Health -= 10;
        }

    }


    [System.Serializable]
    public class GwenUltSkill : UltimateSkill
    {
        bool isActive = false;
        public override void Init()
        {
            base.Init();
        }

        public void OnUltimateSkill(int before, int current)
        {
            if(current >= 5 && !isActive)
            {
                isActive = true;

                GameManager.instance.events.about_GameManager.AddEventOnState(GameState.Prepare, null, null, null);

            }
        }

        private void CreateUltCard()
        {
            //임시카드를 생성합니다.
            GameManager.instance.events.about_GameManager.DeleteEventOnState(GameState.Prepare, CreateUltCard, null, null);
        }

    }
}

