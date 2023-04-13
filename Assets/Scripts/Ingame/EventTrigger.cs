using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// ���� ���� ��� �̺�Ʈ�� ����, ��������Ʈ�� �����մϴ�.
/// ��������Ʈ ���, ������ �� �������ּ���.
/// </summary>
/// 
[System.Serializable]
public class EventTrigger
{
    public void Init()
    {
        about_Battle = new BattleEventHandler();
        about_Deck = new DeckEventHandler();
        about_GameManager = new GameManagerEventHanler();
        about_Trash = new TrashEventHandler();
        about_Hand = new HandEventHandler();
        about_Card = new CardEventHandler();

        about_Aura = new BuffEventHandler();
    }
    /// <summary>
    /// ���ӸŴ����� ���� �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public GameManagerEventHanler about_GameManager;
    [System.Serializable]
    public class GameManagerEventHanler
    {
        private GameState cur_GameState;
        public Dictionary<GameState, GameStateEventTrigger> StateEvents;
        [System.Serializable]
        public class GameStateEventTrigger
        {
            public Del_NoRet_NoParams start;
            public Del_NoRet_NoParams update;
            public Del_NoRet_NoParams exit;

            public GameStateEventTrigger(Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
            {
                this.start += start;
                this.update += update;
                this.exit += exit;
            }
        }

        public GameManagerEventHanler()
        {
            StateEvents = new Dictionary<GameState, GameStateEventTrigger>();
            StateEvents.Add(GameState.Setting_Game, new GameStateEventTrigger());
            StateEvents.Add(GameState.Start, new GameStateEventTrigger());
            StateEvents.Add(GameState.Prepare, new GameStateEventTrigger());
            StateEvents.Add(GameState.Battle, new GameStateEventTrigger());
            StateEvents.Add(GameState.End, new GameStateEventTrigger());
        }

        public void SetGameState(GameState targetState)
        {
            if (StateEvents.TryGetValue(targetState, out var cur_State))
            {
                cur_State.exit?.Invoke();
            }
            if (StateEvents.TryGetValue(targetState, out var next_State))
            {
                next_State.start?.Invoke();
            }
            cur_GameState = targetState;
        }

        /// <summary>
        /// �ش� ���ӽ�����Ʈ�� �Ű����� �޼��带 ����մϴ�.
        /// </summary>
        /// <param name="targetStage"></param>
        /// <param name="start"></param>
        /// <param name="update"></param>
        /// <param name="exit"></param>
        public void AddEventOnState(GameState targetState, Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
        {
            if (StateEvents.TryGetValue(targetState, out var value))
            {
                value.start += start;
                value.update += update;
                value.exit += exit;
            }
            else
            {
                StateEvents.Add(targetState, new GameStateEventTrigger(start, update, exit));
            }
        }

        /// <summary>
        /// �ش� ���ӽ�����Ʈ�� �Ű����� �޼��带 �����մϴ�.
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="start"></param>
        /// <param name="update"></param>
        /// <param name="exit"></param>
        public void DeleteEventOnState(GameState targetState, Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
        {
            if (StateEvents.TryGetValue(targetState, out var value))
            {
                value.start -= start;
                value.update -= update;
                value.exit -= exit;
            }
        }

        /// <summary>
        /// ���ӻ��� ������Ʈ��
        /// </summary>
        public void Update()
        {
            if (StateEvents.TryGetValue(cur_GameState, out var cur_State))
            {
                cur_State.update?.Invoke();
            }
        }
    }



    /// <summary>
    /// ���� ���� �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public DeckEventHandler about_Deck;
    [System.Serializable]
    public class DeckEventHandler
    {
        /// <summary>
        /// ������ ī�带 ���� ���� �̺�Ʈ
        /// ex�ɷ� ) ������ ī�带 ���� ������ ���ݷ��� �����մϴ�.
        /// </summary>
        public Del_NoRet_1_Params<Card> OnDrawCard_From_Deck;


    }

    /// <summary>
    /// �÷��̾� �ڵ忡 ���� �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public HandEventHandler about_Hand;
    [System.Serializable]
    public class HandEventHandler
    {
        /// <summary>
        /// �ڵ忡 �ִ� ī�带 ���� ��
        /// </summary>
        public Del_NoRet_1_Params<Card> OnThrowCard_From_Hand;
        /// <summary>
        /// �ڵ忡 �ִ� ī�尡 ����� �� (�����°Ŷ� �ٸ�)
        /// </summary>
        public Del_NoRet_1_Params<Card> OnBlowCard_From_Hand;
        /// <summary>
        /// �ڵ忡 ī�尡 ���� ��
        /// </summary>
        public Del_NoRet_1_Params<Card> OnGetCard_To_Hand;
    }

    /// <summary>
    /// ī�� �������뿡 ���� �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public TrashEventHandler about_Trash;
    [System.Serializable]
    public class TrashEventHandler
    {
        /// <summary>
        /// ���������� ī�带 ���� ���
        /// </summary>
        public Del_NoRet_1_Params<Card> OnGetCard;



        /// <summary>
        /// ���������� ����� ���
        /// </summary>
        public Del_NoRet_NoParams OnClear_TrashCan;
    }



    /// <summary>
    /// �ʵ忡 ���� �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public FieldEventHanlder about_Field;
    [System.Serializable]
    public class FieldEventHanlder
    {
        /// <summary>
        /// ī�带 �ش� ������ �� ��쿡 �߻��ϴ� �̺�Ʈ
        /// </summary>
        public Del_NoRet_2_Params<Card, Field.Zone> OnLocatedCard_At_Zone;
        /// <summary>
        /// �ش籸���� �ִ� ī�带 ���� ��� �߻��ϴ� �̺�Ʈ
        /// </summary>
        public Del_NoRet_2_Params<Card, Field.Zone> OnRemoveCard_From_Zone;

        /// <summary>
        /// �ش� ������ Ư���� ī�尡 ���� ��� �߻��ϴ� �̺�Ʈ (���� ���̶� �ٸ�)
        /// </summary>
        public Del_NoRet_2_Params<Card, Field.Zone> OnCreateCard;


    }


    /// <summary>
    /// ī�忡 ���� �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public CardEventHandler about_Card;
    [System.Serializable]
    public class CardEventHandler
    {

    
    }


    /// <summary>
    /// ��Ʋ ������ �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public BattleEventHandler about_Battle;
    [System.Serializable]
    public class BattleEventHandler
    {
        /// <summary>
        /// �������� ���� �� �߰������� �߻��ϴ� �̺�Ʈ : ���� �⼱����
        /// </summary>
        public Del_NoRet_NoParams OnEnter_BattlePhase;
        /// <summary>
        /// �������� ���ī�忡 ���� ���� ����, ����ڴ� nuil�� �ɼ��� ����.
        /// </summary>
        public Del_NoRet_2_Params<Card, Card> OnTry_Attack_Card;
        /// <summary>
        /// ������ ������ ����� �̺�Ʈ (�������� ���������� �����´�)
        /// </summary>
        public Del_NoRet_2_Params<Card, Card> OnSuccessAttack;
        /// <summary>
        /// ������ ������ ����� �̺�Ʈ (�������� ���ѱ��)
        /// </summary>
        public Del_NoRet_2_Params<Card, Card> OnFailAttack;
        /// <summary>
        /// ��Ʋ�� ����� �� ���� ������� ����Ǳ� �� �߻��ϴ� �̺�Ʈ
        /// �÷��̾��� ������ �������� ���� �����Ѵ�.
        /// </summary>
        public Del_NoRet_NoParams OnExit_BattlePhase;
        /// <summary>
        /// ����Ŀ�� ī�尡 ���� �� �߻��ϴ� �̺�Ʈ
        /// </summary>
        public Del_NoRet_NoParams OnAttackCard_Null;
        /// <summary>
        /// ������� ī�尡 ���� �� �߻��ϴ� �̺�Ʈ
        /// </summary>
        public Del_NoRet_NoParams OnDefenceCard_Null;


    }

    /// <summary>
    /// �÷��̾� ����/������� ���� �̺�Ʈ ���
    /// </summary>
    /// 
    [SerializeField]
    public BuffEventHandler about_Aura;
    [System.Serializable]
    public class BuffEventHandler
    {
        public Dictionary<Buff, Del_NoRet_NoParams> OnBuff_On;

        public Dictionary<Buff, Del_NoRet_NoParams> OnBuff_Off;

        public Dictionary<Debuff, Del_NoRet_NoParams> OnDebuff_On;

        public Dictionary<Debuff, Del_NoRet_NoParams> OnDebuff_Off;

        public BuffEventHandler()
        {
            OnBuff_On = new Dictionary<Buff, Del_NoRet_NoParams>();
            OnBuff_Off = new Dictionary<Buff, Del_NoRet_NoParams>();

            OnDebuff_On = new Dictionary<Debuff, Del_NoRet_NoParams>();

            OnDebuff_Off = new Dictionary<Debuff, Del_NoRet_NoParams>();
        }

        public void GetPlayerBuff(GamePlayer target, Buff buff_Type)
        {
            if(OnBuff_On.ContainsKey(buff_Type))
            {
                OnBuff_On.TryGetValue(buff_Type, out var value);
                value?.Invoke();
            }
            else
            {
                OnBuff_On.Add(buff_Type, ()=>buff_Type.Init(target));
                OnBuff_On.TryGetValue(buff_Type, out var value);
                value?.Invoke();
            }
        }

        public void GetPlayerDebuff(GamePlayer target, Debuff debuff_Type)
        {
            if (OnDebuff_On.ContainsKey(debuff_Type))
            {
                OnDebuff_On.TryGetValue(debuff_Type, out var value);
                value?.Invoke();
            }
            else
            {
                OnDebuff_On.Add(debuff_Type, () => debuff_Type.Init(target));
                OnDebuff_On.TryGetValue(debuff_Type, out var value);
                value?.Invoke();
            }
        }
    }

    

}
