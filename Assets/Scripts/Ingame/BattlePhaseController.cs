using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 원격으로부터 각 플레이어들의 정보를 받아와 로컬에서 배틀결과를 처리합니다.
/// </summary>
public class BattlePhaseController : MonoBehaviour
{
    public enum BattlePhaseState
    {
        Setting,
        Start,
        End
    }
    public int currentRound = 0;
    public BattlePhaseState _cur_State;
    public BattlePhaseState cur_State => _cur_State;
    public BattlePhaseEventTrigger events;
    public Dictionary<BattlePhaseState, BattlePhaseEventTrigger> StateEvents;
    public class BattlePhaseEventTrigger
    {
        private BattlePhaseController controller;
        public Del_NoRet_NoParams start;
        public Del_NoRet_NoParams update;
        public Del_NoRet_NoParams exit;

        public BattlePhaseEventTrigger(Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
        {
            this.start += start;
            this.update += update;
            this.exit += exit;
        }

        public BattlePhaseEventTrigger(BattlePhaseController controller)
        {
            this.controller = controller;
        }

        public void SetGameState(BattlePhaseState targetState)
        {
            if (controller.StateEvents.TryGetValue(targetState, out var cur_State))
            {
                cur_State.exit?.Invoke();
            }
            if (controller.StateEvents.TryGetValue(targetState, out var next_State))
            {
                next_State.start?.Invoke();
            }
        }

        /// <summary>
        /// ???? ?????????????? ???????? ???????? ??????????.
        /// </summary>
        /// <param name="targetStage"></param>
        /// <param name="start"></param>
        /// <param name="update"></param>
        /// <param name="exit"></param>
        public void AddEventOnState(BattlePhaseState targetState, Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
        {
            if (controller.StateEvents.TryGetValue(targetState, out var value))
            {
                value.start += start;
                value.update += update;
                value.exit += exit;
            }
            else
            {
                controller.StateEvents.Add(targetState, new BattlePhaseEventTrigger(start, update, exit));
            }
        }

        /// <summary>
        /// ???? ?????????????? ???????? ???????? ??????????.
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="start"></param>
        /// <param name="update"></param>
        /// <param name="exit"></param>
        public void DeleteEventOnState(BattlePhaseState targetState, Del_NoRet_NoParams start = null, Del_NoRet_NoParams update = null, Del_NoRet_NoParams exit = null)
        {
            if (controller.StateEvents.TryGetValue(targetState, out var value))
            {
                value.start -= start;
                value.update -= update;
                value.exit -= exit;
            }
        }
    }

    public BattleConnecter connecter;
    public GamePlayer myPlayer;
    public GamePlayer otherPlayer;
    public EventTrigger.BattleEventHandler battleEventHanlder;

    public void Awake()
    {
        StateEvents = new Dictionary<BattlePhaseState, BattlePhaseEventTrigger>();
        StateEvents.Add(BattlePhaseState.Setting, new BattlePhaseEventTrigger());
        StateEvents.Add(BattlePhaseState.Start, new BattlePhaseEventTrigger());
        StateEvents.Add(BattlePhaseState.End, new BattlePhaseEventTrigger());
    }

    /// <summary>
    /// 게임매니저에 이벤트 등록
    /// </summary>
    public void Init()
    {
        //Enter GameManager=>State.battle event subscribe
        GameManager.instance.events.about_GameManager.AddEventOnState(GameState.Battle,
            //start
            () =>
            {
                SetBattlePhase(BattlePhaseState.Setting);

            },
            //update
            () =>
            {


            },

            //exit
            () =>
            {

            }
            );

        //Enter battleState => Setting
        events.AddEventOnState(BattlePhaseState.Setting,
            //start
            () =>
            {
                currentRound = 0;
                otherPlayer = null;
                myPlayer = null;
            },
            () =>
            {
                otherPlayer = connecter.GetOtherPlayer();
                myPlayer = connecter.GetMyPlayer();
                if (otherPlayer != null && myPlayer != null)
                {
                    SetBattlePhase(BattlePhaseState.Start);
                }
            }, //Update
            () => { }  //Exit
            );

        //Enter battleState => Start
        events.AddEventOnState(BattlePhaseState.Start,
            () =>
            {
                GameManager.instance.events.about_Battle.OnEnter_BattlePhase?.Invoke();
                //배틀 시작
                StartBattleMethod_Callback();

            }, //start
            () => { }, //Update
            () => { }  //Exit
            );


        //Enter battleState => End
        events.AddEventOnState(BattlePhaseState.End,
            () =>
            {
                GameManager.instance.events.about_Battle.OnExit_BattlePhase?.Invoke();
                GameManager.instance.SetGameState(GameState.Prepare);
            }, //start
            () => { }, //Update
            () => { }  //Exit
            );
    }

    public void Update()
    {
        if (GameManager.instance.cur_GameState != GameState.Battle) return;
        if (StateEvents.TryGetValue(_cur_State, out var cur_State))
        {
            cur_State.update?.Invoke();
        }
    }

    public void SetBattlePhase(BattlePhaseState state)
    {
        events.SetGameState(state);
        _cur_State = state;
    }

    public void StartBattleMethod_Callback()
    {
        var attackPlayer = connecter.GetPlayer_CompareTurnType(TurnType.Attack_Turn);
        var defencePlayer = connecter.GetPlayer_CompareTurnType(TurnType.Defence_Turn);
        int max_round = 10;

        //라운드가 10라운드 이하일 때, 혹은 둘다 카드가 비어있을 때까지의 배틀 시뮬레이션
        while (currentRound < max_round || (attackPlayer.field.zones[currentRound][0].currentCard == null && defencePlayer.field.zones[currentRound][1].currentCard == null))
        {
            attackPlayer = connecter.GetPlayer_CompareTurnType(TurnType.Attack_Turn);
            defencePlayer = connecter.GetPlayer_CompareTurnType(TurnType.Defence_Turn);
            TryBattle(attackPlayer.field.zones[currentRound][0].currentCard, defencePlayer.field.zones[currentRound][1].currentCard, out var result);
            currentRound++;
        }
        //시뮬레이션 돌리기를 마치면 배틀페이즈를 종료합니다.
        SetBattlePhase(BattlePhaseState.End);
    }

    /// <summary>
    /// 현재 배틀은 두 플레이어 중 한쪽이라도 카드를 낸 경우에만 배틀을 시도합니다.
    /// </summary>
    /// <param name="Attacker"></param>
    /// <param name="Defender"></param>
    /// <param name="result"></param>
    public void TryBattle(Card Attacker, Card Defender, out CardBattleResult result)
    {
        var AttackPlayer = connecter.GetPlayer_CompareTurnType(TurnType.Attack_Turn);
        var DefencePlayer = connecter.GetPlayer_CompareTurnType(TurnType.Defence_Turn);
        //각각의 플레이어로부터 최근에 낸 카드의 카드타입을 받아옵니다.
        CardType attackerType = connecter.GetPlayer_CompareTurnType(TurnType.Attack_Turn).lastOpenCardType;
        CardType defenderType = connecter.GetPlayer_CompareTurnType(TurnType.Defence_Turn).lastOpenCardType;
        if (Attacker == null)
        {
            defenderType = Defender.type;
            GameManager.instance.events.about_Battle.OnAttackCard_Null?.Invoke();

        }
        else if (Defender == null)
        {
            attackerType = Attacker.type;
            GameManager.instance.events.about_Battle.OnDefenceCard_Null?.Invoke();
        }
        //둘다 카드가 있음
        else
        {
            attackerType = Attacker.type;
            defenderType = Defender.type;
        }
        result = GetBattleResult(attackerType, defenderType);
        if (result == CardBattleResult.Success)
        {
            GameManager.instance.events.about_Battle.OnSuccessAttack(Attacker, Defender);
            Attacker.AttackEffect(DefencePlayer, AttackPlayer);
            Attacker.DefenceFail(AttackPlayer, DefencePlayer);
        }
        else
        {
            GameManager.instance.events.about_Battle.OnFailAttack(Attacker, Defender);
            Attacker.AttackFail(DefencePlayer, AttackPlayer);
            Attacker.DefenceEffect(AttackPlayer, DefencePlayer);
            //턴타입을 바꿔줍니다.
            connecter.GetPlayer_CompareTurnType(TurnType.Attack_Turn).current_TurnType = TurnType.Defence_Turn;
            connecter.GetPlayer_CompareTurnType(TurnType.Defence_Turn).current_TurnType = TurnType.Attack_Turn;
        }
    }

    public void GetNextBattleCard()
    {

    }

    /// <summary>
    /// 무승부도 공격 성공입니다.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    private CardBattleResult GetBattleResult(CardType attacker, CardType defender)
    {
        switch (attacker)
        {
            case CardType.Scissors:
                {
                    if (defender == CardType.Paper || defender == CardType.Scissors)
                        return CardBattleResult.Success;
                    else return CardBattleResult.Fail;
                }
            case CardType.Rock:
                {
                    if (defender == CardType.Scissors || defender == CardType.Rock)
                        return CardBattleResult.Success;
                    else return CardBattleResult.Fail;
                }
            default:       //보
                {
                    if (defender == CardType.Rock || defender == CardType.Paper)
                        return CardBattleResult.Success;
                    else return CardBattleResult.Fail;
                }
        }
    }
}