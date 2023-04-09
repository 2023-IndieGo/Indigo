using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ܰ迡���� �������� �� ��������� �����ϴ� ������Ʈ
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
        /// �ش� ���ӽ�����Ʈ�� �Ű����� �޼��带 ����մϴ�.
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
        /// �ش� ���ӽ�����Ʈ�� �Ű����� �޼��带 �����մϴ�.
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
        /// ���ӻ��� ������Ʈ��
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
                //BattleConnecter�κ��� �÷��̾������� �޾ƿ��� �õ�
                //��ƲĿ���Ϳ����� �÷��̾� ������ �޾ƿȰ� ���ÿ� ��Ʋ����� ��Ÿ���� �ٲ�
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
                //��Ʋ����� ����Ǹ� ��Ʋ ��ü ����� �������� �ٲٸ� ���� �ٽ� ��Ʋ������ ���� �� ���ú��� ������ �� �ֵ���
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
