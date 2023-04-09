using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ��� �̺�Ʈ�� ����, ��������Ʈ�� �����մϴ�.
/// ��������Ʈ ���, ������ �� �������ּ���.
/// </summary>
public class EventTrigger
{
    public EventTrigger()
    {
        about_Battle = new BattleEventHandler();
        about_Deck = new DeckEventHandler();
        about_GameManager = new GameManagerEventHanler();
        about_Trash = new TrashEventHandler();
        about_Hand = new HandEventHandler();
        about_Card = new CardEventHandler();
    }

    /// <summary>
    /// ���ӸŴ����� ���� �̺�Ʈ ���
    /// </summary>
    public GameManagerEventHanler about_GameManager;
    public class GameManagerEventHanler
    {
        private GameState cur_GameState;
        public Dictionary<GameState, GameStateEventTrigger> StateEvents;
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
    public DeckEventHandler about_Deck;
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
    public HandEventHandler about_Hand;
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
    public TrashEventHandler about_Trash;
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
    public FieldEventHanlder about_Field;
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
    public CardEventHandler about_Card;
    public class CardEventHandler
    {

    
    }


    /// <summary>
    /// ��Ʋ ������ �̺�Ʈ ���
    /// </summary>
    public BattleEventHandler about_Battle;
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

    }

}
