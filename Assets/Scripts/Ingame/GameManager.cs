using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 인게임상태에서만 새로 생성 후 초기화(Init())되도록
/// 인게임이 종료되면 GameManager
/// null로 바꾸고 이벤트를 모두 날려야함.
/// </summary>
public class GameManager
{
    #region Field
    private GameState _cur_GameState;
    /// <summary>
    /// 현재 게임의 상태입니다.
    /// </summary>
    public GameState cur_GameState
    {
        get => cur_GameState;
    }

    /// <summary>
    /// 인게임인지 아닌지 나타내는 불린 변수:
    /// true : 인게임 ___/___
    /// false : 인게임이 아님
    /// </summary>
    public bool isGaming= false;

    private float _prepare_CurTime;
    /// <summary>
    /// 준비단계의 진행되고 있는 현재시간입니다.
    /// </summary>
    public float prepare_CurTime;

    /// <summary>
    /// 준비단계 진입 시 각 턴마다의 주어지는 시간입니다.
    /// </summary>
    public float _prepare_maxTime => default_PrepareTime * prepareTime_IncreaseRate * current_Turn;

    private float _default_PrepareTime;
    /// <summary>
    /// 기본 준비단계 시간입니다.
    /// </summary>
    public float default_PrepareTime;

    private float _prepareTime_IncreaseRate = 1.1f;
    /// <summary>
    /// 매 턴마다의 준비시간 증가비율입니다.
    /// </summary>
    public float prepareTime_IncreaseRate;

    private uint _current_Turn = 1;
    /// <summary>
    /// 현재 턴이 몇 턴인지 알려주는 필드변수입니다.
    /// </summary>
    public uint current_Turn => _current_Turn;

    /// <summary>
    /// 현재 게임의 플레이어 데이터 배열입니다. [0] = 나, [1] = 상대방
    /// </summary>
    public GamePlayer[] players = new GamePlayer[2];
    #endregion


    #region Properties

    #endregion


    #region Events
    Dictionary<GameState, GameStateEventTrigger> StateEvents;
    class GameStateEventTrigger
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

    #endregion


    #region Constructor
    public GameManager(GamePlayer me, GamePlayer otherPlayer)
    {
        this.players[0] = me;
        this.players[1] = otherPlayer;
    }
    #endregion


    #region Public Methods
    public void Init()
    {
        StateEvents.Add(GameState.Start, new GameStateEventTrigger());
        StateEvents.Add(GameState.Prepare, new GameStateEventTrigger());
        StateEvents.Add(GameState.Battle, new GameStateEventTrigger());
        StateEvents.Add(GameState.End, new GameStateEventTrigger());
    }
    public void Destroy()
    {
        for (int i = 0; i < (int)GameState.End+1; i++)
        {
            StateEvents[(GameState)i] = null;
        }
    }
    /// <summary>
    /// 게임의 상태를 변경합니다.
    /// 변경된 상태에 따른 이벤트가 실행됩니다.
    /// </summary>
    /// <param name="targetState"></param>
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
        _cur_GameState = targetState;
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
    #endregion


    #region Private/Protected Methods
    #endregion


}
