using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 게임 내의 모든 이벤트를 관리, 델리게이트를 관리합니다.
/// 델리게이트 등록, 해제만 잘 관리해주세요.
/// </summary>
/// 
[System.Serializable]
public class EventTrigger
{
    public void Init()
    {
        about_Prepare = new PrepareEventHandler();
        about_GameManager = new GameManagerEventHanler();
        about_Aura = new BuffEventHandler();
    }
    /// <summary>
    /// 게임매니저에 대한 이벤트 목록
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
    /// 
    [SerializeField]
    public PrepareEventHandler about_Prepare;
    [System.Serializable]
    public class PrepareEventHandler
    {
        /// <summary>
        /// 덱에서 카드를 뽑을 때의 이벤트
        /// ex능력 ) 덱에서 카드를 뽑을 때마다 공격력이 증가합니다.
        /// </summary>
        public Del_NoRet_1_Params<Card> OnDrawCard_From_Deck;

        /// <summary>
        /// 덱에 새로운 카드를 생성 할 때의 이벤트
        /// </summary>
        public Del_NoRet_1_Params<Card> OnCreateCard_OnDeck;

        /// <summary>
        /// 덱에서 핸드로 카드를 가져올 때의 이벤트
        /// </summary>
        public Del_NoRet_1_Params<Card> OnDrawOnDeck;

        /// <summary>
        /// 핸드에 새로운 카드를 생성할 때의 이벤트
        /// </summary>
        public Del_NoRet_1_Params<Card> OnCreateCard_OnHand;

        /// <summary>
        /// 핸드에서 필드로 카드를 낼 때의 이벤트
        /// </summary>
        public Del_NoRet_2_Params<Card, Field.Zone> OnDrawField;

        /// <summary>
        /// 핸드에서 필드로 카드를 내는 것을 실패할 때의 이벤트
        /// </summary>
        public Del_NoRet_1_Params<Card> FailDrawOnField;

        /// <summary>
        /// 핸드에서 쓰레기통으로 카드를 버릴 때의 이벤트
        /// </summary>
        public Del_NoRet_1_Params<Card> OnThrowAwayFromHand;

        /// <summary>
        /// 필드에서 쓰레기통으로 카드를 버릴 때의 이벤트
        /// </summary>
        public Del_NoRet_1_Params<Card> OnThrowAwayFromField;

        /// <summary>
        /// 카드가 완전히 게임에서 사라질 때의 이벤트
        /// </summary>
        public Del_NoRet_1_Params<Card> OnCardOutofTheGame;

        /// <summary>
        /// 쓰레기통으로부터 카드를 덱으로 다시 넣을 때
        /// </summary>
        public Del_NoRet_1_Params<Card> OnGetCardToDeck_FromTrash;

        /// <summary>
        /// 쓰레기통으로부터 카드를 핸드로 다시 넣을 때
        /// </summary>
        public Del_NoRet_1_Params<Card> OnGetCardToHand_FromTrash;



    }



    /// <summary>
    /// 배틀 도중의 이벤트 목록
    /// </summary>
    /// 
    [SerializeField]
    public BattleEventHandler about_Battle;
    [System.Serializable]
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
        /// <summary>
        /// 어태커의 카드가 없을 때 발생하는 이벤트
        /// </summary>
        public Del_NoRet_NoParams OnAttackCard_Null;
        /// <summary>
        /// 디펜더의 카드가 없을 때 발생하는 이벤트
        /// </summary>
        public Del_NoRet_NoParams OnDefenceCard_Null;
    }

    /// <summary>
    /// 플레이어 버프/디버프에 대한 이벤트 목록
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
