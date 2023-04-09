using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내의 모든 이벤트를 관리, 델리게이트를 관리합니다.
/// 델리게이트 등록, 해제만 잘 관리해주세요.
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
    /// 게임매니저에 대한 이벤트 목록
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
        /// 해당 게임스테이트에 매개변수 메서드를 등록합니다.
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
        /// 해당 게임스테이트에 매개변수 메서드를 제거합니다.
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
        /// 게임상태 업데이트문
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
    /// 덱에 대한 이벤트 목록
    /// </summary>
    public DeckEventHandler about_Deck;
    public class DeckEventHandler
    {
        /// <summary>
        /// 덱에서 카드를 뽑을 때의 이벤트
        /// ex능력 ) 덱에서 카드를 뽑을 때마다 공격력이 증가합니다.
        /// </summary>
        public Del_NoRet_1_Params<Card> OnDrawCard_From_Deck;


    }

    /// <summary>
    /// 플레이어 핸드에 대한 이벤트 목록
    /// </summary>
    public HandEventHandler about_Hand;
    public class HandEventHandler
    {
        /// <summary>
        /// 핸드에 있는 카드를 버릴 때
        /// </summary>
        public Del_NoRet_1_Params<Card> OnThrowCard_From_Hand;
        /// <summary>
        /// 핸드에 있는 카드가 사라질 때 (버리는거랑 다름)
        /// </summary>
        public Del_NoRet_1_Params<Card> OnBlowCard_From_Hand;
        /// <summary>
        /// 핸드에 카드가 생길 때
        /// </summary>
        public Del_NoRet_1_Params<Card> OnGetCard_To_Hand;
    }

    /// <summary>
    /// 카드 쓰레기통에 대한 이벤트 목록
    /// </summary>
    public TrashEventHandler about_Trash;
    public class TrashEventHandler
    {
        /// <summary>
        /// 쓰레기통이 카드를 쌓을 경우
        /// </summary>
        public Del_NoRet_1_Params<Card> OnGetCard;



        /// <summary>
        /// 쓰레기통이 비워질 경우
        /// </summary>
        public Del_NoRet_NoParams OnClear_TrashCan;
    }



    /// <summary>
    /// 필드에 대한 이벤트 목록
    /// </summary>
    public FieldEventHanlder about_Field;
    public class FieldEventHanlder
    {
        /// <summary>
        /// 카드를 해당 구역에 낼 경우에 발생하는 이벤트
        /// </summary>
        public Del_NoRet_2_Params<Card, Field.Zone> OnLocatedCard_At_Zone;
        /// <summary>
        /// 해당구역에 있는 카드를 없앨 경우 발생하는 이벤트
        /// </summary>
        public Del_NoRet_2_Params<Card, Field.Zone> OnRemoveCard_From_Zone;

        /// <summary>
        /// 해당 구역에 특정한 카드가 생길 경우 발생하는 이벤트 (내는 것이랑 다름)
        /// </summary>
        public Del_NoRet_2_Params<Card, Field.Zone> OnCreateCard;


    }


    /// <summary>
    /// 카드에 대한 이벤트 목록
    /// </summary>
    public CardEventHandler about_Card;
    public class CardEventHandler
    {

    
    }


    /// <summary>
    /// 배틀 도중의 이벤트 목록
    /// </summary>
    public BattleEventHandler about_Battle;
    public class BattleEventHandler
    {
        /// <summary>
        /// 배들페이즈에 진입 시 추가적으로 발생하는 이벤트 : 예는 기선제압
        /// </summary>
        public Del_NoRet_NoParams OnEnter_BattlePhase;
        /// <summary>
        /// 공격자의 방어카드에 대해 공격 실행, 방어자는 nuil이 될수도 있음.
        /// </summary>
        public Del_NoRet_2_Params<Card, Card> OnTry_Attack_Card;
        /// <summary>
        /// 공격이 성공한 경우의 이벤트 (공격턴을 지속적으로 가져온다)
        /// </summary>
        public Del_NoRet_2_Params<Card, Card> OnSuccessAttack;
        /// <summary>
        /// 공격이 실패한 경우의 이벤트 (공격턴을 빼앗긴다)
        /// </summary>
        public Del_NoRet_2_Params<Card, Card> OnFailAttack;
        /// <summary>
        /// 배틀이 종료된 뒤 게임 스테이즈가 변경되기 전 발생하는 이벤트
        /// 플레이어의 마지막 공격턴을 또한 저장한다.
        /// </summary>
        public Del_NoRet_NoParams OnExit_BattlePhase;

    }

}
