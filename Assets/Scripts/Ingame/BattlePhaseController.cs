using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투단계에서의 전투진행 및 전투결과를 관리하는 컴포넌트
/// </summary>
public class BattlePhaseController : MonoBehaviour
{
    public enum BattlePhaseState
    {
        Setting,
        Start,
        End
    }

    public BattleConnecter connecter;
    public BattlePhaseState _cur_State;
    public BattlePhaseEventTrigger events;
    public BattlePhaseState cur_State => _cur_State;
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
        /// 해당 게임스테이트에 매개변수 메서드를 등록합니다.
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
        /// 해당 게임스테이트에 매개변수 메서드를 제거합니다.
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

        /// <summary>
        /// 게임상태 업데이트문
        /// </summary>
        public void Update()
        {

        }
    }

    public GamePlayer myPlayer;
    public GamePlayer otherPlayer;


    public void Awake()
    {
        StateEvents = new Dictionary<BattlePhaseState, BattlePhaseEventTrigger>();
        StateEvents.Add(BattlePhaseState.Setting, new BattlePhaseEventTrigger());
        StateEvents.Add(BattlePhaseState.Start, new BattlePhaseEventTrigger());
        StateEvents.Add(BattlePhaseState.End, new BattlePhaseEventTrigger());
    }


    public void Init()
    {
        GameManager.instance.events.about_GameManager.AddEventOnState(GameState.Battle,
            //start
            () =>
            {
                SetBattlePhase(BattlePhaseState.Setting);
                //BattleConnecter로부터 플레이어정보를 받아오기 시도
                //배틀커넥터에서는 플레이어 정보를 받아옴과 동시에 배틀페이즈를 스타르로 바꿈
                myPlayer = GameManager.instance.players[0];
                otherPlayer = connecter.GetOtherPlayer();
                GameManager.instance.players[1] = otherPlayer;
                if(otherPlayer != null)
                {
                    SetBattlePhase(BattlePhaseState.Start);
                }
            },
            //update
            () =>
            {

            },

            //exit
            () =>
            {
                //배틀페이즈가 종료되면 배틀 자체 페이즈를 세팅으로 바꾸며 만일 다시 배틀페이즈 진입 시 세팅부터 시작할 수 있도록
                SetBattlePhase(BattlePhaseState.Setting);
            }
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

    public void TryBattle(Card Attacker, Card Defender)
    {

    }

    private CardBattleResult GetBattleResult_Attaker()
    {
        return CardBattleResult.Success;
    }
}
